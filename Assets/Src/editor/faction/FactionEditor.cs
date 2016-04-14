using UnityEngine;
using UnityEditor;
using faction;
using unit;

namespace editor.faction
{
    [CustomPropertyDrawer(typeof(FactionData.FactionId))]
    public class FactionIdPropertyDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect guiRect, SerializedProperty property, GUIContent label)
        {
            var rectCutter = new layout.RectCutter(guiRect);
            SerializedProperty intId = property.FindPropertyRelative("id");

            EditorGUI.BeginProperty(guiRect, label, property);
            EditorGUI.LabelField(rectCutter.CutHorz(EditorGUIUtility.labelWidth), property.name);
            int newValue = FactionIdProperty(rectCutter, intId.intValue, property);
            EditorGUI.EndProperty();

            if (intId.intValue != newValue) {
                intId.intValue = newValue;  //TODO: a better to convert newValue to faction id inside of FactionId class...
                property.serializedObject.ApplyModifiedProperties();
                var gameObject = (property.serializedObject.targetObject as Component).gameObject;
                notifyChildren(gameObject);
            }
        }

        private int FactionIdProperty(layout.RectCutter rectCutter, int oldValue, SerializedProperty property)
        {
            if (FactionData.Instance == null)
            {
                return oldValue; //TODO: support something for the case there is not faction data in scene (maybe there is no need to include faction data in each scene?) 
            }
            GUIContent[] content = FactionData.Instance.FactionDataForGUI();
            if (IsReadOnly(property)) {
                EditorGUI.LabelField(rectCutter.Leftover(), content[oldValue]);
                return oldValue;
            } else {
                return EditorGUI.Popup(rectCutter.Leftover(), oldValue, content);
            }
        }

        private static bool IsReadOnly(SerializedProperty property)
        {
            return property.serializedObject.targetObject is FactionFromParent; //Not a best solution, but  don't want to add readonly information to runtime class at the moment
        }

        private static void notifyChildren(GameObject obj)
        {
            foreach(var childComp in obj.GetComponentsInChildren<UnitRepaint>()) {
                childComp.RepaintToFaction();
            }
        }
    }
}
