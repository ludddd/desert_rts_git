using UnityEngine;

namespace grid.mesh.test
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshGeneratorTest: MonoBehaviourEx
    {
        [SerializeField]
        private float size = 1.0f;
        [SerializeField]
        private int partition = 1;

        [editor.attr.Button]
        public void Generate()
        {
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = SquareXZMeshGenerator.Build(size, partition);
        }
    }
}
