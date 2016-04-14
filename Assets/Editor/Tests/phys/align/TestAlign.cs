using System;
using NUnit.Framework;
using UnityEngine;

namespace phys.align
{
    [TestFixture(typeof(AlignByThreeLowest))]
    [TestFixture(typeof(AlignByBottomCenter))]
    [TestFixture(typeof(AlignByBestThree))]
    class TestAlign<T> where T:GroundAligner, new()
    {
        private GameObject obj;
        private BoxCollider bbox;
        private GroundAligner aligner;
        private GameObject terrainObj;

        [SetUp]
        public void Setup()
        {
            obj = new GameObject();
            bbox = obj.AddComponent<BoxCollider>();
            bbox.size = Vector3.one;
            aligner = (T)Activator.CreateInstance(typeof(T), obj.transform, obj.GetComponent<BoxCollider>());
            terrainObj = new GameObject();
        }

        [Test]
        public void AlignToSimplePlane()
        {
            var terrain = CreatePlainTerrain();
            aligner.Align(terrain);

            Assert.AreEqual(0.5f * bbox.size.y * Vector3.up, obj.transform.position);
            Assert.AreEqual(Quaternion.identity, obj.transform.rotation);
        }

        private Terrain CreatePlainTerrain()
        {
            var terrain = terrainObj.AddComponent<Terrain>();
            terrain.terrainData = new TerrainData();
            return terrain;
        }

        [TestCase(0, 10, 0)]
        public void AlignToTranslatedPlane(float x, float y, float z)
        {
            var terrain = CreatePlainTerrain();
            terrainObj.transform.Translate(x, y, z);
            aligner.Align(terrain);

            Assert.AreEqual(new Vector3(0, y + 0.5f * bbox.size.y, 0), obj.transform.position);
            Assert.AreEqual(Quaternion.identity, obj.transform.rotation);
        }
    }
}
