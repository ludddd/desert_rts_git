using System;
using System.Collections.Generic;
using UnityEngine;

namespace grid.mesh
{
    //TODO: find a better name
    class SingleMeshGridMesh : IBoolGridMesh
    {
        private readonly Mesh mesh;
        private readonly int[] cellIndices;
        private readonly int verticiesPerCell;
        private readonly IGrid grid;

        public SingleMeshGridMesh(IGrid grid, Mesh cellMesh)
        {
            this.grid = grid;
            cellIndices = (int[])cellMesh.triangles.Clone();
            verticiesPerCell = cellMesh.vertexCount;
            mesh = GridMeshGenerator.Build(grid, cellMesh);
        }

        public Mesh Mesh {
            get { return mesh; }
        }

        public void Setup(Func<int, bool> cellFunc)
        {
            var indices = new List<int>(mesh.triangles.Length);
            for (int cellIdx = 0; cellIdx < grid.CellCount; cellIdx++)
            {
                if (!cellFunc(cellIdx)) continue;
                foreach (int k in cellIndices) {
                    indices.Add(verticiesPerCell * cellIdx + k);
                }
            }
            mesh.SetTriangles(indices, 0);
        }
    }
}