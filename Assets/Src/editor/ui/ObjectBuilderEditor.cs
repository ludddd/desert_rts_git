using ui.input.visual;
using UnityEditor;
using UnityEngine;

namespace editor.ui
{
    [CustomEditor(typeof(ScreenSpaceTrail))]
    public class ObjectBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ScreenSpaceTrail myScript = (ScreenSpaceTrail)target;
            if (GUILayout.Button("Rebuild")) {
                myScript.BuildMesh();
            }
        }
    }
}
