using UnityEngine;

namespace grid
{
    class BaseHexFieldTest
    {
        protected readonly Vector3 offset;
        protected HexGrid grid;
        protected Hex hex;

        public BaseHexFieldTest()
        {
            offset = Vector3.zero;
        }

        public BaseHexFieldTest(float x, float z)
        {
            offset = new Vector3(x, 0, z);
        }

        protected void SetUp(int sizeX, int sizeZ, float cellSize = 1)
        {
            hex = new Hex(cellSize);
            grid = new HexGrid(offset, sizeX, sizeZ, cellSize);
        }

        protected Vector3 PosInCell(int cellIdx, Vector3 offset)
        {
            return grid.CellCenter(cellIdx) + offset;
        }

        protected Vector3 CellVertex(int cellIdx, Hex.Vertex v, float mult = 1.0f)
        {
            return PosInCell(cellIdx, mult * hex.GetVertex(v));
        }

        protected Vector3 GetInnerPoint(HexGrid.Dir dir)
        {
            return TestHex.GetInnerPoint(hex, dir);
        }

        protected Vector3 GetOuterPoint(HexGrid.Dir dir)
        {
            return TestHex.GetOuterPoint(hex, dir);
        }
    }
}