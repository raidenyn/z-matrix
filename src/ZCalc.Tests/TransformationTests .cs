using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ZCalc;

public class TransformationTests
{
    private Transformation CreateTransformation()
    {
        return new Transformation();
    }

    [Test]
    [TestCaseSource(nameof(GetTestData))]
    public void ZPointToCartesian((ZPoint zPoint, Point expectedResult) param)
    {
        Transformation transformation = CreateTransformation();

        Point result = transformation.ToCartesian(param.zPoint);

        Assert.AreEqual(param.expectedResult, result);
    }

    private static IEnumerable<(ZPoint, Point)> GetTestData()
    {
        yield return (
            new ZPoint
            {
                Distance = null,
                Angle = null,
                Dihedral = null,
            },
            new Point(0, 0, 0)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = 2,
                },
                Angle = null,
                Dihedral = null,
            },
            new Point(2, 0, 0)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = 2,
                },
                Angle = new ZValue
                {
                    Point = new Point(2,0,0),
                    Value = Math.PI / 2,
                },
                Dihedral = null,
            },
            new Point(0, -2, 0)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(2,0,0),
                    Value = 2,
                },
                Angle = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = Math.PI / 2,
                },
                Dihedral = null,
            },
            new Point(2, 2, 0)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(2,0,0),
                    Value = 2,
                },
                Angle = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = Math.PI / 3,
                },
                Dihedral = null,
            },
            new Point(1, 1.732050808, 0)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = 2,
                },
                Angle = new ZValue
                {
                    Point = new Point(2,0,0),
                    Value = Math.PI / 2,
                },
                Dihedral = new ZValue
                {
                    Point = new Point(0,0,-2),
                    Value = Math.PI / 2,
                },
            },
            new Point(0, -2, 0)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = 2,
                },
                Angle = new ZValue
                {
                    Point = new Point(2,0,0),
                    Value = Math.PI / 2,
                },
                Dihedral = new ZValue
                {
                    Point = new Point(0,0,-2),
                    Value = Math.PI,
                },
            },
            new Point(0, 0, 2)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = 2,
                },
                Angle = new ZValue
                {
                    Point = new Point(2,0,0),
                    Value = Math.PI / 2,
                },
                Dihedral = new ZValue
                {
                    Point = new Point(0,0,-2),
                    Value = 0,
                },
            },
            new Point(0, 0, -2)
        );
        
        yield return (
            new ZPoint
            {
                Distance = new ZValue
                {
                    Point = new Point(0,0,0),
                    Value = 2,
                },
                Angle = new ZValue
                {
                    Point = new Point(2,0,0),
                    Value = Math.PI / 2,
                },
                Dihedral = new ZValue
                {
                    Point = new Point(0,0,-2),
                    Value = Math.PI / 3,
                },
            },
            new Point(0, -1.732050808, -1)
        );
    }
    
    
    [Test]
    public void ZMatrixToCartesian()
    {
        var zMatrix = new ZCalc.ZMatrix
        {
            Rows =
            {
                6,
                (1, 1, 1.089),
                (1, 1, 1.089, 2, 109.4710),
                (1, 1, 1.089, 2, 109.4710, 3, 120.0),
                (1, 1, 1.089, 2, 109.4710, 3, -120.0),
            }
        };
        
        Transformation transformation = CreateTransformation();

        CartesianMatrix result = transformation.ToCartesian(zMatrix);

        CollectionAssert.AreEqual(new List<CartesianRow>
            {
                (6, 0.000000000, 0.000000000, 0.000000000),
                (1, 1.089000000, 0.000000000, 0.000000000),
                (1, -0.362996046, -1.026720444, 0.000000000),
                (1, -0.362996046, 0.513360222, 0.889165987),
                (1, -0.362996046, 0.513360222, -0.889165987),
            }
            , result.Rows);
    }
}