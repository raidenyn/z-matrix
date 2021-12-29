namespace ZCalc
{
    public readonly struct ZPoint
    {
        public ZValue? Distance { get; init; }
        
        public ZValue? Angle { get; init; }
        
        public ZValue? Dihedral { get; init; }
        
        public override string ToString()
        {
            var parts = new List<string>(3);

            if (Distance != null)
            {
                parts.Add($"R: {Distance}");
            }
            if (Angle != null)
            {
                parts.Add($"A: {Angle}");
            }
            if (Dihedral != null)
            {
                parts.Add($"D: {Dihedral}");
            }
            
            return String.Join(", ", parts);
        }
    }
    
    public readonly struct ZValue
    {
        public double Value { get; init; }
        
        public Point Point { get; init; }

        public override string ToString()
        {
            return $"{Value} - {Point}";
        }
    }
}