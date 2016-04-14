using utils;
using UnityEngine;

namespace ui.minimap
{
    [RequireComponent(typeof(Camera))]
    class MinimapCameraClicked: MonoBehaviour
    {
        [SerializeField]
        private UnityEventWithVector3 posClickedEvent;

        public void OnScreenPosClicked(Vector2 pos)
        {
            var cam = GetComponent<Camera>();
            var ray = cam.ScreenPointToRay(pos);
            RaycastHit hitInfo;
            if (TerrainCollider.Raycast(ray, out hitInfo, cam.farClipPlane)) {
                posClickedEvent.Invoke(hitInfo.point);
            }
        }

        private static Collider TerrainCollider
        {
            get
            {
                return Terrain.activeTerrain.GetComponent<Collider>();
            }
        }
    }
}
