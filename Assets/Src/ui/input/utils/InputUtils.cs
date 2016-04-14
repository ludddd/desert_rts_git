using UnityEngine;

namespace ui.input.utils
{
    static class InputUtils
    {
        public static int? GetClickPointerId()
        {
            if (Input.GetMouseButtonUp(0))
            {
                return -1;
            }
            for (int touchId = 0; touchId < Input.touchCount; touchId ++)
            {
                if (Input.GetTouch(touchId).phase == TouchPhase.Ended)
                {
                    return touchId;
                }
            }
            return null;
        }

        public static int? GetFirstActiveTouch()
        {
            if (Input.GetMouseButton(0)) {  //TODO: works incorrectly with UNityRemote, cause touch is considerer mouse button...
                return -1;
            }
            if (Input.touchCount > 0)
            {
                return 0;
            }
            return null;
        }

        public static Vector2 GetPosition(int touchId)
        {
            return touchId == -1 ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : Input.GetTouch(touchId).position;
        }
    }
}
