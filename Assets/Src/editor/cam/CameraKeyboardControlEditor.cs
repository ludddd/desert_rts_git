using cam;
using UnityEditor;
using UnityEngine;

namespace editor.cam
{   
    [CustomEditor(typeof (CameraKeyboardControl))]
    class CameraKeyboardControlEditor : Editor
    {
        private bool drawCameraLimits;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawButtons();
        }

        private void DrawButtons()
        {
            var newValue = GUILayout.Toggle(drawCameraLimits, "Draw Camera Limits");
            if (newValue != drawCameraLimits) {
                drawCameraLimits = newValue;
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            if (drawCameraLimits) {
                DrawCameraLimits();
            }
        }

        private void DrawCameraLimits()
        {
            {
                var drawer = new BoundDrawer(CameraKeyboardControl.GetCameraBounds(CameraKeyboardControl.GetComponent<Camera>()));
                drawer.Draw(Color.white);
            }
            {
                var drawer = new BoundDrawer(Terrain.activeTerrain.GetComponent<Collider>().bounds);
                drawer.Draw(Color.white);
            }
        }

        private CameraKeyboardControl CameraKeyboardControl
        {
            get
            {
                return (CameraKeyboardControl)target;
            }
        }
    }

    class BoundDrawer
    {
        private readonly Bounds bounds;
        private readonly Vector3[] points = {
                new Vector3(-1, -1, -1),
                new Vector3(-1, -1, 1),
                new Vector3(-1, 1, 1),
                new Vector3(-1, 1, -1),
                new Vector3(1, -1, -1),
                new Vector3(1, -1, 1),
                new Vector3(1, 1, 1),
                new Vector3(1, 1, -1),
            };
        private readonly int[][] lines = {
                new[] {0, 1, 2, 3, 0},
                new[] {4, 5, 6, 7, 4},
                new[] {0, 4},
                new[] {1, 5},
                new[] {2, 6},
                new[] {3, 7},
            };

        public BoundDrawer(Bounds bounds)
        {
            this.bounds = bounds;
        }

        public void Draw(Color color)
        {
            Handles.color = color;            
            foreach (var line in lines) {
                for (int vertIdx = 0; vertIdx < line.Length - 1; vertIdx++) {
                    Handles.DrawLine(GetPoint(line[vertIdx]), GetPoint(line[vertIdx + 1]));
                }
            }
        }

        private Vector3 GetPoint(int idx)
        {
            return bounds.center + Vector3.Scale(bounds.extents, points[idx]);
        }
    }
}
