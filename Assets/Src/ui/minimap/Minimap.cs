using UnityEngine;
using UnityEngine.UI;

namespace ui.minimap
{
    public class Minimap : MonoBehaviour
    {

        void Start()
        {
            Bounds bounds = Terrain.activeTerrain.GetComponent<Collider>().bounds;
            SetupCameraByBounds(MyCamera, bounds);
            SetTextureSize(GetPixelCorrectRect(GetComponentInParent<Canvas>()));
        }

        private static void SetupCameraByBounds(Camera cam, Bounds bounds)
        {
            var camPos = 0.5f * (bounds.min + bounds.max);
            camPos.y = cam.transform.position.y;    //TODO: may cause a bug...
            cam.transform.position = camPos;
            cam.orthographicSize = 0.5f * (bounds.max.x - bounds.min.x);
            cam.aspect = (bounds.max.x - bounds.min.x) / (bounds.max.z - bounds.min.z);
        }

        public Camera MyCamera
        {
            get
            {
                return GetComponentInChildren<Camera>();
            }
        }

        public void OnRectTransformDimensionsChange()
        {
            var canvas = GetComponentInParent<Canvas>();
            if (canvas == null) return; //happens during CansasScaler::OnDisable
            SetTextureSize(GetPixelCorrectRect(canvas));
        }

        private void SetTextureSize(Rect rect)
        {
            var cam = MyCamera;
            cam.targetTexture =  new RenderTexture(Mathf.CeilToInt(rect.width), 
                Mathf.CeilToInt(rect.height), 
                cam.targetTexture.depth, 
                cam.targetTexture.format);
            GetComponent<RawImage>().texture = cam.targetTexture;
        }

        private Rect GetPixelCorrectRect(Canvas canvas)
        {
            Debug.Assert(canvas != null);
            return RectTransformUtility.PixelAdjustRect((RectTransform)transform, canvas);
        }
    }
}