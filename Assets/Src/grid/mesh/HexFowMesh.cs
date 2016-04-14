using System;
using UnityEngine;

namespace grid.mesh
{
    class HexFowMesh:IBoolGridMesh
    {
        private const int VERTEX_PER_CELL = 7;  //same as number of elements in Hex.Vertex - cannot get in static way
        private readonly Mesh mesh;
        private readonly HexGrid grid;

        public HexFowMesh(HexGrid grid)
        {
            Debug.Assert(VERTEX_PER_CELL == Enum.GetValues(typeof(Hex.Vertex)).Length);
            this.grid = grid;
            mesh = GridMeshGenerator.Build(grid, HexMeshGenerator.Build(grid.CellSize));
        }

        public Mesh Mesh {
            get { return mesh; }
        }

        public void Setup(Func<int, bool> cellFunc)
        {
            var uv = mesh.uv;
            for (int cellIdx = 0; cellIdx < grid.CellCount; cellIdx++)
            {
                SetupCellUV(uv, cellIdx, cellFunc(cellIdx));
            }
            for (int cellIdx = 0; cellIdx < grid.CellCount; cellIdx++)
            {
                if (cellFunc(cellIdx))
                {
                    SetupVisibleCellNeighbors(uv, cellIdx);
                }
            }
            mesh.uv = uv;
        }

        private void SetupCellUV(Vector2[] uv, int cellIdx, bool isVisible)
        {
            var value = GetValue(isVisible);
            int startUVIdx = VERTEX_PER_CELL * cellIdx;
            for (int i = 0; i < VERTEX_PER_CELL; i++) {
                uv[startUVIdx + i] = value;
            }
        }

        private static Vector2 GetValue(bool isVisible)
        {
            return isVisible ? Vector2.one : Vector2.zero;
        }

        private void SetupVisibleCellNeighbors(Vector2[] uv, int cellIdx)
        {
            var value = GetValue(true);
            foreach (HexGrid.Dir dir in Enum.GetValues(typeof(HexGrid.Dir)))
            {
                var neighborIdx = grid.Go(cellIdx, dir);
                if (neighborIdx == CellIdx.Wrong) continue;
                var vertices = HexGrid.VertexInDir(dir);
                foreach (var vertIdx in vertices)
                {
                    uv[neighborIdx * VERTEX_PER_CELL + (int)Hex.Opposite(vertIdx)] = value;
                }
            }
        }
    }
}
