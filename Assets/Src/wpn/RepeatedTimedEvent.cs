using UnityEngine;

namespace wpn
{
    class RepeatedTimedEvent
    {
        public delegate void EventHandler();

        private float delta;
        private float timer;

        public event EventHandler Event;

        public RepeatedTimedEvent(float delta)
        {
            Debug.Assert(delta > 0);
            this.delta = delta;
        }

        public void Update(float timeDelta)
        {
            timer += timeDelta;
            for (; timer >= 0; timer -= delta) {
                Event.Invoke();
            }
        }
    }
}
