using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace grid
{
    [TestFixture(0, 0, 3, 3, 1)]
    [TestFixture(0, 0, 100, 100, 1)]
    [TestFixture(0.3f, -1.8f, 5.6f, 10.1f, 1)]
    class TestHexFieldCreateForArea
    {
        private readonly Bounds bounds;
        private readonly HexGrid grid;

        public TestHexFieldCreateForArea(float xCenter, float zCenter, float sizeX, float sizeZ, float cellSize)
        {
            bounds = new Bounds(new Vector3(xCenter, 0, zCenter), new Vector3(sizeX, 0, sizeZ));
            grid = HexGrid.CreateForArea(bounds, cellSize);
        }

        [Test]
        public void CornerBelongsToField()
        {
            Assert.That(grid.PosToCellId(bounds.min), BelongsToField);
            Assert.That(grid.PosToCellId(bounds.max), BelongsToField);
            Assert.That(grid.PosToCellId(new Vector3(bounds.min.x, 0, bounds.max.z)), BelongsToField);
            Assert.That(grid.PosToCellId(new Vector3(bounds.max.x, 0, bounds.min.z)), BelongsToField);
        }

        private static EqualConstraint BelongsToField
        {
            get { return Is.Not.EqualTo(CellIdx.Wrong); }
        }

        [Test]
        public void BorderBelogsToField()
        {
            foreach (float z in new[] { bounds.min.z, bounds.max.z }) {
                for (float x = bounds.min.x; x < bounds.max.x; x += 0.5f*grid.CellSize) {             
                    var pos = new Vector3(x, 0, z);
                    Assert.That(grid.PosToCellId(pos), BelongsToField);
                }
            }
            foreach (float x in new[] {bounds.min.x, bounds.max.x}) {
                for (float z = bounds.min.z; z < bounds.max.z; z += 0.5f*grid.CellSize) {
                    var pos = new Vector3(x, 0, z);
                    Assert.That(grid.PosToCellId(pos), BelongsToField);
                }
            }          
        }

        [Test]
        public void NoExcessColumn()
        {
            int firstInOddRow = grid.GetCellIdx(0, 0);
            int lastInEvenRow = grid.GetCellIdx(grid.NColumn - 1, 1);
            Assert.That(grid.CellCenter(lastInEvenRow).x - grid.CellCenter(firstInOddRow).x,
                Is.LessThanOrEqualTo(bounds.size.x + grid.CellWidth));
        }

        [Test]
        public void NoExcessRow()
        {
            int firstRow = grid.GetCellIdx(0, 0);
            int lastRow = grid.GetCellIdx(0, grid.NRow - 1);
            Assert.That(grid.CellCenter(lastRow).z - grid.CellCenter(firstRow).z,
                Is.LessThanOrEqualTo(bounds.size.z + 1.5f * grid.CellSize));
        }

        [Test]
        public void HorizontalSymmetry()
        {
            int firstInOddRow = grid.GetCellIdx(0, 0);
            int lastInEvenRow = grid.GetCellIdx(grid.NColumn - 1, 1);
            float left = bounds.min.x - grid.CellCenter(firstInOddRow).x;
            float right = grid.CellCenter(lastInEvenRow).x - bounds.max.x;
            Assert.That(left, Is.EqualTo(right).Within(0.01f * grid.CellSize));
        }

        [Test]
        public void VerticalSymmetry()
        {
            int firstRow = grid.GetCellIdx(0, 0);
            int lastRow = grid.GetCellIdx(0, grid.NRow - 1);
            float bottom = grid.CellCenter(firstRow).z - bounds.min.z;
            float top = bounds.max.z - grid.CellCenter(lastRow).z;
            Assert.That(bottom, Is.EqualTo(top).Within(0.01f * grid.CellSize));
        }
    }
}
