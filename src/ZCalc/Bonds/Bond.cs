namespace ZCalc.Bonds;

public record Bond
{
    public int Atom1 { get; set; }
    
    public int Atom2 { get; set; }
    
    public BondType Type { get; set; }
}