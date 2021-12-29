using ZCalc.Matrix;

namespace ZCalc
{
    public readonly struct Point
    {
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; init; }
        
        public double Y { get; init; }
        
        public double Z { get; init; }

        public override string ToString()
        {
            return $"{X:F9}, {Y:F9}, {Z:F9}";
        }

        public static implicit operator Vector(Point point) => (point.X, point.Y, point.Z);

        public static implicit operator Point(Vector vector) => new(Math.Round(vector.X, 9), Math.Round(vector.Y, 9), Math.Round(vector.Z, 9));
    }
}