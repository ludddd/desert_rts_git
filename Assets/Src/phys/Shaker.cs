using System;
using UnityEngine;

namespace phys
{
    [Serializable]
    class Shaker
    {
        enum State
        {
            Playing,
            Stopped
        };

        public interface IValueSetter
        {
            void SetValue(float value);
            void Reset();
        }

        public class RotationValueSetter : IValueSetter
        {
            private Transform transform;
            private Vector3 rotationAxe;
            private float maxAngle;
            private Quaternion initialRotation;

            public RotationValueSetter(Transform transform, Vector3 rotationAxe, float maxAngle)
            {
                this.transform = transform;
                this.rotationAxe = rotationAxe;
                this.maxAngle = maxAngle;
                initialRotation = transform.rotation;
            }

            public void SetValue(float value)
            {
                transform.rotation = Quaternion.AngleAxis(value * maxAngle, rotationAxe) * initialRotation;
            }

            public void Reset()
            {
                transform.rotation = initialRotation;
            }
        }

        public AnimationCurve offset = null;

        private float playTime;
        private float currentTime;
        private State state = State.Stopped;
        private IValueSetter valueSetter;

        public void Update(float timeDelta)
        {
            if (state == State.Playing) {
                currentTime += timeDelta;
                if (currentTime >= playTime) {
                    Stop();
                } else {
                    SetValue(offset.Evaluate(currentTime));
                }                
            }
        }

        void SetValue(float value)
        {
            valueSetter.SetValue(value);
        }

        public void Start(IValueSetter valueSetter, float playTime)
        {
            if (state == State.Playing) {
                Stop();
            }
            this.valueSetter = valueSetter;
            this.playTime = playTime;
            currentTime = 0;
            state = State.Playing;
        }

        public void Stop()
        {
            valueSetter.Reset();
            state = State.Stopped;
        }
    }
}
