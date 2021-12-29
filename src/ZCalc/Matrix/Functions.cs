namespace ZCalc.Matrix;

public static class Functions
{
    private const double Epsilon = 1E-10;
    
    private static readonly IReadOnlyList<Coordinate> Coords = new[]
    {
        Coordinate.X,
        Coordinate.Y,
        Coordinate.Z,
        Coordinate.T,
    };
    
    public static double LengthSquare(this Vector vector)
    {
        (double x, double y, double z) = vector;

        return x * x + y * y + z * z;
    }
    
    public static double Length(this Vector vector)
    {
        return Math.Sqrt(LengthSquare(vector));
    }
    
    public static Vector? Normalize(this Vector vector)
    {
        double length = vector.Length();

        if (length == 0)
        {
            return null;
        }

        (double x, double y, double z) = vector;
        
        return (x / length, y / length, z / length);
    }
    
    public static Vector Multiply(this IReadOnlyMatrix matrix, Vector vector)
    {
        (double x, double y, double z, double t) = vector;
        
        return (
            x: matrix[Coordinate.X, Coordinate.X] * x + matrix[Coordinate.X, Coordinate.Y] * y +
               matrix[Coordinate.X, Coordinate.Z] * z + matrix[Coordinate.X, Coordinate.T] * t,
            y: matrix[Coordinate.Y, Coordinate.X] * x + matrix[Coordinate.Y, Coordinate.Y] * y +
               matrix[Coordinate.Y, Coordinate.Z] * z + matrix[Coordinate.Y, Coordinate.T] * t,
            z: matrix[Coordinate.Z, Coordinate.X] * x + matrix[Coordinate.Z, Coordinate.Y] * y +
               matrix[Coordinate.Z, Coordinate.Z] * z + matrix[Coordinate.Z, Coordinate.T] * t,
            t: matrix[Coordinate.T, Coordinate.X] * x + matrix[Coordinate.T, Coordinate.Y] * y +
               matrix[Coordinate.T, Coordinate.Z] * z + matrix[Coordinate.T, Coordinate.T] * t
        );
    }
    
    public static Vector VectorMultiply(this Vector vector1, Vector vector2)
    {
        (double x1, double y1, double z1) = vector1;
        (double x2, double y2, double z2) = vector2;
        
        return (
            x: y1 * z2 - z1 * y2,
            y: z1 * x2 - x1 * z2,
            z: x1 * y2 - y1 * x2
        );
    }
    
    public static double ScalarMultiply(this Vector vector1, Vector vector2)
    {
        (double x1, double y1, double z1) = vector1;
        (double x2, double y2, double z2) = vector2;
        
        return x1 * x2 + y1 * y2 + z1 * z2;
    }
    
    public static double? CosAngle(this Vector vector1, Vector vector2)
    {
        double l = vector1.Length() * vector2.Length();

        if (l == 0)
        {
            return null;
        }

        return vector1.ScalarMultiply(vector2) / l;
    }
    
    public static bool AlmostEquals(this double d1, double d2)
    {
        return Math.Abs(d1 - d2) < Epsilon;
    }

    public static Matrix Multiply(this IReadOnlyMatrix matrix1, IReadOnlyMatrix matrix2)
    {
        var result = new Matrix();
        
        foreach (Coordinate coord1 in Coords)
        {
            foreach (Coordinate coord2 in Coords)
            {
                double value = 0;
                
                foreach (Coordinate iterator in Coords)
                {
                    value += matrix1[coord1, iterator] * matrix2[iterator, coord2];
                }

                result[coord1, coord2] = value;
            }
        }

        return result;
    }
}