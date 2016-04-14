using System;
using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture]
    class TestHexFieldSingleCell: BaseHexFieldTest
    {
        private const float SIZE = 2.0f;

        [SetUp]
        public void SetUp()
        {
            SetUp(1, 1, SIZE);
        }

        [Test]
        public void CreateField()
        {
            Assert.IsInstanceOf<IGrid>(grid);
        }

        [Test]
        public void CellCenter()
        {
            Assert.AreEqual(Vector3.zero, grid.CellCenter(0));
        }

        [Test]
        public void PosToCellId()
        {
            float hH = hex.HalfHeight;
            float hW = hex.HalfWidth;
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(hH, 0, 0)));
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(-hH, 0, 0)));
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(0, 0, hH + 0.1f)));

            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(hW, 0, hH)));
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(-hW, 0, -hH)));
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(-hW, 0, hH)));
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(new Vector3(-hW, 0, -hH)));
        }

        [Test]
        public void PosToCellIdFarAway()
        {
            var farAway = 100 * SIZE * Vector3.one;
            Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(farAway));
        }

        private string GetMsgForVertex(Hex.Vertex v)
        {
            return string.Format("with vertex {0}", Enum.GetName(typeof (Hex.Vertex), v));
        }

        [Test]
        public void PosToCellIdWithHexVertices()
        {
            var includedVertices = new[] { Hex.Vertex.Center, Hex.Vertex.Top, Hex.Vertex.Bottom, Hex.Vertex.TopLeft, Hex.Vertex.BottomLeft };
            foreach (var v in includedVertices) {
                Assert.AreEqual(0, grid.PosToCellId(hex.GetVertex(v)), GetMsgForVertex(v));
            }

            var excludedVertices = new[] { Hex.Vertex.TopRight, Hex.Vertex.BottomRight };
            foreach (var v in excludedVertices) {
                Assert.AreEqual(CellIdx.Wrong, grid.PosToCellId(hex.GetVertex(v)), GetMsgForVertex(v));
                var justInsideVertex = 0.99f * hex.GetVertex(v);
                Assert.AreEqual(0, grid.PosToCellId(justInsideVertex), GetMsgForVertex(v));
            }
        }

        [Test]
        public void NowhereToGo()
        {
            foreach (HexGrid.Dir dir in Enum.GetValues(typeof(HexGrid.Dir)))
            {
                Assert.AreEqual(CellIdx.Wrong, grid.Go(0, dir));
            }            
        }

        [TestCase(0, 0, 1)]
        [TestCase(0.1f, 0.1f, 1)]
        [TestCase(0, 1, 1)]
        public void HasCellInCircle(float x, float z, float radius)
        {
            var pos = SIZE * new Vector3(x, 0, z);
            Assert.AreEqual(new[] {0}, grid.CellIdxInCircle(pos, radius * SIZE));
        }

        [TestCase(10, 10, 1)]
        [TestCase(1.1f, 0, 1)]
        public void NoCellInCircle(float x, float z, float radius)
        {
            var pos = SIZE * new Vector3(x, 0, z);
            Assert.That(grid.CellIdxInCircle(pos, radius * SIZE), Is.Empty);
        }

        [TestCase(HexGrid.Dir.TopRight, Hex.Vertex.Top, Hex.Vertex.TopRight)]
        [TestCase(HexGrid.Dir.Right, Hex.Vertex.TopRight, Hex.Vertex.BottomRight)]
        [TestCase(HexGrid.Dir.BottomRight, Hex.Vertex.BottomRight, Hex.Vertex.Bottom)]
        [TestCase(HexGrid.Dir.BottomLeft, Hex.Vertex.Bottom, Hex.Vertex.BottomLeft)]
        [TestCase(HexGrid.Dir.Left, Hex.Vertex.BottomLeft, Hex.Vertex.TopLeft)]
        [TestCase(HexGrid.Dir.TopLeft, Hex.Vertex.TopLeft, Hex.Vertex.Top)]
        public void VertexInDir(HexGrid.Dir dir, Hex.Vertex v1, Hex.Vertex v2)
        {
            Assert.AreEqual(new [] {v1, v2}, HexGrid.VertexInDir(dir));
        }
    }
}