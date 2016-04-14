using grid.render;
using UnityEngine;

namespace grid
{
    class GridRenderer : IGridRenderer
    {
        private IGrid grid;
        private Material material;
        private Mesh mesh;

        public GridRenderer(IGrid grid, Mesh cellMesh, Material material)
        {
            this.grid = grid;
            this.material = material;
            mesh = cellMesh;
        }

        public void DrawCell(int cellIdx)
        {
            if (material.SetPass(0)) {
                var pos = grid.CellCenter(cellIdx);
                Graphics.DrawMeshNow(mesh, Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
            }
        }

        public void Render()
        {
            //do nothing fow a while
        }
    }
}
