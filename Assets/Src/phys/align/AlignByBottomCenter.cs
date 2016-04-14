using UnityEngine;

namespace phys.align
{
    public class AlignByBottomCenter: GroundAligner
    {
        public AlignByBottomCenter(Transform transform, BoxCollider collider) : base(transform, collider)
        {
        }

        public override void Align(Terrain terrain)
        {
            Debug.Assert(Transform.up.y >= 0);
            float groundY = SampleHeightInWorldCS(terrain, Transform.position);
            Vector3 objBottom = Transform.TransformPoint(Collider.center + new Vector3(0, -0.5f * Collider.size.y, 0));
            Transform.Translate(0, groundY - objBottom.y, 0, Space.World);

            Vector2 terrainCoord = WorldToTerrainCoord(terrain, Transform.position);
            Vector3 groundNormal = terrain.terrainData.GetInterpolatedNormal(terrainCoord.x, terrainCoord.y);
            Transform.rotation = Quaternion.FromToRotation(Transform.up, groundNormal) * Transform.rotation;
        }

        private static Vector2 WorldToTerrainCoord(Terrain terrain, Vector3 v)
        {
            return new Vector2((v.x - terrain.GetPosition().x) / terrain.terrainData.size.x,
                               (v.z - terrain.GetPosition().z) / terrain.terrainData.size.z);
        }
    }
}
