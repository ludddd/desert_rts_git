using System;
using UnityEngine;

namespace ui.input.gesture
{
    class Draw: IUpdatable
    {
        private Vector2 startPos;
        private Action onDetection;

        public Draw(Action onDetection)
        {
            this.onDetection = onDetection;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0)) {
                startPos = Input.mousePosition;
            } else if (Input.GetMouseButton(0)) { 
                if (Vector2.Distance(startPos, Input.mousePosition) > LongTouch.ZeroMoveDistance) {
                    onDetection.Invoke();
                }
            }
        }
    }
}
