using UnityEngine;

namespace grid.mesh.test
{
    [RequireComponent(typeof(MeshFilter))]
    public class IntensityShaderTest: MonoBehaviourEx
    {
        [editor.attr.Button]
        public void GenerateUV()
        {
            var mesh = GetComponent<MeshFilter>().sharedMesh;
            var uv = mesh.uv;
            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] = i%2 == 0 ? Vector2.zero : 2 * Vector2.one;
            }
            mesh.uv = uv;
        }
    }
}
