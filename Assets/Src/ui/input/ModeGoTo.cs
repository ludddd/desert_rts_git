using ui.input.utils;
using UnityEngine;
using utils;

namespace ui.input
{
    class ModeGoTo : ControlModeSelector.IMode
    {
        [SerializeField]
        private UnityEventWithVector3 groundClicked = null;

        public void Update()
        {
            var click = InputUtils.GetClickPointerId();
            if (!click.HasValue) return;
            var obj = ScreenObjectDetector.AtCursor(click.Value);
            if (obj.IsTerrain) {
                groundClicked.Invoke(obj.Point);
            }
            if (!obj.NoObject) {
                ModeSelector.SetDefaultMode();
                return;
            }
        }
    }
}
