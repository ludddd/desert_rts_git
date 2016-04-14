using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace grid.mesh
{
    class HexMeshGenerator
    {
        private const int VERTEX_COUNT = 6;

        public static Mesh Build(float radius)
        {
            var mesh = new Mesh();
            mesh.vertices = BuildVertices(radius);
            mesh.uv = BuildCircleUV();
            mesh.triangles = BuildIndices();
            return mesh;
        }

        private static int[] BuildIndices()
        {
            var rez = new List<int>();
            const int centerIdx = 0;
            const int vertexStartIdx = 1;
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                rez.Add(centerIdx);
                rez.Add(vertexStartIdx + i);
                rez.Add(vertexStartIdx + (i + 1) % VERTEX_COUNT);
            }
            return rez.ToArray();
        }

        private static Vector2[] BuildCircleUV()
        {
            var rez = new List<Vector2>();
            rez.Add(Vector2.zero);
            rez.AddRange(Enumerable.Repeat(Vector2.one, VERTEX_COUNT));
            return rez.ToArray();
        }

        private static Vector3[] BuildVertices(float radius)
        {
            var rez = new List<Vector3>();
            rez.Add(Vector3.zero);
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                rez.Add(Quaternion.AngleAxis(360.0f * i / VERTEX_COUNT, Vector3.up) * Vector3.forward * radius);
            }
            return rez.ToArray();
        }
    }
}
