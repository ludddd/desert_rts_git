using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture(0, 0)]
    [TestFixture(2, 1)]
    [TestFixture(-10.3f, -8.555f)]
    class TestHexField : BaseHexFieldTest
    {
        private const int SIZE_X = 4;
        private const int SIZE_Z = 5;

        public TestHexField(float x, float z):base(x, z)
        {
        }

        [SetUp]
        public void SetUp()
        {
            SetUp(SIZE_X, SIZE_Z);
        }


        [Test]
        public void CreationAndSize()
        {
            Assert.AreEqual(SIZE_X, grid.NColumn);
            Assert.AreEqual(SIZE_Z, grid.NRow);
            Assert.AreEqual(SIZE_X * SIZE_Z, grid.CellCount);
        }

        class Vector3Equality: IEqualityComparer<Vector3>
        {
            private readonly float precision;

            public Vector3Equality(float precision)
            {
                if (precision < 0)
                {
                    throw new ArgumentOutOfRangeException("precision", precision, "should be greater or equal to 0");
                }
                this.precision = precision;
            }

            public bool Equals(Vector3 x, Vector3 y)
            {
                return Vector3.Distance(x, y) < precision;
            }

            public int GetHashCode(Vector3 obj)
            {
                return obj.GetHashCode();
            }
        }

        [Test]
        public void CellCenter([NUnit.Framework.Range(0, 3)] int z, [NUnit.Framework.Range(0, 2)] int x)
        {
            var expected = offset + new Vector3(hex.Width * x, 0, 0.75f * hex.Height * z);
            if (z % 2 == 1) expected += new Vector3(hex.HalfWidth, 0, 0);
            Assert.That(grid.CellCenter(grid.GetCellIdx(x, z)), Is.EqualTo(expected).Using(new Vector3Equality(0.001f)));
        }

        [Test]
        public void CellCenterWithWrongCellIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.CellCenter(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.CellCenter(grid.CellCount));
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.CellCenter(grid.CellCount + 1));
        }

        [TestCase(0, 0, 1, new[] { 0 })]
        [TestCase(1, 0, 0.5f, new[] { 1 })]
        [TestCase(0, 0, 2, new[] { 0, 1, SIZE_X })]
        public void CellsInCircle(int x, int z, float radius, int[] cells)
        {
            var pos = grid.CellCenter(grid.GetCellIdx(x, z));
            Assert.That(grid.CellIdxInCircle(pos, radius * hex.HalfHeight), Is.EquivalentTo(cells));
        }

        [TestCase(2, 2, 1)]
        [TestCase(2, 2, 2)]
        [TestCase(2, 2, 3)]
        [TestCase(3, 2, 4)]
        [TestCase(3, 2, 100)]
        public void CellsInCircleAuto(int x, int z, float radius)
        {
            Assert.GreaterOrEqual(radius, hex.HalfHeight);  //this test is correct only for big radius
            var pos = grid.CellCenter(grid.GetCellIdx(x, z));
            var expected = from i in Enumerable.Range(0, grid.CellCount)
                where Vector3.Distance(pos, grid.CellCenter(i)) <= radius
                select i;
            Assert.AreEqual(expected.ToArray(), grid.CellIdxInCircle(pos, radius).ToArray());
        }

        [TestCase(0.2f, 0, 0.1f, new[] { 0 })]
        [TestCase(-1000, 0, 0.1f, new int[] {  })]
        public void VerySmallRadius(float x, float z, float radius, int[] cells)
        {
            var pos = grid.CellCenter(0) + new Vector3(x, 0, z);
            Assert.AreEqual(cells, grid.CellIdxInCircle(pos, radius));
        }
    }  
}
