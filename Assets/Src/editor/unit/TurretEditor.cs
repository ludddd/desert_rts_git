using ai.target;
using UnityEditor;
using UnityEngine;
using wpn;

namespace editor.unit
{
    [CustomEditor(typeof(Turret))]
    class TurretEditor: Editor
    {
        private bool drawTargetingLines = false;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawButtons();
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("SelectShootObstacle"))
            {
                SelectShootObstacle();
            }
            if (GUILayout.Button("Select Target"))
            {
                SelectTarget();
            }
            var newValue = GUILayout.Toggle(drawTargetingLines, "Test Targeting");
            if (newValue != drawTargetingLines)
            {
                drawTargetingLines = newValue;
                SceneView.RepaintAll();
            }
        }

        private void SelectShootObstacle()
        {
            var obj = Turret.GetShootingObstacle();
            if (obj != null) {
                SelectObject(obj);
            }
        }

        private static void SelectObject(Object obj)
        {
            Selection.objects = new[] { obj };
        }

        private Turret Turret
        {
            get
            {
                return (Turret)target;
            }
        }

        private void SelectTarget()
        {
            var obj = GameObjectTarget.TargetToGameObject(Turret.Target);
            if (obj != null)
            {
                SelectObject(obj);
            }
        }

        private void OnSceneGUI()
        {
            if (drawTargetingLines)
            {
                DrawTargetingLines();
            }
        }

        private void DrawTargetingLines()
        {
            var targets = FindNearest.GetTargetsInRange(Turret.gameObject, Turret.weapon.shootDist);
            foreach (var target in targets)
            {
                var hitInfo = Turret.weapon.ShootCast(Turret.DefaultMuzzlePoint, target.Pos);
                if (hitInfo.HasValue)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(Turret.DefaultMuzzlePoint, hitInfo.Value.point);
                }
                else
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(Turret.DefaultMuzzlePoint, target.Pos);
                }
            }
        }        
    }
}
