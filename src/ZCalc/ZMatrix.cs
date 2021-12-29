namespace ZCalc;

public record ZMatrix
{
    public List<ZRow> Rows { get; init; } = new();
    
    public override string ToString()
    {
        var parts = new List<string>(Rows.Capacity);

        foreach (ZRow row in Rows)
        {
            parts.Add(row.ToString());
        }

        return String.Join(Environment.NewLine, parts);
    }
}

public record ZRow
{
    public int Element { get; set; }
    
    public ZDistance? Distance { get; set; }
    
    public ZAngle? Angle { get; set; }
    
    public ZAngle? Dihedral { get; set; }
    
    public static implicit operator ZRow(int element) =>
        new()
        {
            Element = element
        };

    public static implicit operator ZRow((int element, int refD, double distance) row) =>
        new()
        {
            Element = row.element, 
            Distance = new() { Reference = row.refD, Value = row.distance }
        };

    public static implicit operator ZRow((int element, int refD, double distance, int refA, double angle) row) =>
        new()
        {
            Element = row.element,
            Distance = new() { Reference = row.refD, Value = row.distance },
            Angle = new() { Reference = row.refA, Value = new Angle { Degree = row.angle } }
        };

    public static implicit operator ZRow(
        (int element, int refD, double distance, int refA, double angle, int refT, double dihedral) row) =>
        new()
        {
            Element = row.element,
            Distance = new() { Reference = row.refD, Value = row.distance },
            Angle = new() { Reference = row.refA, Value = new Angle { Degree = row.angle } },
            Dihedral = new() { Reference = row.refT, Value = new Angle { Degree = row.dihedral } }
        };
    
    public override string ToString()
    {
        var parts = new List<string>(3);

        if (Distance != null)
        {
            parts.Add(Distance.ToString());
        }
        if (Angle != null)
        {
            parts.Add(Angle.ToString());
        }
        if (Dihedral != null)
        {
            parts.Add(Dihedral.ToString());
        }

        return $"{Element}  {String.Join("   ", parts)}";
    }
}

public record ZDistance
{
    public int Reference { get; set; }
    
    public double Value { get; set; }

    public override string ToString()
    {
        return $"{Reference}  {Value.ToString("F9")}";
    }
}

public record ZAngle
{
    public int Reference { get; set; }
    
    public Angle Value { get; set; }

    public override string ToString()
    {
        return $"{Reference}  {Value.Degree.ToString("F9")}";
    }
}

public readonly struct Angle
{
    private readonly double _radian;

    public double Degree
    {
        get => _radian * 180 / Math.PI;
        init => _radian = value * Math.PI / 180;
    }

    public double Radian
    {
        get => _radian;
        init => _radian = value;
    }
}
