using System;
using System.Collections.Generic;
using UnityEngine;

namespace grid.mesh
{
    using V = MeshPoint9Generator.VERTEX;

    class Point9FowMesh : IBoolGridMesh
    {
        private readonly Mesh mesh;
        private readonly SquareGrid grid;
        private readonly Traveler traveler;
        private static readonly IDictionary<Traveler.Dir, V[]> AdjustVertexMap = new Dictionary<Traveler.Dir, V[]> {
            {Traveler.Dir.NW, new [] { V.SE } },
            {Traveler.Dir.N, new [] { V.SE, V.S, V.SW } },
            {Traveler.Dir.NE, new [] { V.SW } },
            {Traveler.Dir.W, new [] { V.SE, V.E, V.NE } },
            {Traveler.Dir.E, new [] { V.SW, V.W, V.NW } },
            {Traveler.Dir.SW, new [] { V.NE } },
            {Traveler.Dir.S, new [] { V.NW, V.N, V.NE } },
            {Traveler.Dir.SE, new [] { V.NW } }
        };

        public Mesh Mesh
        {
            get
            {
                return mesh;
            }
        }

        public Point9FowMesh(SquareGrid grid, float cellSize)
        {
            this.grid = grid;
            mesh = GridMeshGenerator.Build(grid, MeshPoint9Generator.Build(cellSize));
            traveler = new Traveler(grid.SizeX, grid.SizeZ);
        }

        public void Setup(Func<int, bool> cellFunc)
        {
            var uv = mesh.uv;
            for (int cellIdx = 0; cellIdx < grid.CellCount; cellIdx++) {
                SetupCellUV(uv, cellIdx, cellFunc(cellIdx));
            }
            for (int cellIdx = 0; cellIdx < grid.CellCount; cellIdx++) {
                if (cellFunc(cellIdx)) {
                    SetupVisibleCellNeighbors(uv, cellIdx);
                }
            }
            mesh.uv = uv;
        }

        private void SetupCellUV(Vector2[] uv, int cellIdx, bool isVisible)
        {
            int startUVIdx = cellIdx * MeshPoint9Generator.VertexCount;
            for (int i = startUVIdx; i < startUVIdx + MeshPoint9Generator.VertexCount; i++) {
                uv[i] = GetUV(isVisible);
            }
        }

        private static Vector2 GetUV(bool isVisible)
        {
            return isVisible ? Vector2.one : Vector2.zero;
        }

        private void SetupVisibleCellNeighbors(Vector2[] uv, int cellIdx)
        {
            foreach (Traveler.Dir dir in Enum.GetValues(typeof(Traveler.Dir))) {
                int neighborIdx = traveler.Go(cellIdx, dir);
                if (neighborIdx == CellIdx.Wrong) {
                    continue;
                }
                var startUVIdx = neighborIdx * MeshPoint9Generator.VertexCount;
                foreach (V idx in AdjustVertexMap[dir]) {
                    SetNeighborVertexUV(uv, startUVIdx, idx);
                }
            }
        }

        //TODO: ugly function name
        private static void SetNeighborVertexUV(Vector2[] uv, int cellStartUVIdx, V vertexIdx)
        {
            var value = (MeshPoint9Generator.IsCorner(vertexIdx) ? 0.8f : 1) * GetUV(true);
            int uvIdx = cellStartUVIdx + (int)vertexIdx;
            if (HasNoValue(uv, uvIdx))
            {
                uv[uvIdx] = value;
            }
            else
            {
                uv[uvIdx] += 0.5f*value;
            }
        }

        private static bool HasNoValue(Vector2[] uv, int uvIdx)
        {
            return Mathf.Approximately(uv[uvIdx].x, 0); //TODO: hidden duplication with GetUV(false) and Vector.zero...
        }
    }
}
