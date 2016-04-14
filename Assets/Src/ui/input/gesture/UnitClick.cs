using UnityEngine;
using System.Collections.Generic;
using utils;
using System.Linq;
using System;
using ui.input.utils;

namespace ui.input.gesture
{
    class UnitClick: IUpdatable
    {
        private const float BETWEEN_CLICK_TIME = 0.2f;
        private const int MAX_CLICK_DETECTED = 3;

        private List<float> clickTime = new List<float>();
        private GameObject lastClickedObject;
        private Action<GameObject>[] actions;

        public UnitClick(Action<GameObject>[] actions)
        {
            this.actions = actions;
        }

        public void Update()
        {
            var click = InputUtils.GetClickPointerId();
            if (!click.HasValue) return;
            ScreenObjectDetector obj = ScreenObjectDetector.AtCursor(click.Value);
            if (!obj.IsUnit) {
                lastClickedObject = null;
                clickTime.Clear();
                return;
            }
            if (lastClickedObject != obj.GameObject ||
                clickTime.Count == 0 ||
                clickTime.Last() < Time.time - BETWEEN_CLICK_TIME) {
                lastClickedObject = obj.GameObject;
                clickTime.Clear();
            }
            clickTime.Add(Time.time);
            if (clickTime.Count > MAX_CLICK_DETECTED) {
                clickTime.RemoveRange(0, clickTime.Count - MAX_CLICK_DETECTED);
            }
            if (clickTime.Count > 0 && clickTime.Count <= actions.Length) {
                actions[clickTime.Count - 1].Invoke(lastClickedObject);
            }
        }
    }
}
