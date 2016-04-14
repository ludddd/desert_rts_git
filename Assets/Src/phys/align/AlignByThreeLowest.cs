using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace phys.align
{
    public class AlignByThreeLowest: GroundAligner
    {
        private static readonly Vector3[] BottomPointOffsets = new Vector3[4]
        {
                new Vector3(-1, -1, -1),
                new Vector3(-1, -1, +1),
                new Vector3(+1, -1, +1),
                new Vector3(+1, -1, -1),
        };

        public AlignByThreeLowest(Transform transform, BoxCollider collider) : base(transform, collider)
        {
        }

        public override void Align(Terrain terrain)
        {
            ICollection<int> indicies = new List<int> { 0, 1, 2, 3 };
            Func<int, float> heightOverTerrain = i => HeightOverTerrain(GetPoint(i), terrain);
            int idx0 = GetIdxWithMinValue(indicies, heightOverTerrain);
            PlaceOnGroundInPoint(terrain, GetPoint(idx0));

            indicies.Remove(idx0);
            int idx1 = GetIdxWithMinValue(indicies, heightOverTerrain);
            PlaceOnGroundRotatingAroundPoint(terrain, GetPoint(idx0), GetPoint(idx1));

            indicies.Remove(idx1);
            int idx2 = GetIdxWithMinValue(indicies, heightOverTerrain);
            var ray = new Ray(GetPoint(idx0), GetPoint(idx1) - GetPoint(idx0));
            PlaceOnGroundRotatingAroundRay(terrain, ray, GetPoint(idx2));
        }

        private static float HeightOverTerrain(Vector3 pos, Terrain terrain)
        {
            return pos.y - SampleHeightInWorldCS(terrain, pos);
        }

        private int GetIdxWithMinValue(ICollection<int> indexArray, Func<int, float> valueFunc)
        {
            Debug.Assert(indexArray.Any());
            int minIdx = -1;
            float minValue = float.MaxValue;
            foreach (int idx in indexArray) {
                float value = valueFunc(idx);
                if (minValue > value) {
                    minIdx = idx;
                    minValue = value;
                }
            }
            return minIdx;
        }

        private void PlaceOnGroundInPoint(Terrain terrain, Vector3 point)
        {
            float dy = HeightOverTerrain(point, terrain);
            Transform.Translate(0, -dy, 0, Space.World);
        }

        private void PlaceOnGroundRotatingAroundPoint(Terrain terrain, Vector3 fixedPoint, Vector3 point)
        {
            Vector3 desired = point - new Vector3(0, HeightOverTerrain(point, terrain), 0);
            Quaternion rot = Quaternion.FromToRotation(point - fixedPoint, desired - fixedPoint);
            Transform.rotation = rot * Transform.rotation;
            PlaceOnGroundInPoint(terrain, fixedPoint);
        }

        private Vector3 GetPoint(int idx)
        {
            Debug.Assert(Transform.up.y > 0);
            return Transform.TransformPoint(Collider.center + 0.5f * Vector3.Scale(BottomPointOffsets[idx], Collider.size));
        }

        private void PlaceOnGroundRotatingAroundRay(Terrain terrain, Ray ray, Vector3 point)
        {
            Vector3 desired = point - new Vector3(0, HeightOverTerrain(point, terrain), 0);
            Vector3 rotOrg = ray.origin + Vector3.Project(point - ray.origin, ray.direction);
            Quaternion rot = Quaternion.FromToRotation(point - rotOrg, desired - rotOrg);
            Transform.rotation = rot * Transform.rotation;
            PlaceOnGroundInPoint(terrain, ray.origin);
        }
    }
}
