using UnityEngine;
using UnityEditor;

namespace fow
{
    // Tell the RangeDrawer that it is a drawer for properties with the RangeAttribute.
    [CustomPropertyDrawer(typeof(ViewRange))]
    public class ViewRangeDrawer : PropertyDrawer
    {

        // Draw the property inside the given rect
        public override void OnGUI(Rect guiRect, SerializedProperty property, GUIContent label)
        {
            var rectCutter = new editor.layout.RectCutter(guiRect);

            EditorGUI.BeginProperty(guiRect, label, property);
            EditorGUI.LabelField(rectCutter.CutHorz(EditorGUIUtility.labelWidth), property.name);
            string value = "()";
            if (property.objectReferenceValue!= null) {
                value = "(" + ((ViewRange)property.objectReferenceValue).range.ToString() + ")";
            }
            EditorGUI.LabelField(rectCutter.CutHorz(50), value);
            property.objectReferenceValue = EditorGUI.ObjectField(rectCutter.Leftover(), property.objectReferenceValue, typeof(ViewRange), false);
            EditorGUI.EndProperty();
        }
    }
}
