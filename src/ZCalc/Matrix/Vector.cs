namespace ZCalc.Matrix;

public readonly struct Vector
{
    private readonly double[] _values = new double[4];

    public static readonly Vector OrtX = (1, 0, 0);
    
    public static readonly Vector OrtY = (0, 1, 0);
    
    public static readonly Vector OrtZ = (0, 0, 1);
    
    public double this[Coordinate coord]
    {
        get => _values[(int)coord];
        private set => _values[(int)coord] = value;
    }

    public double X => this[Coordinate.X];
    public double Y => this[Coordinate.Y];
    public double Z => this[Coordinate.Z];
    public double T => this[Coordinate.T];
    
    public Vector(double x, double y, double z, double t = 1)
    {
        this[Coordinate.X] = x;
        this[Coordinate.Y] = y;
        this[Coordinate.Z] = z;
        this[Coordinate.T] = t;
    }
    
    public void Deconstruct(out double x, out double y, out double z)
    {
        x = this[Coordinate.X];
        y = this[Coordinate.Y];
        z = this[Coordinate.Z];
    }
    
    public void Deconstruct(out double x, out double y, out double z, out double t)
    {
        x = this[Coordinate.X];
        y = this[Coordinate.Y];
        z = this[Coordinate.Z];
        t = this[Coordinate.T];
    }

    public static implicit operator Vector((double x, double y, double z) coords) =>
        new (coords.x, coords.y, coords.z, 1);

    public static implicit operator Vector((double x, double y, double z, double t) coords) =>
        new (coords.x, coords.y, coords.z, coords.t);
    
    public static Vector operator -(Vector vector) =>
        new (-vector.X, -vector.Y, -vector.Z, vector.T);
    public override string ToString()
    {
        return $"({this[Coordinate.X]},{this[Coordinate.Y]},{this[Coordinate.Z]},{this[Coordinate.T]})";
    }
}