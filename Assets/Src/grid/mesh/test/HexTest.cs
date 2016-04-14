using UnityEngine;

namespace grid.mesh.test
{
    [RequireComponent(typeof (MeshFilter))]
    class HexTest : MonoBehaviourEx
    {
        [SerializeField] private float size = 1.0f;

        [editor.attr.Button]
        public void Update()
        {
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = HexMeshGenerator.Build(size);
        }
    }
}