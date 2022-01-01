using System.Globalization;
using System.Text.RegularExpressions;
using ZCalc.Elements;

namespace ZCalc.Formatters;

public class ZMatrixParser
{
    private readonly ElementSymbols _elementSymbols = new();

    private static readonly Regex ElementIndexRemoved = new Regex(@"(\w)\d*", RegexOptions.Compiled);
    
    public ZMatrix Parse(string text)
    {
        string[] lines = text.Split("\n");

        Dictionary<string, double> @params = 
            GetParams(lines).ToDictionary(param => param.name, param => param.value);
        List<ZRow> rows = GetCoords(lines, @params).ToList();

        return new ZMatrix
        {
            Rows = rows
        };
    }

    private IEnumerable<ZRow> GetCoords(IEnumerable<string> lines, IReadOnlyDictionary<string, double> @params)
    {
        Dictionary<string, int> aliases = new();
        int index = 0;
        foreach (string line in lines)
        {
            if (String.IsNullOrWhiteSpace(line) || line.Contains("="))
            {
                yield break;
            }
            index++;

            string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (!TryParseElement(parts[0], index, aliases, out int element))
            {
                throw new Exception($"Cannot parse element on line: {line}");
            }

            if (parts.Length < 3)
            { 
                yield return element;
                continue;
            }
            
            if (!TryParseReference(parts[1], aliases, out int refD))
            {
                throw new Exception($"Cannot parse distance reference on line: {line}");
            }
            
            if (!TryParseValue(parts[2], @params, out double distance))
            {
                throw new Exception($"Cannot parse distance on line: {line}");
            }
            
            if (parts.Length < 5)
            { 
                yield return (element, refD, distance);
                continue;
            }
            
            if (!TryParseReference(parts[3], aliases, out int refA))
            {
                throw new Exception($"Cannot parse angle reference on line: {line}");
            }
            
            if (!TryParseValue(parts[4], @params, out double angle))
            {
                throw new Exception($"Cannot parse angle on line: {line}");
            }
            
            if (parts.Length < 7)
            { 
                yield return (element, refD, distance, refA, angle);
                continue;
            }
            
            if (!TryParseReference(parts[5], aliases, out int refT))
            {
                throw new Exception($"Cannot parse dihedral reference on line: {line}");
            }
            
            if (!TryParseValue(parts[6], @params, out double dihedral))
            {
                throw new Exception($"Cannot parse dihedral on line: {line}");
            }
            
            yield return (element, refD, distance, refA, angle, refT, dihedral);
        }
    }

    private bool TryParseElement(string val, int index, Dictionary<string, int> aliases, out int element)
    {
        if (Int32.TryParse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out element))
        {
            return true;
        }

        string symbol = ElementIndexRemoved.Replace(val, "$1");

        if (symbol != val)
        {
            aliases.Add(val, index);
        }
        
        if (_elementSymbols.GetElementBySymbol(symbol) is { } elementBySymbol)
        {
            element = elementBySymbol;
            return true;
        }

        return false;
    }
    
    private bool TryParseReference(string val, Dictionary<string, int> aliases, out int index)
    {
        if (Int32.TryParse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out index))
        {
            return true;
        }

        if (aliases.TryGetValue(val, out index))
        {
            return true;
        }

        return false;
    }
    
    private bool TryParseValue(string val, IReadOnlyDictionary<string, double> @params, out double result)
    {
        if (Double.TryParse(val, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result))
        {
            return true;
        }

        string positiveVal = val.TrimStart('-');

        if (@params.TryGetValue(positiveVal, out result))
        {
            if (val != positiveVal)
            {
                result *= -1;
            }

            return true;
        }

        return false;
    }
    
    private IEnumerable<(string name, double value)> GetParams(IEnumerable<string> lines)
    {
        foreach (string line in lines)
        {
            string[] parts = line.Split("=");
            if (parts.Length >= 2)
            {
                if (double.TryParse(parts[1].Trim(), out double value))
                {
                    string name = parts[0].Trim();
                    yield return (name, value);
                }
            }
        }
    }
}