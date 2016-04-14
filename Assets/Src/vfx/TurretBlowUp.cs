using UnityEngine;

namespace vfx
{
    class TurretBlowUp: MonoBehaviour
    {
        [SerializeField] private float angularDrag = 0;
        [SerializeField] private float force = 0;
        [SerializeField] private float forceConeDeg = 0;

        public void Play(GameObject turretObj)
        {
            var rbody = turretObj.AddComponent<Rigidbody>();
            rbody.angularDrag = angularDrag;

            var renderer = turretObj.GetComponent<Renderer>();
            var bbox = turretObj.AddComponent<BoxCollider>();
            bbox.center = turretObj.transform.InverseTransformPoint(renderer.bounds.center);
            bbox.size = renderer.bounds.size;

            var forceDir = RandomVectorCone(forceConeDeg);
            rbody.AddForceAtPosition(force * forceDir, RandomPosOnBBoxBottomSide(bbox));
        }

        private Vector3 RandomVectorCone(float value)
        {
            return Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Quaternion.AngleAxis(value, Vector3.forward) * Vector3.up;
        }

        private Vector3 RandomPosOnBBoxBottomSide(BoxCollider bbox)
        {
            return bbox.center - 0.5f * new Vector3(Random.Range(0, bbox.size.x), 0, Random.Range(0, bbox.size.z));
        }
    }
}
