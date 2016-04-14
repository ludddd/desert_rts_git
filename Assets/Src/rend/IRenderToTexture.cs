using UnityEngine;

namespace rend
{
    public interface IRenderToTexture
    {
        void Render(RenderTexture rt, Camera camera);
    }
}
