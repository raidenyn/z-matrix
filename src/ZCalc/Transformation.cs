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
            
            if (zPoint.Dihedral is not { } dihedral)
            {
                IReadOnlyMatrix rotate = Matrices.Rotate(Vector.OrtZ, -angle.Value);
                return reverseTranslate.Multiply(reverseRotateAngle.Multiply(rotate.Multiply((-distance.Value, 0, 0))));
            }
            
            IReadOnlyMatrix rotateAngle = GetRotationMatrix(a1, -Vector.OrtX);
            Vector d2 = translate.Multiply(dihedral.Point);
            Vector d3 = rotateAngle.Multiply(d2);

            IReadOnlyMatrix reverseRotateDihedral = GetRotationMatrix(Vector.OrtY, (0, d3.Y, d3.Z));

            IReadOnlyMatrix rotate1 = Matrices.Rotate(Vector.OrtZ, -angle.Value);
            IReadOnlyMatrix rotate2 = Matrices.Rotate(Vector.OrtX, -dihedral.Value);
            var v1 = rotate1.Multiply((-distance.Value, 0, 0));
            var v2 = rotate2.Multiply(v1);
            var v3 = reverseRotateDihedral.Multiply(v2);
            var v4 = reverseRotateAngle.Multiply(v3);
            var v5 = reverseTranslate.Multiply(v4);


            return v5;
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