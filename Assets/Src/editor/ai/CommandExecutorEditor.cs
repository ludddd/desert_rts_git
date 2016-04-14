using ai;
using UnityEditor;
using UnityEngine;

namespace editor.ai
{
    [CustomEditor(typeof (CommandExecutor))]
    class CommandExecutorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ShowCurrentCommand();
        }

        private void ShowCurrentCommand()
        {
            var executor = (CommandExecutor) target;
            GUILayout.Label(executor.GetCommandName());
        }
    }
}
