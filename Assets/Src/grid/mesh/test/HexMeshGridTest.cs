using UnityEngine;

namespace grid.mesh.test
{
    [RequireComponent(typeof(MeshFilter))]
    class HexMeshGridTest: MonoBehaviourEx
    {
        [SerializeField] private float cellSize = 1.0f;
        [SerializeField] private int rowCount = 2;
        [SerializeField] private int columnCount = 2;

        [editor.attr.Button]
        public void Update()
        {
            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = GridMeshGenerator.Build(new HexGrid(columnCount, rowCount, cellSize),
                HexMeshGenerator.Build(cellSize));
        }
    }
}
