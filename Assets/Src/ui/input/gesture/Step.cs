using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ui.input.gesture.step
{
    public enum Signal
    {
        None, 
        TerrainClicked,
        PlayerUnitClicked,
        EnemyUnitClicked,
        DrawClosedArea,
        DrawPath,
        LongTouch
    };

    public abstract class AbstractStep
    {
        public abstract AbstractStep nextStep();
        virtual public Signal GetSignal()
        {
            return Signal.None;
        }
    }

    public class SignalStep: AbstractStep
    {
        private Signal signal;

        public static AbstractStep Get(Signal signal)
        {
            return new SignalStep(signal);
        }

        public override Signal GetSignal()
        {
            return signal;
        }

        public override AbstractStep nextStep()
        {
            throw new Exception("nextStep should be called for SignalStep");
        }

        private SignalStep(Signal signal)
        {
            this.signal = signal;
        }
    }

    public class Root: AbstractStep
    {
        private Root() { }

        public static AbstractStep Get()
        {
            return new Root();
        }

        override public AbstractStep nextStep()
        {
            if (Input.GetMouseButtonDown(0)) {
                return MouseDown.Get();
            }
            return this;
        }
    }

    public class MouseDown: AbstractStep
    {
        private const float ZERO_MOVE_DISTANCE = 5.0f;
        private const float LONG_TOUCH_TIME = 0.5f;

        private Vector2 startPos;
        private float startTime;

        private MouseDown()
        {
            startPos = Input.mousePosition;
            startTime = Time.time;
        }

        public static AbstractStep Get()
        {
            return new MouseDown();
        }

        override public AbstractStep nextStep()
        {
            if (Input.GetMouseButtonUp(0)) {
                return Root.Get();
            }
            if (Vector2.Distance(startPos, Input.mousePosition) > ZERO_MOVE_DISTANCE) {
                return Draw.Get(startPos);
            }
            if (Time.time - startTime > LONG_TOUCH_TIME) {
                return SignalStep.Get(Signal.LongTouch);
            }
            return this;
        }
    }

    public class Draw: AbstractStep
    {
        public const float NEXT_POINT_DIST = 30.0f;
        private IList<Vector2> points = new List<Vector2>();

        public static AbstractStep Get(Vector2 startPos)
        {
            return new Draw(startPos);
        }

        private Draw(Vector2 startPos)
        {
            points.Add(startPos);
            TryAddPoint(Input.mousePosition);
        }

        private void TryAddPoint(Vector2 point)
        {
            if (Vector2.Distance(point, points.Last()) >= NEXT_POINT_DIST) {
                points.Add(point);
            }
        }

        override public AbstractStep nextStep()
        {
            if (Input.GetMouseButtonUp(0)) {
                return Gesture.Get(points);
            }
            TryAddPoint(Input.mousePosition);
            return this;
        }
    }

    public class Gesture
    {
        public static AbstractStep Get(IList<Vector2> points)
        {
            return ClosedArea.Get(points);
        }

        private Gesture() { }
    }

    public class ClosedArea
    {
        public static AbstractStep Get(IList<Vector2> points)
        {
            var closedList = GetMaxClosedList(points, Draw.NEXT_POINT_DIST);
            if (closedList.Any()) {
                return SignalStep.Get(Signal.DrawClosedArea);
            }
            return Path.Get();
        }

        private ClosedArea() { }

        static IEnumerable<Vector2> GetMaxClosedList(IList<Vector2> arr, float closeDist)
        {
            for (int lastIdx = arr.Count - 1; lastIdx >= 2; lastIdx--) {
                for (int currIdx = 0; currIdx < lastIdx; currIdx++) {
                    if (Vector2.Distance(arr[currIdx], arr[lastIdx]) <= closeDist) {
                        return arr.Skip(currIdx - 1).Take(lastIdx - currIdx);
                    }
                }
            }
            return Enumerable.Empty<Vector2>();
        }
    }

    public class Path
    {
        public static AbstractStep Get()
        {
            return SignalStep.Get(Signal.DrawPath);
        }
    }
}
