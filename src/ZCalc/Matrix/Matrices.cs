namespace ZCalc.Matrix;

public static class Matrices
{
    public static IReadOnlyMatrix Translation(Vector vector)
    {
        return new Matrix
        {
            [Coordinate.X, Coordinate.X] = 1,
            [Coordinate.Y, Coordinate.Y] = 1,
            [Coordinate.Z, Coordinate.Z] = 1,
            [Coordinate.T, Coordinate.T] = 1,
            [Coordinate.X, Coordinate.T] = -vector.X,
            [Coordinate.Y, Coordinate.T] = -vector.Y,
            [Coordinate.Z, Coordinate.T] = -vector.Z
        };
    }
    
    public static IReadOnlyMatrix Rotate(Vector axe, double angle)
    {
        double cosA = Math.Cos(angle);
        double sinA = Math.Sin(angle);
        double cosA1 = 1 - cosA;

        (double x, double y, double z) = axe;
        
        return new Matrix
        {
            [Coordinate.X, Coordinate.X] = cosA + cosA1 * x * x,
            [Coordinate.X, Coordinate.Y] = cosA1 * x * y - sinA * z,
            [Coordinate.X, Coordinate.Z] = cosA * x * z + sinA * y,
            [Coordinate.Y, Coordinate.X] = cosA1 * y * x + sinA * z,
            [Coordinate.Y, Coordinate.Y] = cosA + cosA1 * y  * y,
            [Coordinate.Y, Coordinate.Z] = cosA1 * y * z - sinA * x,
            [Coordinate.Z, Coordinate.X] = cosA1 * z * x - sinA * y,
            [Coordinate.Z, Coordinate.Y] = cosA1 * z * y + sinA * x,
            [Coordinate.Z, Coordinate.Z] = cosA + cosA1 * z * z,
            [Coordinate.T, Coordinate.T] = 1,
        };
    }

    public static readonly IReadOnlyMatrix Single = new Matrix
    {
        [Coordinate.X, Coordinate.X] = 1,
        [Coordinate.Y, Coordinate.Y] = 1,
        [Coordinate.Z, Coordinate.Z] = 1,
        [Coordinate.T, Coordinate.T] = 1,
    };
}