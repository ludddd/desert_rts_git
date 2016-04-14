using UnityEngine;

namespace fow.test
{
    class TestRenderScreenSpace: MonoBehaviour
    {
        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
        [SerializeField] private RenderTexture rt;
        [SerializeField]
        private new Camera camera;

        private void Update()
        {
            RenderTexture.active = rt;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;

            Graphics.SetRenderTarget(rt);            

            GL.PushMatrix();
            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            if (material.SetPass(0))
            {
                Graphics.DrawMeshNow(mesh, transform.localToWorldMatrix);
            }

            GL.PopMatrix();

            Graphics.SetRenderTarget(null);
        }
    }
}
