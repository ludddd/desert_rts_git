using utils;
using UnityEngine;

namespace cam
{
    public static class CameraMoveLimits
    {
        public static Bounds calcCameraBounds(Camera cam, Bounds bounds)
        {
            float cameraHeight = cam.transform.position.y - bounds.min.y;
            Debug.Assert(cameraHeight > 0);
            var cameraRect = CameraRectAtDistance(cam, cameraHeight);
            if (cameraRect.width > bounds.size.x ||
                cameraRect.height > bounds.size.z) {
                LogOnce.LogWarning("bounds is too small for current camera height");
            }
            return new Bounds(new Vector3(bounds.center.x, cam.transform.position.y, bounds.center.z),
                              new Vector3(Mathf.Max(bounds.size.x - cameraRect.width, 0),
                                          0,
                                          Mathf.Max(bounds.size.z - cameraRect.height, 0)));
        }

        public static Rect CameraRectAtDistance(Camera cam, float dist)
        {
            if (cam.orthographic) {
                float horzOffset = cam.orthographicSize * cam.aspect;
                float vertOffset = cam.orthographicSize;
                return new Rect(-horzOffset, -vertOffset, 2.0f * vertOffset, 2.0f * horzOffset);
            } else {
                float horzOffset = Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView) * cam.aspect * dist;
                float vertOffset = Mathf.Tan(0.5f * Mathf.Deg2Rad * cam.fieldOfView) * dist;
                return new Rect(-horzOffset, -vertOffset, 2.0f * horzOffset, 2.0f * vertOffset);
            }
        }
    }
}
