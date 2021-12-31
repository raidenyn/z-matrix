using ZCalc.Elements;
using ZCalc.Matrix;

namespace ZCalc.Bonds;

public class BondsCalculator
{
    private readonly ElementRadii _elementRadii = new ();
    
    public IEnumerable<Bond> GetBonds(CartesianMatrix cartesian)
    {
        for (var i = 1; i < cartesian.Rows.Count; i++)
        {
            CartesianRow atom1 = cartesian.Rows[i];
            for (var j = 0; j < i; j++)
            {
                CartesianRow atom2 = cartesian.Rows[j];

                if (GetBond(atom1, atom2) is { } bondType)
                {
                    yield return new Bond
                    {
                        Atom1 = i,
                        Atom2 = j,
                        Type = bondType,
                    };
                }
            }
        }
    }
    
    public BondType? GetBond(CartesianRow atom1, CartesianRow atom2)
    {
        if (atom1.Element <= 0 || atom2.Element <= 0)
        {
            return null;
        }
        
        Vector vector1 = atom1.Point;
        Vector vector2 = atom2.Point;

        double length = vector1.Minus(vector2).Length();
        double covalentDistance = _elementRadii.GetSingleBondRadius(atom1.Element) +
                                  _elementRadii.GetSingleBondRadius(atom2.Element);
        
        if (length <= covalentDistance * 1.5)
        {
            return BondType.Single;
        }

        return null;
    }
}