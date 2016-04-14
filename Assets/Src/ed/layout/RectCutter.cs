using UnityEngine;

namespace editor.layout
{
    public class RectCutter
    {
        Rect rect;

        public RectCutter(Rect rect)
        {
            this.rect = rect;
        }

        public Rect CutHorz(float length)
        {
            length = Mathf.Min(rect.width, length);
            Rect rez = new Rect(rect.x, rect.y, length, rect.height);
            rect = new Rect(rect.x + length, rect.y, rect.width - length, rect.height);
            return rez;
        }

        public Rect CutHorzPercent(float percent)
        {
            return CutHorz(rect.width * percent);
        }

        public Rect Leftover()
        {
            return rect;
        }

        public Rect GetHorzCenterPercent(float percent)
        {
            rect = new Rect(rect.x + 0.5f * percent * rect.width, rect.y, percent * rect.width, rect.height);
            return rect;
        }
    }
}
