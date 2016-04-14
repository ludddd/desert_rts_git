using System.Collections.Generic;
using UnityEngine;

namespace grid.mesh
{
    public static class MeshPoint9Generator
    {
        public enum VERTEX
        {
            NW, N, NE,
            W, CENTER, E,
            SW, S, SE
        };

        private const int PARTITION = 2;
        private const int VERTEX_COUNT = (PARTITION + 1) * (PARTITION + 1);

        private static readonly VERTEX[] PerimeterIndicies = { VERTEX.NW, VERTEX.N, VERTEX.NE,
            VERTEX.E, VERTEX.SE, VERTEX.S, VERTEX.SW, VERTEX.W, VERTEX.NW };

        public static int VertexCount
        {
            get
            {
                return VERTEX_COUNT;
            }
        }

        public static Mesh Build(float size)
        {
            var mesh = SquareXZMeshGenerator.Build(size, PARTITION);
            ReorderVertices(mesh);
            return mesh;
        }

        private static void ReorderVertices(Mesh mesh)
        {
            Debug.Assert(mesh.vertexCount == VERTEX_COUNT);
            var rez = new List<int>(6 * PARTITION * PARTITION);
            for (int i = 0; i < PerimeterIndicies.Length - 1; i++) {
                rez.Add((int)VERTEX.CENTER);
                rez.Add((int)PerimeterIndicies[i]);
                rez.Add((int)PerimeterIndicies[i + 1]);
            }
            mesh.triangles = rez.ToArray();
        }

        public static bool IsCorner(VERTEX v)
        {
            return v == VERTEX.NW || v == VERTEX.NE || v == VERTEX.SW || v == VERTEX.SE;
        }
    }
}
