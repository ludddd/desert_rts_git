using System;
using UnityEngine;

namespace grid.mesh
{
    interface IBoolGridMesh
    {
        Mesh Mesh { get; }
        void Setup(Func<int, bool> cellFunc);
    }
}