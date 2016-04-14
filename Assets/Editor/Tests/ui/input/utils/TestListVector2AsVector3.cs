using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace ui.input.utils
{
    [TestFixture]
    class TestListVector2AsVector3
    {
        private IList<Vector2> list;
        private readonly IEqualityComparer<Vector2> vector2Comparer = new Vector2Comparer(0.001f);

        [SetUp]
        public void Setup()
        {
            var terrain = CreateTerrain();
            var camera = CreateCamera(terrain);
            list = new ListVector2AsVector3(terrain.GetComponent<Collider>(), camera);
        }

        private static GameObject CreateTerrain()
        {
            var terrainData = new TerrainData();
            var terrain = Terrain.CreateTerrainGameObject(terrainData);
            return terrain;
        }

        private static Camera CreateCamera(GameObject terrain)
        {
            var camObj = new GameObject();
            var camera = camObj.AddComponent<Camera>();
            PositionCameraOverTerrain(terrain, camObj.transform);
            return camera;
        }
      
        private static void PositionCameraOverTerrain(GameObject terrain, Transform camTransform)
        {
            camTransform.rotation = Quaternion.LookRotation(Vector3.down);
            var terrainCollider = terrain.GetComponent<Collider>();
            var bounds = terrainCollider.bounds;
            camTransform.position = bounds.center + Vector3.up * (bounds.extents.y + 1.0f);
        }

        [Test]
        public void Add()
        {
            list.Add(Vector2.zero);
        }

        [Test]
        public void WrongCameraOrientation()
        {
            var terrain = CreateTerrain();
            var camera = CreateCamera(terrain);
            camera.transform.rotation = Quaternion.LookRotation(Vector3.up);
            var otherList = new ListVector2AsVector3(terrain.GetComponent<Collider>(), camera);
            Assert.Throws<Exception>(() => otherList.Add(Vector2.zero));
        }

        [Test]
        public void Count()
        {
            Assert.AreEqual(0, list.Count);
            list.Add(Vector2.zero);
            Assert.AreEqual(1, list.Count);
            list.Add(Vector2.zero);
            Assert.AreEqual(2, list.Count);
        }

        [Test]
        public void Clear()
        {
            list.Add(Vector2.zero);
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void Index()
        {
            list.Add(Vector2.zero);
            Assert.That(list[0], Is.EqualTo(Vector2.zero).Using(vector2Comparer));
            list.Add(Vector2.left);
            Assert.That(list[1], Is.EqualTo(Vector2.left).Using(vector2Comparer));
        }

        [Test]
        public void Enumerator()
        {
            list.Add(Vector2.zero);
            list.Add(Vector2.left);
            var it = list.GetEnumerator();
            it.MoveNext();
            Assert.That(it.Current, Is.EqualTo(Vector2.zero).Using(vector2Comparer));
            it.MoveNext();
            Assert.That(list[1], Is.EqualTo(Vector2.left).Using(vector2Comparer));
            Assert.False(it.MoveNext());
        }
    }

    class Vector2Comparer : IEqualityComparer<Vector2>
    {
        private readonly float eps;

        public Vector2Comparer(float eps)
        {
            this.eps = eps;
        }

        public bool Equals(Vector2 x, Vector2 y)
        {
            return Vector2.Distance(x, y) <= eps;
        }

        public int GetHashCode(Vector2 obj)
        {
            return obj.GetHashCode();
        }
    }
}
