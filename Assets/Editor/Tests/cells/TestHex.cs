using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace grid
{
    [TestFixture]
    class TestHex
    {
        [TestCase(1)]
        [TestCase(3.5f)]
        public void Sizes(float size)
        {
            var hex = new Hex(size);
            Assert.AreEqual(hex.HalfHeight, size);
            Assert.AreEqual(hex.HalfWidth, Mathf.Cos(Mathf.Deg2Rad * 30) * size);
            Assert.AreEqual(2.0f * hex.HalfWidth, hex.Width);
            Assert.AreEqual(2.0f * hex.HalfHeight, hex.Height);
        }

        public static Vector3 GetInnerPoint(Hex hex, HexGrid.Dir dir)
        {
            var map = new Dictionary<HexGrid.Dir, Vector3>
            {
                {HexGrid.Dir.Left, new Vector3(-0.99f * hex.HalfWidth, 0, 0)},
                {HexGrid.Dir.Right, new Vector3(0.99f * hex.HalfWidth, 0, 0)},
                {HexGrid.Dir.TopLeft, new Vector3(-0.5f * hex.HalfWidth, 0, 0.65f * hex.HalfHeight)},
                {HexGrid.Dir.TopRight, new Vector3(0.5f * hex.HalfWidth, 0, 0.65f * hex.HalfHeight)},
                {HexGrid.Dir.BottomLeft, new Vector3(-0.5f * hex.HalfWidth, 0, -0.65f * hex.HalfHeight)},
                {HexGrid.Dir.BottomRight, new Vector3(0.5f * hex.HalfWidth, 0, -0.65f * hex.HalfHeight)},
            };
            return map[dir];
        }

        private static readonly Array directions = Enum.GetValues(typeof(HexGrid.Dir));

        [Test, TestCaseSource("directions")]
        public void TestGetInnerPoint(HexGrid.Dir dir)
        {
            var hex = new Hex(1);
            Assert.True(hex.Contains(GetInnerPoint(hex, dir)));
        }

        public static Vector3 GetOuterPoint(Hex hex, HexGrid.Dir dir)
        {
            var map = new Dictionary<HexGrid.Dir, Vector3>()
            {
                {HexGrid.Dir.Left, new Vector3(-1.01f * hex.HalfWidth, 0, 0)},
                {HexGrid.Dir.Right, new Vector3(1.01f * hex.HalfWidth, 0, 0)},
                {HexGrid.Dir.TopLeft, new Vector3(-0.5f * hex.HalfWidth, 0, 0.85f * hex.HalfHeight)},
                {HexGrid.Dir.TopRight, new Vector3(0.5f * hex.HalfWidth, 0, 0.85f * hex.HalfHeight)},
                {HexGrid.Dir.BottomLeft, new Vector3(-0.5f * hex.HalfWidth, 0, -0.85f * hex.HalfHeight)},
                {HexGrid.Dir.BottomRight, new Vector3(0.5f * hex.HalfWidth, 0, -0.85f * hex.HalfHeight)},
            };
            return map[dir];
        }

        [Test, TestCaseSource("directions")]
        public void TestGetPuterPoint(HexGrid.Dir dir)
        {
            var hex = new Hex(1);
            Assert.False(hex.Contains(GetOuterPoint(hex, dir)));
        }

        [TestCase(Hex.Vertex.Top, Hex.Vertex.Bottom)]
        [TestCase(Hex.Vertex.TopLeft, Hex.Vertex.BottomRight)]
        [TestCase(Hex.Vertex.TopRight, Hex.Vertex.BottomLeft)]
        public void OppositeVertex(Hex.Vertex v, Hex.Vertex opposite)
        {
            Assert.AreEqual(opposite, Hex.Opposite(v));
            Assert.AreEqual(v, Hex.Opposite(opposite));
        }
    }
}
