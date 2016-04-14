using System.Collections.Generic;
using UnityEngine;

namespace grid.mesh
{
    public static class SquareXZMeshGenerator
    {
        public static Mesh Build(float size, int partition = 1)
        {
            var mesh = new Mesh();
            mesh.vertices = SquareVertices(size, partition);
            mesh.uv = SquareUVs(partition);
            mesh.triangles = SquareTriangles(partition);
            return mesh;
        }

        public static Vector2[] SquareUVs(int partition)
        {
            var rez = new Vector2[(1 + partition) * (1 + partition)];
            for (int i = 0; i <= partition; i++) {
                for (int k = 0; k <= partition; k++) {
                    rez[i * (partition + 1) + k] = new Vector2(1.0f * k / partition, 1.0f * i / partition);
                }
            }
            return rez;
        }

        public static Vector3[] SquareVertices(float size, int partition)
        {
            var rez = new Vector3[(1 + partition) * (1 + partition)];
            for (int i = 0; i <= partition; i++) {
                for (int k = 0; k <= partition; k++) {
                    rez[i * (partition + 1) + k] = new Vector3(1.0f * k / partition - 0.5f, 0, 1.0f * i / partition - 0.5f) * size;
                }
            }
            return rez;
        }

        public static int[] SquareTriangles(int partition)
        {
            var rez = new List<int>(6 * partition * partition);
            for (int i = 0; i < partition; i++) {
                for (int k = 0; k < partition; k++) {
                    int leftUpperVertexIdx = i * (partition + 1) + k;
                    int rightUpperVertexIdx = leftUpperVertexIdx + 1;
                    int leftBottomVertexIdx = leftUpperVertexIdx + partition + 1;
                    int rightBottomVertexIdx = leftBottomVertexIdx + 1;
                    rez.Add(leftUpperVertexIdx);
                    rez.Add(rightUpperVertexIdx);
                    rez.Add(leftBottomVertexIdx);
                    rez.Add(rightUpperVertexIdx);
                    rez.Add(rightBottomVertexIdx);
                    rez.Add(leftBottomVertexIdx);
                }
            }
            return rez.ToArray();
        }
    }
}
