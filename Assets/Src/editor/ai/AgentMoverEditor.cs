using ai;
using UnityEditor;
using UnityEngine;

namespace editor.ai
{
    [CustomEditor(typeof(AgentMover))]
    [CanEditMultipleObjects]
    class AgentMoverEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawAlignButton();
        }

        private void DrawAlignButton()
        {
            if (GUILayout.Button("Align With Terrain")) {
                foreach (var target in targets) {
                    if (target is AgentMover) {
                        (target as AgentMover).AlignWithTerrain();
                    }
                }
            }
        }
    }

}
