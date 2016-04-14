using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture]
    class Test_SquareField
    {
        private SquareGrid CreateField(float sizeX, float sizeZ, float cellSize)
        {
            return SquareGrid.CreateForArea(new Bounds(Vector3.zero, new Vector3(sizeX, 0, sizeZ)), cellSize);
        }

        [Test]
        public void CreateForArea_Size_1()
        {
            var field = CreateField(1, 1, 1);
            Assert.AreEqual(1, field.SizeX);
            Assert.AreEqual(1, field.SizeZ);
            Assert.AreEqual(1, field.CellCount);
        }

        [Test]
        public void CreateForArea_Size_2()
        {
            var field = CreateField(1, 1, 0.5f);
            Assert.AreEqual(2, field.SizeX);
            Assert.AreEqual(2, field.SizeZ);
            Assert.AreEqual(4, field.CellCount);
        }

        [Test]
        public void CreateForArea_Size_2_more()
        {
            var field = CreateField(1, 1, 0.4f);
            Assert.AreEqual(3, field.SizeX);
            Assert.AreEqual(3, field.SizeZ);
            Assert.AreEqual(9, field.CellCount);
        }

        [Test]
        public void MakeBoundsMultiplyOfSize()
        {
            {
                var rez = SquareGrid.MakeBoundsMultiplyOfSize(new Bounds(Vector3.zero, 2 * Vector3.one), 1.5f);
                Assert.AreEqual(3, rez.size.x);
                Assert.AreEqual(3, rez.size.z);
            }
            {
                var rez = SquareGrid.MakeBoundsMultiplyOfSize(new Bounds(Vector3.zero, new Vector3(4.5f, 0, 0.5f)), 1.5f);
                Assert.AreEqual(4.5f, rez.size.x);
                Assert.AreEqual(1.5f, rez.size.z);
            }
        }

        [Test]
        public void PosToCellId()
        {
            var field = CreateField(2, 2, 1);
            Assert.AreEqual(CellIdx.Wrong, field.PosToCellId(-2 * Vector3.one));
            Assert.AreEqual(0, field.PosToCellId(new Vector3(-0.5f, 0, -0.5f)));
            Assert.AreEqual(1, field.PosToCellId(new Vector3(0, 0, -1)));
            Assert.AreEqual(3, field.PosToCellId(new Vector3(0.1f, 0, 0.1f)));
            Assert.AreEqual(3, field.PosToCellId(new Vector3(1, 0, 1)));
            Assert.AreEqual(CellIdx.Wrong, field.PosToCellId(new Vector3(1.01f, 0, 1)));
        }

        [Test]
        public void CellCenter()
        {
            var field = CreateField(2, 2, 1);
            Assert.AreEqual(new Vector3(-0.5f, 0, -0.5f), field.CellCenter(0));
            Assert.AreEqual(new Vector3( 0.5f, 0, -0.5f), field.CellCenter(1));
            Assert.AreEqual(new Vector3(-0.5f, 0,  0.5f), field.CellCenter(2));
            Assert.AreEqual(new Vector3( 0.5f, 0,  0.5f), field.CellCenter(3));
            Assert.Throws<System.IndexOutOfRangeException>(() => field.CellCenter(4));
        }

        [Test]
        public void CellIdxInCircle()
        {
            var field = CreateField(5, 5, 1);
            Assert.AreEqual(new int[] { }, field.CellIdxInCircle(new Vector3(-3, 0, -3), 0.5f));
            Assert.AreEqual(new int[] { 12 }, field.CellIdxInCircle(new Vector3(0, 0, 0), 0));
            Assert.AreEqual(new int[] { 7, 11, 12, 13, 17 }, field.CellIdxInCircle(new Vector3(0, 0, 0), 1));
            Assert.AreEqual(new int[] { 6, 7, 8, 11, 12, 13, 16, 17, 18 }, field.CellIdxInCircle(new Vector3(0, 0, 0), Mathf.Sqrt(2) + 0.1f));
            Assert.AreEqual(new int[] { 2, 6, 7, 8, 10, 11, 12, 13, 14, 16, 17, 18, 22 }, field.CellIdxInCircle(new Vector3(0, 0, 0), 2));
            Assert.AreEqual(new int[] { 0, 1, 5 }, field.CellIdxInCircle(new Vector3(-2, 0, -2), 1));
            Assert.AreEqual(new int[] { 18, 19, 23, 24 }, field.CellIdxInCircle(new Vector3( 2, 0,  2), Mathf.Sqrt(2) + 0.1f));
        }
    }
}
