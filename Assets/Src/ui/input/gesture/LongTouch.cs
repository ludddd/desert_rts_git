using System;
using UnityEngine;

namespace ui.input.gesture
{
    class LongTouch: IUpdatable
    {
        //TODO: move to editor GUI
        public const float LongTouchTime = 0.5f;   //in sec
        public const float ZeroMoveDistance = 5.0f;  //in pixels
        public const float CameraZeroMoveDistance = 0.1f;   //in world units

        private float touchStartTime;
        private Vector2 touchPos;
        private Vector3 camPos;
        private readonly Action onDetection;

        public LongTouch(Action onDetection)
        {
            this.onDetection = onDetection;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0)) {
                StartDetection();
            } else if (Input.GetMouseButton(0))
            {
                if (IsDetected()) {
                    Debug.Log("long touch detected");
                    onDetection.Invoke();
                }
            } else {
                StopDetection();
            }
        }

        private void StartDetection()
        {
            touchStartTime = Time.time;
            Debug.Assert(touchStartTime > 0);
            touchPos = Input.mousePosition;
            camPos = Camera.main.transform.position;
        }

        private void StopDetection()
        {
            touchStartTime = 0;
        }

        private bool IsDetected()
        {
            return IsDetectionInProgress && 
                   TimeSinceTouch >= LongTouchTime &&
                   CursorDistancePassed < ZeroMoveDistance &&
                   Vector3.Distance(camPos, Camera.main.transform.position) < CameraZeroMoveDistance;
        }

        private bool IsDetectionInProgress
        {
            get
            {
                return touchStartTime > 0;
            }
        }

        private float CursorDistancePassed
        {
            get
            {
                return Vector2.Distance(touchPos, Input.mousePosition);
            }
        }

        private float TimeSinceTouch
        {
            get
            {
                return Time.time - touchStartTime;
            }
        }
    }
}
