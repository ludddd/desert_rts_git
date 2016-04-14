using UnityEngine;

namespace rend
{
    class FillWithColorTextureRenderer : IRenderToTexture
    {
        private Color color;

        public FillWithColorTextureRenderer(Color color)
        {
            this.color = color;
        }

        public void Render(RenderTexture rt, Camera camera)
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, color);
            RenderTexture.active = null;
        }
    }
}
