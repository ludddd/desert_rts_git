using UnityEngine;

namespace grid.mesh.test
{
    [RequireComponent(typeof(MeshFilter))]
    public class FowCellTest: MonoBehaviourEx
    {
        [SerializeField]
        private float size = 1.0f;

        [SerializeField]
        private bool[] visibility = new bool[9];

        [editor.attr.Button]
        public void Generate()
        {
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = MeshPoint9Generator.Build(size);
        }

        [editor.attr.Button]
        public void RepaintVisibility()
        {
            var mesh = GetComponent<MeshFilter>().sharedMesh;
            var uv = mesh.uv;
            for (int i = 0; i < visibility.Length; i++) {
                uv[i] = visibility[i] ? Vector2.one : Vector2.zero;
            }
            mesh.uv = uv;
        }
    }
}
