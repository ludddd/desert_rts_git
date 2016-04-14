using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace grid.mesh.test
{
    [RequireComponent(typeof(MeshFilter))]
    public class Fow9CellTest : MonoBehaviourEx
    {
        private const int CELL_COUNT = 3;

        [SerializeField]
        private float cellSize = 1.0f;

        [SerializeField]
        private bool[] visibility = new bool[CELL_COUNT * CELL_COUNT];

        private Point9FowMesh mesh;

        [editor.attr.Button]
        public void Generate()
        {
            mesh = new Point9FowMesh(SquareGrid.CreateForArea(GetBounds(), cellSize), cellSize);

            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh.Mesh;

            RepaintVisibility();
        }

        private Bounds GetBounds()
        {
            return new Bounds(Vector3.zero, new Vector3(CELL_COUNT * cellSize, 0, CELL_COUNT * cellSize));
        }

        [editor.attr.Button]
        public void RepaintVisibility()
        {
            mesh.Setup(idx => visibility[idx]);
        }   
    }
}
