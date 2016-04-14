using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace grid.mesh
{
    static class GridMeshGenerator
    {
        public static Mesh Build(IGrid grid, Mesh proto)
        {
            Mesh mesh = new Mesh();
            var vertices = new List<Vector3>(proto.vertexCount * grid.CellCount);
            var uv = new List<Vector2>(proto.vertexCount * grid.CellCount);
            var indicies = new List<int>(proto.triangles.Length * grid.CellCount);

            for (int i = 0; i < grid.CellCount; i++) {
                var pos = grid.CellCenter(i);
                vertices.AddRange(proto.vertices.Select(v => pos + v));
                uv.AddRange(proto.uv);
                indicies.AddRange(proto.triangles.Select(idx => idx + i * proto.vertexCount));
            }

            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = indicies.ToArray();
            return mesh;
        }
    }
}
