#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEditorInternal;
using System.Text.RegularExpressions;
using static VInspector.Libs.VUtils;
using static VInspector.Libs.VGUI;



namespace VInspector
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
    public class VISerializedDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            var indentedRect = EditorGUI.IndentedRect(rect);

            void header()
            {
                var headerRect = indentedRect.SetHeight(EditorGUIUtility.singleLineHeight);

                void foldout()
                {
                    var fullHeaderRect = headerRect.MoveX(3).AddWidthFromRight(17);

                    if (fullHeaderRect.IsHovered())
                        fullHeaderRect.Draw(Greyscale(1, .07f));

                    GUI.color = Color.clear;
                    if (GUI.Button(fullHeaderRect.AddWidth(-50), ""))
                        prop.isExpanded = !prop.isExpanded;
                    GUI.color = Color.white;


                    var triangleRect = rect.SetHeight(EditorGUIUtility.singleLineHeight);

                    EditorGUI.Foldout(triangleRect, prop.isExpanded, "");

                }
                void label_()
                {
                    SetLabelBold();
                    GUI.color = Greyscale(.9f);
                    GUI.Label(headerRect, prop.displayName);
                    GUI.color = Color.white;
                    ResetLabelStyle();

                }
                void count()
                {
                    kvpsProp.arraySize = EditorGUI.DelayedIntField(headerRect.SetWidthFromRight(48 + EditorGUI.indentLevel * 15), kvpsProp.arraySize);
                }
                void repeatedKeysWarning()
                {
                    if (eType != EventType.Repaint) return;


                    var hasRepeated = false;

                    for (int i = 0; i < kvpsProp.arraySize; i++)
                        hasRepeated |= kvpsProp.GetArrayElementAtIndex(i).FindPropertyRelative("isKeyRepeated").boolValue;


                    if (!hasRepeated) return;

                    var warningRect = headerRect.AddWidthFromRight(-prop.displayName.GetLabelWidth()).MoveX(3);

                    GUI.Label(warningRect.SetHeightFromMid(20).SetWidth(20), EditorGUIUtility.IconContent("Warning"));
                    GUI.color = new Color(1, .9f, .03f) * 1.1f;
                    GUI.Label(warningRect.MoveX(16), "Repeated keys");
                    GUI.color = Color.white;

                }

                foldout();
                label_();
                count();
                repeatedKeysWarning();

            }
            void list_()
            {
                if (!prop.isExpanded) return;

                SetupList(prop);

                list.DoList(indentedRect.AddHeightFromBottom(-EditorGUIUtility.singleLineHeight - 3));
            }


            SetupProps(prop);

            header();
            list_();

        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            SetupProps(prop);

            var height = EditorGUIUtility.singleLineHeight;

            if (prop.isExpanded)
            {
                SetupList(prop);
                height += list.GetHeight() + 3;
            }

            return height;
        }

        float GetListElementHeight(int index)
        {
            var kvpProp = kvpsProp.GetArrayElementAtIndex(index);
            var keyProp = kvpProp.FindPropertyRelative("Key");
            var valueProp = kvpProp.FindPropertyRelative("Value");

            float getPropHeight(SerializedProperty prop)
            {
                if (IsSingleLine(prop)) return EditorGUI.GetPropertyHeight(prop);


                var height = 1f;

                foreach (var childProp in GetChildren(prop, false))
                    height += EditorGUI.GetPropertyHeight(childProp) + 1;

                height += 10;


                return height;
            }

            return Mathf.Max(getPropHeight(keyProp), getPropHeight(valueProp));

        }

        void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect keyRect;
            Rect valueRect;
            Rect dividerRect;

            var kvpProp = kvpsProp.GetArrayElementAtIndex(index);
            var keyProp = kvpProp.FindPropertyRelative("Key");
            var valueProp = kvpProp.FindPropertyRelative("Value");

            void drawProp(Rect rect_, SerializedProperty prop)
            {
                if (IsSingleLine(prop))
                    EditorGUI.PropertyField(rect_.SetHeight(EditorGUIUtility.singleLineHeight), prop, GUIContent.none);
                else
                    foreach (var childProp in GetChildren(prop, false))
                    {
                        var childPropHeight = EditorGUI.GetPropertyHeight(childProp);

                        EditorGUI.PropertyField(rect_.SetHeight(childPropHeight), childProp, true);
                        rect_ = rect_.MoveY(childPropHeight + 2);
                    }

            }

            void rects()
            {
                var dividerWidh = IsSingleLine(valueProp) ? 6 : 16f;

                var dividerPos = dividerPosProp.floatValue;

                var fullRect = rect.AddWidthFromRight(-1).AddHeightFromMid(-2);

                keyRect = fullRect.SetWidth(fullRect.width * dividerPos - dividerWidh / 2);
                valueRect = fullRect.SetWidthFromRight(fullRect.width * (1 - dividerPos) - dividerWidh / 2);
                dividerRect = fullRect.MoveX(fullRect.width * dividerPos - dividerWidh / 2).SetWidth(dividerWidh).Resize(-1);

            }
            void key()
            {
                drawProp(keyRect, keyProp);

                if (kvpProp.FindPropertyRelative("isKeyRepeated").boolValue)
                    GUI.Label(keyRect.SetWidthFromRight(20).SetHeight(20).MoveY(-1), EditorGUIUtility.IconContent("Warning"));

            }
            void value()
            {
                drawProp(valueRect, valueProp);
            }
            void divider()
            {
                EditorGUIUtility.AddCursorRect(dividerRect, MouseCursor.ResizeHorizontal);

                if (!rect.IsHovered()) return;

                if (dividerRect.IsHovered())
                {
                    if (eType == EventType.MouseDown)
                        isDividerDragged = true;

                    if (eType == EventType.MouseUp || eType == EventType.MouseMove || eType == EventType.MouseLeaveWindow)
                        isDividerDragged = false;
                }

                if (isDividerDragged && eType == EventType.MouseDrag)
                    dividerPosProp.floatValue = Mathf.Clamp(dividerPosProp.floatValue + e.delta.x / rect.width, .2f, .8f);

            }

            rects();
            key();
            value();
            divider();

        }

        void DrawDictionaryIsEmpty(Rect rect) => GUI.Label(rect, "Dictionary is empty");



        IEnumerable<SerializedProperty> GetChildren(SerializedProperty prop, bool enterVisibleGrandchildren)
        {
            prop = prop.Copy();

            var startPath = prop.propertyPath;

            var enterVisibleChildren = true;

            while (prop.NextVisible(enterVisibleChildren) && prop.propertyPath.StartsWith(startPath))
            {
                yield return prop;
                enterVisibleChildren = enterVisibleGrandchildren;
            }

        }

        bool IsSingleLine(SerializedProperty prop) => prop.propertyType != SerializedPropertyType.Generic || !prop.hasVisibleChildren;



        public void SetupList(SerializedProperty prop)
        {
            if (list != null) return;

            SetupProps(prop);

            this.list = new ReorderableList(kvpsProp.serializedObject, kvpsProp, true, false, true, true);
            this.list.drawElementCallback = DrawListElement;
            this.list.elementHeightCallback = GetListElementHeight;
            this.list.drawNoneElementCallback = DrawDictionaryIsEmpty;

        }
        ReorderableList list;
        bool isDividerDragged;


        public void SetupProps(SerializedProperty prop)
        {
            if (this.prop != null) return;

            this.prop = prop;
            this.kvpsProp = prop.FindPropertyRelative("serializedKvps");
            this.dividerPosProp = prop.FindPropertyRelative("dividerPos");


        }
        SerializedProperty prop;
        SerializedProperty kvpsProp;
        SerializedProperty dividerPosProp;

    }

}
#endif