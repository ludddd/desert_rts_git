using NUnit.Framework;
using UnityEngine;

namespace grid.mesh
{
    [TestFixture]
    class Test_SquareXZMeshGenerator
    {
        [Test]
        public void SquareVertex_partition1()
        {
            Assert.AreEqual(new Vector3[] { new Vector3(-1, 0, -1),
                                            new Vector3(1, 0, -1),
                                            new Vector3(-1, 0, 1),
                                            new Vector3(1, 0, 1)
            }, SquareXZMeshGenerator.SquareVertices(2, 1));
        }

        [Test]
        public void SquareVertex_partition2()
        {
            Assert.AreEqual(new Vector3[] { new Vector3(-1, 0, -1), new Vector3(0, 0, -1), new Vector3(1, 0, -1),
                                            new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0),
                                            new Vector3(-1, 0, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 1),
            }, SquareXZMeshGenerator.SquareVertices(2, 2));
        }
    }
}
