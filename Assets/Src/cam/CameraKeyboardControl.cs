using UnityEngine;

namespace cam
{
    [RequireComponent(typeof(Camera))]
    public class CameraKeyboardControl : MonoBehaviour
    {
        [SerializeField]
        private float sensivity = 1.0f;

        private void Update()
        {
            var delta = Input.GetAxis("Horizontal") * Vector3.right +
                        Input.GetAxis("Vertical") * Vector3.forward;
            delta *= sensivity*Time.deltaTime;
            Move(delta);
        }

        public void Move(Vector3 delta)
        {
            transform.Translate(delta, Space.World);
            SetPos(transform.position);
        }

        public void SetPos(Vector3 pos)
        {
            transform.position = FitInBounds(pos, GetCameraBounds(GetComponent<Camera>()));
        }

        public static Vector3 FitInBounds(Vector3 pos, Bounds bounds)
        {
            return bounds.Contains(pos) ? pos : bounds.ClosestPoint(pos);
        }

        //TODO: cache value until camera height is changed
        public static Bounds GetCameraBounds(Camera cam)
        {
            return CameraMoveLimits.calcCameraBounds(cam, Terrain.activeTerrain.GetComponent<Collider>().bounds);
        }
    }
}
