using System.Text;

namespace ZCalc.Formatters;

public class CartesianMatrixFormatter
{
    public string Print(CartesianMatrix cartesian)
    {
        StringBuilder sb = new StringBuilder();

        foreach (CartesianRow row in cartesian.Rows)
        {
            sb.Append(row.Element.ToString().PadRight(4));
            
            sb.Append(row.Point.X.ToString("F9").PadLeft(15));
            sb.Append(row.Point.Y.ToString("F9").PadLeft(15));
            sb.Append(row.Point.Z.ToString("F9").PadLeft(15));

            sb.AppendLine();
        }

        return sb.ToString();
    }
}