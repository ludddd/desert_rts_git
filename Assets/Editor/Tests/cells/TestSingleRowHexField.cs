using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture]
    class TestSingleRowHexField: BaseHexFieldTest
    {
        private const int CELL_COUNT = 5;

        [SetUp]
        public void SetUp()
        {
            SetUp(CELL_COUNT, 1);
        }

        [Test]
        public void CellCount()
        {
            Assert.AreEqual(CELL_COUNT, grid.CellCount);
        }

        [Test]
        public void CellCenter()
        {
            Assert.AreEqual(Vector3.zero, grid.CellCenter(0));
            int cellId = 3;
            Assert.AreEqual(new Vector3(hex.Width * cellId, 0, 0), grid.CellCenter(cellId));
        }

        [Test]
        public void PosToCellId()
        {
            Assert.AreEqual(3, grid.PosToCellId(grid.CellCenter(3)));
            Assert.AreEqual(0, grid.PosToCellId(new Vector3(-hex.HalfWidth, 0, 0)));
            Assert.AreEqual(1, grid.PosToCellId(new Vector3(hex.HalfWidth, 0, 0)));
            Assert.AreEqual(2, grid.PosToCellId(CellVertex(2, Hex.Vertex.Top)));
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(CellVertex(4, Hex.Vertex.BottomRight)));
        }


    }
}
