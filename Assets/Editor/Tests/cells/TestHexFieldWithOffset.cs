using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture(2, 1)]
    class TestHexFieldWithOffset
    {
        private readonly Vector3 offset;

        public TestHexFieldWithOffset(float x, float z)
        {
            offset = new Vector3(x, 0, z);
        }

        [Test]
        public void Create()
        {
            var field = new HexGrid(offset, 1, 1, 1);
            Assert.NotNull(field);
        }

        [Test]
        public void CellCenter([Values(0, 3, 10)]int cellIdx)
        {
            var field = new HexGrid(4, 5, 1);
            var fieldWithOffset = new HexGrid(offset, field.NColumn, field.NRow, field.CellSize);
            Assert.AreEqual(offset + field.CellCenter(cellIdx), fieldWithOffset.CellCenter(cellIdx));
        }        
    }
}
