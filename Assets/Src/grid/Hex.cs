using System;
using UnityEngine;

namespace grid
{
    public class Hex
    {
        public enum Vertex
        {
            Center,
            Top,
            TopRight,
            BottomRight,
            Bottom,
            BottomLeft,
            TopLeft
        }

        private static readonly float Cos30 = Mathf.Cos(Mathf.Deg2Rad * 30);

        public float HalfHeight { get { return size; } }
        public float HalfWidth { get { return Cos30 * size; } }
        public float Width { get { return 2.0f * HalfWidth; } }
        public float Height { get { return 2.0f*HalfHeight; } }

        private readonly float size;
        private readonly Vector3 org;

        public Hex(float size)
        {
            this.size = size;
            this.org = Vector3.zero;
        }

        public Hex(float size, Vector3 org)
        {
            this.size = size;
            this.org = org;
        }

        public bool Contains(Vector3 pos)
        {
            if (!Bounds().Contains(pos))
            {
                return false;
            }
            //TODO: how to make this check more readable?
            if (Mathf.Abs(pos.z - org.z) + Mathf.Abs(pos.x - org.x) / (2.0f * Cos30) > HalfHeight) {
                return false;
            }
            return true;
        }

        public Vector3 GetVertex(Vertex v)
        {
            return org + GetVertexLocal(v);
        }

        private Vector3 GetVertexLocal(Vertex v)
        {
            switch (v) {
                case Vertex.Center:
                    return Vector3.zero;
                case Vertex.Top:
                    return new Vector3(0, 0, HalfHeight);
                case Vertex.TopRight:
                    return new Vector3(HalfWidth, 0, 0.5f * HalfHeight);
                case Vertex.BottomRight:
                    return new Vector3(HalfWidth, 0, -0.5f * HalfHeight);
                case Vertex.Bottom:
                    return -GetVertexLocal(Vertex.Top);
                case Vertex.BottomLeft:
                    return -GetVertexLocal(Vertex.TopRight);
                case Vertex.TopLeft:
                    return -GetVertexLocal(Vertex.BottomRight);
            }
            throw new ArgumentOutOfRangeException("unsupported hex vertex");
        }

        public Bounds Bounds()
        {
            return new Bounds(org, new Vector3(2.0f * HalfWidth, float.MaxValue, 2.0f * HalfHeight));
        }

        public static Vertex Opposite(Vertex v)
        {
            switch (v)
            {
                case Vertex.Center:
                    return Vertex.Center;
                default:
                    return (v - Vertex.Top + 3) % 6 + Vertex.Top;   //this order protected by tests
            }
        }
    };
}