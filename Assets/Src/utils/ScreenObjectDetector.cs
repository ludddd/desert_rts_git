using UnityEngine;
using UnityEngine.EventSystems;
using select;
using faction;
using unit;

namespace utils
{
    public class ScreenObjectDetector
    {
        private static readonly ScreenObjectDetector noObject = new ScreenObjectDetector();

        private GameObject obj;
        private RaycastHit hitInfo;

        private ScreenObjectDetector()
        {
            obj = null;
        }

        private ScreenObjectDetector(RaycastHit hitInfo)
        {
            this.hitInfo = hitInfo;
            obj = hitInfo.collider.gameObject;
        }

        public static ScreenObjectDetector AtCursor(int pointerId)
        {
            if (EventSystem.current.IsPointerOverGameObject(pointerId)) {
                return noObject;
            }
            return AtPoint(Input.mousePosition);
        }

        public static ScreenObjectDetector AtPoint(Vector2 point)
        {
            Ray ray = Camera.main.ScreenPointToRay(point);
            RaycastHit hitInfo;
            if (!Physics.Raycast(ray, out hitInfo, RayCastDistance)) {
                return noObject;
            }
            return new ScreenObjectDetector(hitInfo);
        }

        private static float RayCastDistance
        {
            get
            {
                return Camera.main.farClipPlane;
            }
        }

        public bool NoObject
        {
            get
            {
                return obj == null;
            }
        }

        public bool IsTerrain
        {
            get
            {
                return obj != null && obj.GetComponent<Collider>() is TerrainCollider;
            }
        }

        public bool IsUnit
        {
            get
            {
                return obj != null && obj.IsUnit();
            }
        }

        public GameObject GameObject
        {
            get
            {
                return obj;
            }
        }

        public Vector3 Point    //TODO: better name + can crash if called on NoObject
        {
            get
            {
                return hitInfo.point;
            }
        }

        public bool IsEnemyUnit
        {
            get
            {
                if (!IsUnit) {
                    return false;
                }
                return PlayerFaction.FactionId.IsFoe(obj);
            }
        }
    }
}
