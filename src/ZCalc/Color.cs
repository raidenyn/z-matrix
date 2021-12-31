namespace ZCalc;

public readonly struct Color {
    public byte R { get; init; }

    public byte G { get; init; }
    
    public byte B { get; init; }
    
    public static implicit operator Color((byte r, byte g, byte b) color) =>
        new()
        {
            R = color.r, G = color.g, B = color.b
        };
}
