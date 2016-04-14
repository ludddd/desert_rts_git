using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using utils;

namespace ui.input
{
    //TODO: should work with camera scrolling
    class ModeDrawGesture : ControlModeSelector.IMode
    {
        [SerializeField]
        private UnityEventWithVector2List closedAreaGesture = null;
        [SerializeField]
        private UnityEventWithVector3List pathGesture = null;

        private const float NEXT_POINT_DIST = 30.0f;
        private IList<Vector2> points;

        private void Awake()
        {
            points = new utils.ListVector2AsVector3(Terrain.activeTerrain.GetComponent<Collider>(), Camera.main);
        }

        public void OnEnable()
        {
            if (!Input.GetMouseButton(0)) {
                ModeSelector.SetDefaultMode();
                return;
            }
            ClearPoints();
            AddPoint(Input.mousePosition);
        }

        private void ClearPoints()
        {
            points.Clear();
        }

        public void OnDisable()
        {
            ClearPoints();
        }

        public void Update()
        {
            if (Input.GetMouseButton(0)) {
                if (Vector2.Distance(points.Last(), Input.mousePosition) > NEXT_POINT_DIST) {
                    AddPoint(Input.mousePosition);
                }
            } else if (Input.GetMouseButtonUp(0)) {
                CheckAndApplyGesture();
                ModeSelector.SetDefaultMode();
                return;
            } else {
                ModeSelector.SetDefaultMode();
            }
        }

        void AddPoint(Vector2 point)
        {
            points.Add(point);
        }

        void CheckAndApplyGesture()
        {
            //TODO: original vector3 points can be used here as args to events
            gesture.ClosedArea closedArea = gesture.ClosedArea.Detect(points, NEXT_POINT_DIST);
            if (closedArea != null) {
                closedAreaGesture.Invoke(closedArea.Points.ToList());
                return;
            }
            gesture.Path path = gesture.Path.Detect(points);
            if (path != null) {
                pathGesture.Invoke(path.Points);
                return;
            }
        }

        public IEnumerable<Vector2> Points
        {
            get { return points; }
        }
    }
}
