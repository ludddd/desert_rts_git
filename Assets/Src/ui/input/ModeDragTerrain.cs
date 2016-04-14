using ui.input.utils;
using UnityEngine;
using utils;

namespace ui.input
{
    class ModeDragTerrain : ControlModeSelector.IMode
    {
        [SerializeField]
        private UnityEventWithVector3 drag = null;
        private Vector3 _pos;
        [SerializeField]
        private UnityEventWithBool modeStateChanged = null;

        public void OnEnable()
        {
            var click = InputUtils.GetFirstActiveTouch();
            if (!click.HasValue) {             
                ModeSelector.SetDefaultMode();
                return;
            }
            ScreenObjectDetector obj = ScreenObjectDetector.AtCursor(click.Value);
            if (obj.NoObject) {
                ModeSelector.SetDefaultMode();
                return;
            }
            _pos = obj.Point;
            modeStateChanged.Invoke(true);
        }

        public void OnDisable()
        {
            modeStateChanged.Invoke(false);
        }

        public void Update()
        {
            var click = InputUtils.GetFirstActiveTouch();
            if (!click.HasValue) {
                ModeSelector.SetDefaultMode();
                return;
            }
            
            Camera cam = Camera.main;
            Plane plane = new Plane(cam.transform.forward, _pos);
            Ray ray = cam.ScreenPointToRay(InputUtils.GetPosition(click.Value));
            float dist;
            if (!plane.Raycast(ray, out dist)) {
                return;
            }
            Vector3 newPos = ray.GetPoint(dist);
            Vector3 delta = newPos - _pos;
            delta = Vector3.ProjectOnPlane(delta, cam.transform.forward);
            drag.Invoke(-delta);
        }
    }
}

