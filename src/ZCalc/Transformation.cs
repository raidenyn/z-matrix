using ZCalc.Matrix;

namespace ZCalc
{
    public class Transformation
    {
        public CartesianMatrix ToCartesian(ZMatrix zMatrix)
        {
            var result = new CartesianMatrix { Rows = new List<CartesianRow>(zMatrix.Rows.Count) };

            foreach (ZRow zMatrixRow in zMatrix.Rows)
            {
                var zPoint = new ZPoint
                {
                    Distance = zMatrixRow.Distance switch
                    {
                        { } distance => new ZValue
                        {
                            Point = result.Rows[distance.Reference-1].Point,
                            Value = distance.Value
                        },
                        _ => null
                    },
                    Angle = zMatrixRow.Angle switch
                    {
                        { } angle => new ZValue
                        {
                            Point = result.Rows[angle.Reference-1].Point,
                            Value = angle.Value.Radian
                        },
                        _ => null
                    },
                    Dihedral = zMatrixRow.Dihedral switch
                    {
                        { } dihedral => new ZValue
                        {
                            Point = result.Rows[dihedral.Reference-1].Point,
                            Value = dihedral.Value.Radian
                        },
                        _ => null
                    }
                };
                
                result.Rows.Add(new CartesianRow
                {
                    Element = zMatrixRow.Element,
                    Point = ToCartesian(zPoint)
                });
            }

            return result;
        }
        
        /// <summary>
        /// Return cartesian coordinate from a Z-Coordinate
        /// </summary>
        /// <param name="zPoint"></param>
        /// <returns></returns>
        public Point ToCartesian(ZPoint zPoint)
        {
            if (zPoint.Distance is not { } distance)
            {
                return new Point();
            }
            
            IReadOnlyMatrix reverseTranslate = Matrices.Translation(-(Vector)distance.Point);
            
            if (zPoint.Angle is not { } angle)
            {
                return reverseTranslate.Multiply((distance.Value, 0, 0));
            }
            
            IReadOnlyMatrix translate = Matrices.Translation(distance.Point);
            Vector a1 = translate.Multiply(angle.Point);
            IReadOnlyMatrix reverseRotateAngle = GetRotationMatrix(-Vector.OrtX, a1);
            IReadOnlyMatrix reverseRotateAngleValue = reverseRotateAngle.Multiply(Matrices.Rotate(Vector.OrtZ, -angle.Value));
            
            IReadOnlyMatrix reverseRotateDihedralValue = zPoint.Dihedral switch
            {
                { } dihedral => ((Func<IReadOnlyMatrix>)(() =>
                {
                    IReadOnlyMatrix rotateAngle = GetRotationMatrix(a1, -Vector.OrtX);
                    Vector d2 = rotateAngle.Multiply(translate.Multiply(dihedral.Point));

                    IReadOnlyMatrix reverseRotateDihedral = GetRotationMatrix(Vector.OrtY, (0, d2.Y, d2.Z));

                    return reverseRotateDihedral.Multiply(Matrices.Rotate(Vector.OrtX, -dihedral.Value));
                }))(),
                _ => Matrices.Single
            };
            

            return reverseTranslate.Multiply(reverseRotateDihedralValue.Multiply(reverseRotateAngleValue.Multiply((-distance.Value, 0, 0))));
        }

        /// <summary>
        /// Returns rotation matrix to rotate vector1 to vector2
        /// </summary>
        private IReadOnlyMatrix GetRotationMatrix(Vector vector1, Vector vector2)
        {
            double? cos = vector1.CosAngle(vector2);
            
            if (cos == null || cos.Value.AlmostEquals(1))
            {
                return Matrices.Single;
            }
            
            if (cos.Value.AlmostEquals(-1))
            {
                return Matrices.Rotate(Vector.OrtZ, Math.PI);
            }

            Vector axe = vector1.VectorMultiply(vector2).Normalize()!.Value;

            double angle = Math.Acos(cos.Value);

            return Matrices.Rotate(axe, -angle);
        }
    }
}