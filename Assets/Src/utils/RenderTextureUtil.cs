using UnityEngine;

namespace utils
{
    public static class RenderTextureUtil
    {
        public static Texture2D CreateTextureFromRT(RenderTexture rt)
        {
            Debug.Assert(rt != null);
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = rt;
            var texture = new Texture2D(rt.width, rt.height);
            texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture.Apply();
            RenderTexture.active = currentRT;
            return texture;
        }
    }
}
