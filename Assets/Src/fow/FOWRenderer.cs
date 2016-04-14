using System;
using UnityEngine;

namespace fow
{
    [Serializable]
    public class FOWRenderer
    {
        public const string TEXTURE_PARAM_NAME = "_FogOfWar";
        public const string MATRIX_PARAM_NAME = "_FOWToUV";

        [SerializeField]    //for debuging purposes
        private RenderTexture exploredArea = null;
        [SerializeField]    //for debuging purposes
        private RenderTexture fowRT = null;
        private rend.IRenderToTexture exploredAreaRenderer;
        private rend.IRenderToTexture visibleAreaRenderer;
        private Material blitMaterial;

        public void Init(int resolutionX, int resolutionY, rend.IRenderToTexture exploredAreaRenderer, rend.IRenderToTexture visibleAreaRenderer, Material blitMaterial)
        {
            Debug.Assert(exploredAreaRenderer != null);
            Debug.Assert(visibleAreaRenderer != null);
            exploredArea = new RenderTexture(resolutionX, resolutionY, 0);
            fowRT = new RenderTexture(resolutionX, resolutionY, 0);
            ClearRT(exploredArea);
            ClearRT(fowRT);
            this.exploredAreaRenderer = exploredAreaRenderer;
            this.visibleAreaRenderer = visibleAreaRenderer;
            this.blitMaterial = blitMaterial;
        }

        private static void ClearRT(RenderTexture rt)
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
        }

        public void Render(Camera camera)
        {
            if (!IsInited) return;
            ClearRT(exploredArea);
            exploredAreaRenderer.Render(exploredArea, camera);
            ClearRT(fowRT);
            Graphics.Blit(exploredArea, fowRT, blitMaterial);
            visibleAreaRenderer.Render(fowRT, camera);
        }

        private bool IsInited
        {
            get { return fowRT != null; }
        }

        public void ApplyFOW(Camera camera)
        {
            if (!IsInited) return;
            Shader.SetGlobalTexture(TEXTURE_PARAM_NAME, fowRT);
            Shader.SetGlobalMatrix(MATRIX_PARAM_NAME, GetFOWToUV(camera));
        }

        public void ApplyNoFOW()
        {
            if (!IsInited) return;
            Shader.SetGlobalTexture(TEXTURE_PARAM_NAME, Texture2D.whiteTexture);
        }

        private Matrix4x4 GetFOWToUV(Camera camera)
        {
            float dist = FOWPlane.GetDistanceToPoint(camera.transform.position);
            var cameraRect = cam.CameraMoveLimits.CameraRectAtDistance(camera, dist);
            var textureRect = new Rect(cameraRect.position - cameraRect.size, 2 * cameraRect.size);   // (-1, 1) to (0, 1) coord
            var projection = Matrix4x4.Ortho(textureRect.xMin, textureRect.xMax,
                                            textureRect.yMin, textureRect.yMax,
                                            camera.nearClipPlane, camera.farClipPlane);
            return (projection * camera.worldToCameraMatrix).transpose;
        }

        private Plane FOWPlane
        {
            get
            {
                //TODO: plane should be somehow passed from fow mesh...
                return new Plane(Vector3.up, Vector3.zero);
            }
        }
    }
}
