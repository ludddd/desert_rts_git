using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture]
    class TestSingleColumnHexField: BaseHexFieldTest
    {
        private const int CELL_COUNT = 3;

        [SetUp]
        public void SetUp()
        {
            SetUp(1, CELL_COUNT);
        }

        [Test]
        public void Create()
        {
            Assert.AreEqual(CELL_COUNT, grid.CellCount);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 0.75f, 1)]
        [TestCase(0, 1.5f, 2)]
        public void CellCenter(float x, float z, int cellIdx)
        {
            Assert.AreEqual(new Vector3(x * hex.HalfWidth, 0, z * hex.Height), grid.CellCenter(cellIdx));
        }

        [TestCase(0, Hex.Vertex.TopRight, 1)]
        [TestCase(0, Hex.Vertex.Top, 1)]
        [TestCase(1, Hex.Vertex.TopLeft, 2)]
        [TestCase(1, Hex.Vertex.Top, CellIdx.Wrong)]
        public void PosToCellIdForVertex(int cellIdx, Hex.Vertex vertex, int expectedCellIdx)
        {
            Assert.AreEqual(expectedCellIdx, grid.PosToCellId(CellVertex(cellIdx, vertex)));
        }

        public void PosToCellId()
        {
            {
                Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(hex.HalfWidth, 0, 0)));
            }
            {
                int cellIdx = 1;

                var edgeCenter = 0.5f*(hex.GetVertex(Hex.Vertex.Top) + hex.GetVertex(Hex.Vertex.TopLeft));
                Assert.AreEqual(cellIdx + 1, grid.PosToCellId(PosInCell(cellIdx, edgeCenter)));
            }
            {
                int cellIdx = 0;
                var justBelowTop = 0.99f *hex.GetVertex(Hex.Vertex.Top);
                Assert.AreEqual(cellIdx, grid.PosToCellId(PosInCell(cellIdx, justBelowTop)));
            }
            {
                int cellIdx = 1;
                var justAboveBottom = 0.99f *hex.GetVertex(Hex.Vertex.Bottom);
                Assert.AreEqual(cellIdx, grid.PosToCellId(PosInCell(cellIdx, justAboveBottom)));

                var edgeCenter = 0.5f * (hex.GetVertex(Hex.Vertex.Bottom) + hex.GetVertex(Hex.Vertex.BottomLeft));
                Assert.AreEqual(cellIdx, grid.PosToCellId(grid.CellCenter(cellIdx) + 0.99f * edgeCenter));
            }
        }
    }
}
