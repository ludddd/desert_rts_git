using ui.input.utils;
using UnityEngine;
using utils;

namespace ui.input
{
    class ModeAttack : ControlModeSelector.IMode
    {
        [SerializeField]
        private UnityEventWithGameObject enemyUnitClicked = null;

        public void Update()
        {
            var click = InputUtils.GetClickPointerId();
            if (!click.HasValue) return;
            ScreenObjectDetector obj = ScreenObjectDetector.AtCursor(click.Value);
            if (obj.IsEnemyUnit) {
                enemyUnitClicked.Invoke(obj.GameObject);
                ModeSelector.SetDefaultMode();
            }                
        }
    }
}
