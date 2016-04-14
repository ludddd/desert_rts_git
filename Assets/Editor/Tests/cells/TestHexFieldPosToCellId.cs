using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture(0, 0)]
    [TestFixture(3, -2)]
    class TestHexFieldPosToCellId: BaseHexFieldTest
    {
        private const int SIZE_X = 4;
        private const int SIZE_Z = 5;

        public TestHexFieldPosToCellId(float x, float z):base(x, z)
        {
        }

        [SetUp]
        public void SetUp()
        {
            SetUp(SIZE_X, SIZE_Z);
        }

        [Test]
        public void IdempontentWithCellCenter([NUnit.Framework.Range(0, SIZE_X *SIZE_Z - 1)] int cellIdx)
        {
            Assert.AreEqual(cellIdx, grid.PosToCellId(grid.CellCenter(cellIdx)), string.Format("for cellIdx {0}", cellIdx));
        }

        [TestCase(1, 0, HexGrid.Dir.Right)]
        [TestCase(0, 3, HexGrid.Dir.Left)]
        [TestCase(1, 2, HexGrid.Dir.TopRight)]
        [TestCase(1, SIZE_Z-1, HexGrid.Dir.TopLeft)]
        public void InnerPoints(int x, int z, HexGrid.Dir dir)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            var point = GetInnerPoint(dir);
            Assert.AreEqual(cellIdx, grid.PosToCellId(PosInCell(cellIdx, point)));
        }

        

        [TestCase(1, 0, HexGrid.Dir.TopRight)]
        [TestCase(2, 2, HexGrid.Dir.Right)]
        [TestCase(3, 2, HexGrid.Dir.BottomLeft)]
        public void OuterPoints(int x, int z, HexGrid.Dir dir)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            var point = GetOuterPoint(dir);
            Assert.AreEqual(grid.Go(cellIdx, dir), grid.PosToCellId(PosInCell(cellIdx, point)));
        }

        [TestCase(1, 0, HexGrid.Dir.BottomRight)]
        [TestCase(0, 2, HexGrid.Dir.Left)]
        [TestCase(2, SIZE_Z-1, HexGrid.Dir.TopLeft)]
        [TestCase(SIZE_X-1, SIZE_Z-1, HexGrid.Dir.Right)]
        public void OuterPointsOnBorder(int x, int z, HexGrid.Dir dir)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            var point = GetOuterPoint(dir);
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(PosInCell(cellIdx, point)));
        }

        [TestCase(2)]
        [TestCase(1000)]
        [TestCase(1000000)]
        public void PrecisionErrorBetweenCells(int sizeX)
        {
            //point belong to some cell, even if precision error causes it belong to "wrong" one...
            var field1 = new HexGrid(sizeX, 1, 1);
            var pos = new Vector3(hex.Width * (sizeX - 1) - hex.HalfWidth, 0, 0);
            Assert.That(field1.PosToCellId(pos), Is.EqualTo(sizeX - 2).Or.EqualTo(sizeX - 1));
        }
    }
}