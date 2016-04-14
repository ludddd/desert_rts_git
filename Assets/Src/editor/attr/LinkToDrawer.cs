using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using utils;

namespace editor.attr
{
    [CustomPropertyDrawer(typeof(LinkTo))]
    public class LinkToDrawer : PropertyDrawer
    {
        abstract class Impl
        {
            protected SerializedProperty property;

            protected Impl(SerializedProperty property)
            {
                this.property = property;
            }

            public abstract float GetPropertyHeight(GUIContent label);
            public abstract void OnGUI(Rect guiRect, GUIContent label, LinkTo linkAttr);

            protected Object TargetObject
            {
                get
                {
                    return property.serializedObject.targetObject;
                }
            }
        }

        private bool IsGameObjectList(SerializedProperty property)
        {
            return property.type == typeof(MultipleObjectLink).Name;
        }

        private Impl GetImpl(SerializedProperty property)
        {
            return IsGameObjectList(property) ? (Impl)new MultiObjectImpl(property) : new SingleObjectImpl(property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetImpl(property).GetPropertyHeight(label);
        }

        public override void OnGUI(Rect guiRect, SerializedProperty property, GUIContent label)
        {
            GetImpl(property).OnGUI(guiRect, label, (LinkTo)attribute);
        }

        class SingleObjectImpl : Impl
        {
            public SingleObjectImpl(SerializedProperty property) :
            base(property)
            {
            }

            public override float GetPropertyHeight(GUIContent label)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }

            public override void OnGUI(Rect guiRect, GUIContent label, LinkTo linkAttr)
            {
                var rectCutter = new layout.RectCutter(guiRect);
                EditorGUI.BeginProperty(guiRect, label, property);
                EditorGUI.LabelField(rectCutter.CutHorz(EditorGUIUtility.labelWidth), linkAttr.pattern);
                EditorGUI.SelectableLabel(rectCutter.CutHorz(EditorGUIUtility.fieldWidth), linkAttr.pattern);
                EditorGUI.ObjectField(rectCutter.CutHorz(rectCutter.Leftover().width - 60), property.objectReferenceValue, typeof(GameObject), true);
                if (GUI.Button(rectCutter.Leftover(), "Setup")) {
                    SetupLink(property, linkAttr);
                }
                EditorGUI.EndProperty();
            }

            void SetupLink(SerializedProperty property, LinkTo attr)
            {
                GameObject gameObject = ((MonoBehaviour)TargetObject).gameObject;
                var matchedObjects = gameObject.transform.IterateBreadthFirst().
                    Where(obj => attr.Match(obj.name));
                property.objectReferenceValue = matchedObjects.Any() ? matchedObjects.First().gameObject : null;
            }
        }

        class MultiObjectImpl : Impl
        {
            public MultiObjectImpl(SerializedProperty property) :
            base(property)
            {
            }

            private object PropertyValue
            {
                get
                {
                    var fieldInfo = TargetObject.GetType().GetField(property.name);
                    return fieldInfo.GetValue(TargetObject);
                }
            }

            public override float GetPropertyHeight(GUIContent label)
            {
                float defaultHeight = EditorGUI.GetPropertyHeight(property, label);
                var link = (MultipleObjectLink)PropertyValue;
                if (link == null || link.objects == null) return defaultHeight;
                return defaultHeight * (1 + link.objects.Count);
            }

            public override void OnGUI(Rect guiRect, GUIContent label, LinkTo linkAttr)
            {
                Rect rect = guiRect;
                rect.height = EditorGUIUtility.singleLineHeight;
                CreateLabelAndButton(label, linkAttr, rect);
                CreateObjectList(rect);
            }

            private void CreateLabelAndButton(GUIContent label, LinkTo linkAttr, Rect rect)
            {
                var rectCutter = new layout.RectCutter(rect);
                EditorGUI.BeginProperty(rect, label, property);
                EditorGUI.LabelField(rectCutter.CutHorz(EditorGUIUtility.labelWidth), label);
                EditorGUI.SelectableLabel(rectCutter.CutHorz(EditorGUIUtility.labelWidth), linkAttr.pattern);
                if (GUI.Button(rectCutter.Leftover(), "Setup")) {
                    SetupMultipleLinks(property, linkAttr);
                }
                EditorGUI.EndProperty();
            }

            private void SetupMultipleLinks(SerializedProperty property, LinkTo attr)
            {
                var targetObject = property.serializedObject.targetObject;
                GameObject gameObject = ((MonoBehaviour)targetObject).gameObject;
                var objects = gameObject.transform.IterateBreadthFirst().
                    Select(obj => obj.gameObject).
                    Where(obj => attr.Match(obj.name));

                FieldInfo fieldInfo = targetObject.GetType().GetField(property.name);
                if (!objects.Any()) {
                    fieldInfo.SetValue(targetObject, null);
                } else {
                    MultipleObjectLink link = (MultipleObjectLink)fieldInfo.GetValue(targetObject);
                    if (link == null) {
                        link = new MultipleObjectLink();
                        fieldInfo.SetValue(targetObject, link);
                    }
                    link.objects.Clear();
                    var t = objects.ToList();
                    link.objects.AddRange(t);
                }
            }

            private void CreateObjectList(Rect rect)
            {
                MultipleObjectLink link = (MultipleObjectLink)PropertyValue;
                if (link == null) {
                    return;
                }

                EditorGUI.indentLevel++;
                for (int idx = 0; idx < link.objects.Count; idx++) {
                    var obj = link.objects[idx];
                    rect.y += EditorGUIUtility.singleLineHeight;
                    var rectCutter = new layout.RectCutter(rect);
                    EditorGUI.LabelField(rectCutter.CutHorz(EditorGUIUtility.labelWidth), "item " + idx);
                    EditorGUI.ObjectField(rectCutter.Leftover(), obj, typeof(GameObject), true);
                }
                EditorGUI.indentLevel--;
            }
        }
    }

}
