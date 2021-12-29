namespace ZCalc;

public record CartesianMatrix
{
    public List<CartesianRow> Rows { get; init; } = new();
    
    public override string ToString()
    {
        return String.Join(Environment.NewLine, Rows);
    }
}

public record CartesianRow
{
    public int Element { get; set; }
    
    public Point Point { get; set; }

    public static implicit operator CartesianRow((int element, double x, double y, double z) row) =>
        new()
        {
            Element = row.element, 
            Point = new Point(row.x, row.y, row.z)
        };
    
    public override string ToString()
    {
        return $"{Element}  {Point}";
    }
}
