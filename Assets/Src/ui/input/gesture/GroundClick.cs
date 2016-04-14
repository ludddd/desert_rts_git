using System;
using ui.input.utils;
using UnityEngine;
using utils;

namespace ui.input.gesture
{
    class GroundClick: IUpdatable
    {
        private readonly Action<Vector3> action;

        public GroundClick(Action<Vector3> action)
        {
            this.action = action;
        }

        public void Update()
        {
            var click = InputUtils.GetClickPointerId();
            if (!click.HasValue) return;            
            ScreenObjectDetector obj = ScreenObjectDetector.AtCursor(click.Value);
            if (obj.IsTerrain) {
                action.Invoke(obj.Point);
            }
        }
    }
}
