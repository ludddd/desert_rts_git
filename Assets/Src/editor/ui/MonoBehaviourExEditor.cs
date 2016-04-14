using editor.attr;
using UnityEditor;
using UnityEngine;

namespace editor.ui
{
    [CustomEditor(typeof(MonoBehaviourEx), true)]
    [CanEditMultipleObjects]
    public class MonoBehaviourExEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();            
            DrawMethodButtons();
        }

        private void DrawMethodButtons()
        {
            var type = target.GetType();
            System.Type buttonAttrType = typeof(ButtonAttribute);
            foreach (var method in type.GetMethods()) {
                if (method.GetCustomAttributes(buttonAttrType, true).Length == 0) {
                    continue;
                }
                if (GUILayout.Button(method.Name)) {
                    if (method.GetParameters().Length > 0) {
                        Debug.Log("Cannot invoke method " + method.Name + " cause it has parameters. Use ButtonAttribute only for method without parameters.");
                        continue;
                    }
                    InvokeMethodForAll(type, method);
                }
            }
        }

        private void InvokeMethodForAll(System.Type type, System.Reflection.MethodInfo method)
        {
            foreach (var target in targets) {
                if (target.GetType() == type) {
                    method.Invoke(target, null);
                }
            }
        }
    }
}
