using UnityEngine;

namespace grid.mesh.test
{
    [RequireComponent(typeof(MeshFilter))]
    class MeshFieldGeneratorTest: MonoBehaviourEx
    {
        [SerializeField]
        private float size = 1.0f;
        [SerializeField]
        private int partition = 1;
        [SerializeField]
        private float fieldSize = 1.0f;

        [editor.attr.Button]
        public void Generate()
        {
            var cellMesh = SquareXZMeshGenerator.Build(size, partition);
            var field = SquareGrid.CreateForArea(new Bounds(Vector3.zero, new Vector3(fieldSize, 0, fieldSize)), size);
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = GridMeshGenerator.Build(field, cellMesh);
        }

        [editor.attr.Button]
        public void Generate8Point()
        {
            var cellMesh = MeshPoint9Generator.Build(size);
            var field = SquareGrid.CreateForArea(new Bounds(Vector3.zero, new Vector3(fieldSize, 0, fieldSize)), size);
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = GridMeshGenerator.Build(field, cellMesh);
        }
    }
}
