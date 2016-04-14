using grid.render;
using UnityEngine;

namespace grid
{
    class BoolGridToTexture : rend.IRenderToTexture
    {
        private BoolGrid visGrid;
        private IGridRenderer gridRenderer;

        public BoolGridToTexture(BoolGrid visGrid, IGridRenderer gridRenderer)
        {
            this.visGrid = visGrid;
            this.gridRenderer = gridRenderer;
        }

        public void Render(RenderTexture rt, Camera camera)
        {
            Graphics.SetRenderTarget(rt);

            GL.PushMatrix();
            GL.LoadIdentity();

            GL.MultMatrix(camera.worldToCameraMatrix);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            foreach (var idx in visGrid.GetVisibleCells()) {
                gridRenderer.DrawCell(idx);
            }
            gridRenderer.Render();
            GL.PopMatrix();

            Graphics.SetRenderTarget(null);
        }
    }
}
