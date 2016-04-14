using System.Collections.Generic;
using grid.mesh;
using UnityEngine;

namespace grid.render
{
    class MeshGridRenderer : IGridRenderer
    {
        private readonly Material material;
        
        private readonly IBoolGridMesh mesh;
        private readonly IGrid grid;
        private readonly ICollection<int> visibleCells = new HashSet<int>();

        private MeshGridRenderer(IGrid grid, IBoolGridMesh mesh, Material material)
        {
            this.material = material;
            this.grid = grid;
            this.mesh = mesh;
        }

        public static MeshGridRenderer CreatePoint9FowMesh(IGrid grid, float size, Material material)
        {
            if (!(grid is SquareGrid))
            {
                return null;
            }
            var squareGrid = (SquareGrid) grid;
            return new MeshGridRenderer(grid, new Point9FowMesh(squareGrid, size), material);
        }

        //TODO: remove duplication with CreatePoint9FowMesh
        public static MeshGridRenderer CreateHexFowMesh(IGrid grid, Material material)
        {
            if (!(grid is HexGrid))
            {
                return null;
            }
            var hexGrid = (HexGrid) grid;
            return new MeshGridRenderer(grid, new HexFowMesh(hexGrid), material);
        }

        public static MeshGridRenderer CreateSingleMeshGrid(IGrid grid, Mesh cellMesh, Material material)
        {
            var mesh = new SingleMeshGridMesh(grid, cellMesh);
            return new MeshGridRenderer(grid, mesh, material);
        }

        public void DrawCell(int cellIdx)
        {
            Debug.Assert(grid.IsIndexValid(cellIdx));
            visibleCells.Add(cellIdx);
        }

        public void Render()
        {
            if (material.SetPass(0)) {
                mesh.Setup(visibleCells.Contains);
                Graphics.DrawMeshNow(mesh.Mesh, Matrix4x4.identity);
            }
            visibleCells.Clear();
        }
    }
}
