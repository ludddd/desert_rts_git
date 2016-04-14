using NUnit.Framework;

namespace grid
{
    [TestFixture]
    class TestHexFieldGo : BaseHexFieldTest
    {
        private const int SIZE_X = 4;
        private const int SIZE_Z = 5;
        private const int CELL_SIZE = 1;

        [SetUp]
        public void Setup()
        {
            SetUp(SIZE_X, SIZE_Z, CELL_SIZE);
        }

        [Test]
        public void LeftRight([Values(1)] int x, [Values(1, 2)] int z)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            Assert.AreEqual(grid.GetCellIdx(x + 1, z), grid.Go(cellIdx, HexGrid.Dir.Right));
            Assert.AreEqual(grid.GetCellIdx(x - 1, z), grid.Go(cellIdx, HexGrid.Dir.Left));
        }

        [Test]
        public void UpDownForEvenRow([Values(1)] int x, [Values(2)] int z)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            Assert.True(z%2==0);
            Assert.AreEqual(grid.GetCellIdx(x - 1, z + 1), grid.Go(cellIdx, HexGrid.Dir.TopLeft));
            Assert.AreEqual(grid.GetCellIdx(x - 1, z - 1), grid.Go(cellIdx, HexGrid.Dir.BottomLeft));
        }

        [Test]
        public void UpDownForOddRow([Values(1)] int x, [Values(1)] int z)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            Assert.True(z % 2 == 1);
            Assert.AreEqual(grid.GetCellIdx(x, z + 1), grid.Go(cellIdx, HexGrid.Dir.TopLeft));
            Assert.AreEqual(grid.GetCellIdx(x, z - 1), grid.Go(cellIdx, HexGrid.Dir.BottomLeft));
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void TopRightIsSameAsTopLeftAndRight(int x, int z)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            Assert.AreEqual(grid.Go(grid.Go(cellIdx, HexGrid.Dir.TopLeft), HexGrid.Dir.Right), grid.Go(cellIdx, HexGrid.Dir.TopRight));
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void BottomRightIsSameAsBottomLeftAndRight(int x, int z)
        {
            int cellIdx = grid.GetCellIdx(x, z);
            Assert.AreEqual(grid.Go(grid.Go(cellIdx, HexGrid.Dir.BottomLeft), HexGrid.Dir.Right), grid.Go(cellIdx, HexGrid.Dir.BottomRight));
        }
    }

    public struct DirTestData
    {
        public int dx;
        public int dz;
        public HexGrid.Dir dir;

        public override string ToString()
        {
            return string.Format("Dx: {0}, Dz: {1}, Dir: {2}", dx, dz, dir);
        }
    }
}