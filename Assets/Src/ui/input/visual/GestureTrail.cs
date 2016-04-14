using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ui.input.visual
{

    [RequireComponent(typeof(ModeDrawGesture))]
    [RequireComponent(typeof(ScreenSpaceTrail))]
    public class GestureTrail : MonoBehaviour
    {
        private ModeDrawGesture source;
        private ScreenSpaceTrail target;

        private void Awake()
        {
            source = GetComponent<ModeDrawGesture>();
            target = GetComponent<ScreenSpaceTrail>();
        }

        private void OnEnable()
        {
            target.SetPoints(Enumerable.Empty<Vector2>());
        }

        private void Update()
        {
            target.SetPoints(source.Points);
        }
        
    }
}
