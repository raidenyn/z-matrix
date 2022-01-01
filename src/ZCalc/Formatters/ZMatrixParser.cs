using System.Globalization;
using ZCalc.Elements;

namespace ZCalc.Formatters;

public class ZMatrixParser
{
    private readonly ElementSymbols _elementSymbols = new();
    
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
        foreach (string line in lines)
        {
            if (String.IsNullOrWhiteSpace(line) || line.Contains("="))
            {
                yield break;
            }

            string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (!TryParseElement(parts[0], out int element))
            {
                throw new Exception($"Cannot parse element on line: {line}");
            }

            if (parts.Length < 3)
            { 
                yield return element;
                continue;
            }
            
            if (!Int32.TryParse(parts[1], out int refD))
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
            
            if (!Int32.TryParse(parts[3], out int refA))
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
            
            if (!Int32.TryParse(parts[5], out int refT))
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

    private bool TryParseElement(string val, out int element)
    {
        if (Int32.TryParse(val, out element))
        {
            return true;
        }
        if (_elementSymbols.GetElementBySymbol(val) is { } elementBySymbol)
        {
            element = elementBySymbol;
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