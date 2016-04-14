using NUnit.Framework;
using UnityEngine;

namespace math.geom2d
{
    [TestFixture]
    class Test_Poly_IsInside_NonConvex
    {
        Poly poly;

        [SetUp]
        public void Init()
        {
            poly = new Poly(new Vector2[] {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(2, 0),
                    new Vector2(2, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 2),
                    new Vector2(0, 2),
                    new Vector2(0, 1),
                    }
                );
        }

        [Test]
        public void CheckInside()
        {
            Assert.IsTrue(poly.IsInside(new Vector2(0.5f, 0.5f)));
            Assert.IsTrue(poly.IsInside(new Vector2(1.9f, 0.5f)));
            Assert.IsTrue(poly.IsInside(new Vector2(0.5f, 1.9f)));
        }

        [Test]
        public void CheckOutside()
        {
            Assert.IsFalse(poly.IsInside(new Vector2(-0.5f, -0.5f)));
            Assert.IsFalse(poly.IsInside(new Vector2(1.0f, -0.5f)));
            Assert.IsFalse(poly.IsInside(new Vector2(0.5f, 2.5f)));
            Assert.IsFalse(poly.IsInside(new Vector2(1.5f, 1.5f)));
            Assert.IsFalse(poly.IsInside(new Vector2(0.5f, 2.5f)));
            Assert.IsFalse(poly.IsInside(new Vector2(-0.5f, 1.0f)));
        }
    }
}
