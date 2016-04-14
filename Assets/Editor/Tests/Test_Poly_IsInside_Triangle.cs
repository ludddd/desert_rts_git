using NUnit.Framework;
using UnityEngine;

namespace math.geom2d
{
    [TestFixture]
    class Test_Poly_IsInside_Triangle
    {
        private Poly poly;

        [SetUp]
        public void Init()
        {
            poly = new Poly(new Vector2[] {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1)
                    }
                );
        }

        [Test]
        public void CheckOutside()
        {
            Assert.IsFalse(poly.IsInside(new Vector2(-1, 0)));
            Assert.IsFalse(poly.IsInside(new Vector2(2, 0)));
            Assert.IsFalse(poly.IsInside(new Vector2(0, -1)));
            Assert.IsFalse(poly.IsInside(new Vector2(0, 2)));
            Assert.IsFalse(poly.IsInside(new Vector2(1, 1)));
        }

        [Test]
        public void CheckCenter()
        {
            Assert.IsTrue(poly.IsInside(new Vector2(0.3f, 0.3f)));
        }
    }
}
