using ZCalc.Bonds;

namespace ZCalc.Blazor.Models;

public record Molecule
{
    public Atom[] Atoms { get; set; } = Array.Empty<Atom>();
    
    public Bond[] Bonds { get; set; }= Array.Empty<Bond>();
}

public record Atom {
    public string Element { get; set; } = String.Empty;
    
    public Point Point { get; set; } 
    
    public Color Color { get; set; }
}
