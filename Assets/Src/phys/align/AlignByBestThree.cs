using System;
using UnityEngine;

namespace phys.align
{
    public class AlignByBestThree: GroundAligner
    {
        public AlignByBestThree(Transform transform, BoxCollider collider) : base(transform, collider)
        {
        }

        public override void Align(Terrain terrain)
        {
            Plane bestPlane = FindBestFitPlane(terrain, Transform, GetBottomPoints());
            float groundY = GetPlaneYByXZ(bestPlane, Transform.position.x, Transform.position.z);
            Vector3 objBottom = Transform.TransformPoint(Collider.center + new Vector3(0, -0.5f * Collider.size.y, 0));
            Transform.Translate(0, groundY - objBottom.y, 0, Space.World);
            Transform.rotation = Quaternion.FromToRotation(Transform.up, bestPlane.normal) * Transform.rotation;
        }

        private Vector3[] GetBottomPoints()
        {
            Debug.Assert(Transform.up.y > 0);
            var rez = new Vector3[4]
            {
                new Vector3(-1, -1, -1),
                new Vector3(-1, -1, +1),
                new Vector3(+1, -1, +1),
                new Vector3(+1, -1, -1),
            };
            for (var i = 0; i < rez.Length; i++) {
                rez[i] = 0.5f * Vector3.Scale(rez[i], Collider.size);
                rez[i] = Collider.center + rez[i];
                rez[i] = Transform.TransformPoint(rez[i]);
            }
            return rez;
        }

        private static Plane FindBestFitPlane(Terrain terrain, Transform transform, Vector3[] offsets)
        {
            for (var i = 0; i < offsets.Length; i++) {
                offsets[i].y = SampleHeightInWorldCS(terrain, offsets[i]);
            }
            Func<int, PointDescr> getOffsetDescr = i => new PointDescr
            {
                plane = BuildPlaneBy3Points(offsets[(i + 1) % offsets.Length],
                                            offsets[(i + 2) % offsets.Length],
                                            offsets[(i + 3) % offsets.Length]),
                point = offsets[i]
            };
            var min = getOffsetDescr(0);
            for (int i = 1; i < offsets.Length; i++) {
                var item = getOffsetDescr(i);
                if (item.dist < min.dist) {
                    min = item;
                }
            }
            return min.plane;
        }

        private class PointDescr
        {
            public Plane plane { get; set; }
            public Vector3 point { get; set; }
            public float dist
            {
                get
                {
                    return plane.GetDistanceToPoint(point);
                }
            }
        };
        
        static Plane BuildPlaneBy3Points(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Plane rez = new Plane();
            rez.Set3Points(v1, v2, v3);
            return rez;
        }

        static float GetPlaneYByXZ(Plane p, float x, float z)
        {
            return -(p.distance + p.normal.x * x + p.normal.z * z) / p.normal.y;
        }
    }
}
