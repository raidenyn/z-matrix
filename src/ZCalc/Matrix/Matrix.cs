namespace ZCalc.Matrix;

public interface IReadOnlyMatrix
{
    public double this[Coordinate coord1, Coordinate coord2]
    {
        get;
    }
}

public record Matrix: IReadOnlyMatrix
{
    private readonly double[,] _values = new double[4,4];

    public double this[Coordinate coord1, Coordinate coord2]
    {
        get => _values[(int)coord1, (int)coord2];
        set => _values[(int)coord1, (int)coord2] = value;
    }
}