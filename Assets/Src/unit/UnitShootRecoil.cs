using UnityEngine;
using editor.attr;

namespace unit
{
    class UnitShootRecoil : MonoBehaviour
    {
        public float value = 0;
        [LinkTo("gun")]
        public GameObject gun;
        private Quaternion initialRotation;

        void Start()
        {
            initialRotation = transform.localRotation;
        }

        void Update()
        {
            Vector3 rotationAxe = Vector3.Cross(gun.transform.forward, Vector3.up);  //TODO: fix possible bug when this is zero
            transform.localRotation = initialRotation;
            rotationAxe = transform.InverseTransformVector(rotationAxe).normalized;
            transform.localRotation = Quaternion.AngleAxis(value, rotationAxe) * initialRotation;
        }
    }
}
