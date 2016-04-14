using UnityEngine;

namespace utils
{
    static class Projection
    {
        public static Vector3 ProjectToTerrain(Camera camera, Collider collider, Vector3 viewportPoint)
        {
            var ray = camera.ViewportPointToRay(viewportPoint);
            RaycastHit hitInfo;
            if (collider.Raycast(ray, out hitInfo, float.MaxValue)) {
                return hitInfo.point;
            }
            var plane = new Plane(Vector3.up, Vector3.zero);
            float dist;
            if (plane.Raycast(ray, out dist)) {
                return ray.GetPoint(dist);
            }
            throw new System.Exception(string.Format("camera {0} ray is parallel to XZ plane", camera));
        }

    }
}
