using UnityEngine;

namespace phys.align
{
    public abstract class GroundAligner
    {
        protected readonly Transform Transform;
        protected readonly BoxCollider Collider;

        protected GroundAligner(Transform transform, BoxCollider collider)
        {
            Transform = transform;
            Collider = collider;
        }

        public abstract void Align(Terrain terrain);

        public static float SampleHeightInWorldCS(Terrain terrain, Vector3 pos)
        {
            return terrain.transform.position.y + terrain.SampleHeight(pos);
        }

    }
}
