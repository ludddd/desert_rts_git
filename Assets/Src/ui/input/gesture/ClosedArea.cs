using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ui.input.gesture
{
    class ClosedArea
    {
        public IEnumerable<Vector2> Points { get; private set; }

        public static ClosedArea Detect(IList<Vector2> points, float nearDist)
        {
            var closedList = GetMaxClosedList(points, nearDist);
            return closedList.Any() ? new ClosedArea(closedList) : null;
        }

        private static IEnumerable<Vector2> GetMaxClosedList(IList<Vector2> arr, float closeDist)
        {
            for (int lastIdx = arr.Count - 1; lastIdx >= 2; lastIdx--) {
                for (int currIdx = 0; currIdx < lastIdx - 2; currIdx++) {
                    if (Vector2.Distance(arr[currIdx], arr[lastIdx]) <= closeDist) {
                        return arr.Skip(currIdx - 1).Take(lastIdx - currIdx);
                    }
                }
            }
            return Enumerable.Empty<Vector2>();
        }

        private ClosedArea(IEnumerable<Vector2> points)
        {
            Points = points;
        }
    }
}
