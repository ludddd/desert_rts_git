using UnityEngine;

namespace cam
{
    [RequireComponent(typeof(Camera))]
    public class CameraBorderControl : MonoBehaviour
    {

        [SerializeField]
        private float scrollAreaSize = 0; 
        [SerializeField]
        private float scrollSpeed = 0.0f;
        [SerializeField]
        private bool mouseSupported = false;

        void Update()
        {
            if (Input.touchCount == 1) {
                var touch = Input.GetTouch(0);
                ProcessTouchPos(touch.position);
            } else if (mouseSupported)
            {
                ProcessTouchPos(Input.mousePosition);
            }
        }

        private void ProcessTouchPos(Vector3 touchPos)
        {
            if (touchPos.x <= ScrollAreaSize) {
                transform.position -= scrollSpeed * Vector3.right * Time.deltaTime;
            } else if (touchPos.x >= Screen.width - ScrollAreaSize) {
                transform.position += scrollSpeed * Vector3.right * Time.deltaTime;
            }
            if (touchPos.y <= ScrollAreaSize) {
                transform.position -= scrollSpeed * Vector3.forward * Time.deltaTime;
            } else if (touchPos.y >= Screen.height - ScrollAreaSize) {
                transform.position += scrollSpeed * Vector3.forward * Time.deltaTime;
            }
            transform.position = CameraKeyboardControl.FitInBounds(transform.position, CameraKeyboardControl.GetCameraBounds(GetComponent<Camera>()));
        }

        public float ScrollAreaSize
        {
            get { return scrollAreaSize * Mathf.Min(Camera.main.pixelHeight, Camera.main.pixelWidth); }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            var width = Camera.current.pixelWidth;
            var height = Camera.current.pixelHeight;
            Gizmos.DrawGUITexture(new Rect(0, 0, ScrollAreaSize, height), Texture2D.whiteTexture);
            Gizmos.DrawGUITexture(new Rect(0, 0, width, ScrollAreaSize), Texture2D.whiteTexture);
            Gizmos.DrawGUITexture(new Rect(width - ScrollAreaSize, 0, ScrollAreaSize, height), Texture2D.whiteTexture);
            Gizmos.DrawGUITexture(new Rect(0, height - ScrollAreaSize, width, ScrollAreaSize), Texture2D.whiteTexture);
        }
    }
}