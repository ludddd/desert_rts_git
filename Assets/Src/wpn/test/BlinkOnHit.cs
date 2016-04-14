using UnityEngine;

namespace wpn.test
{
    class BlinkOnHit: Damagable
    {
        [SerializeField]
        private Color color = Color.black;
        [SerializeField]
        private float time = 1.0f;
        private float timeSinceBlink = 0;
        private Color initialColor;

        private void Start()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                initialColor = renderer.material.color;
            }
        }

        public override void Damage(float value)
        {
            Blink();
        }

        private void Blink()
        {
            timeSinceBlink = time;
        }

        private void Update()
        {
            if (timeSinceBlink > 0)
            {
                timeSinceBlink = Mathf.Max(0, timeSinceBlink - Time.deltaTime);
                ApplyColor();
            }
        }

        private void ApplyColor()
        {
            var renderer = GetComponent<Renderer>();
            if (renderer == null) return;
            renderer.material.color = Color.Lerp(initialColor, color, timeSinceBlink/time);
        }

        public override bool IsAlive
        {
            get { return true; }
        }
    }
}
