#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using static VInspector.Libs.VUtils;
using static VInspector.Libs.VGUI;



namespace VInspector
{
    [CustomPropertyDrawer(typeof(VariantsAttribute))]
    public class VIVariantsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            var variants = ((VariantsAttribute)attribute).variants;
            var curVal = prop.stringValue;

            EditorGUI.BeginProperty(rect, label, prop);

            var i = EditorGUI.IntPopup(rect, label.text, variants.ToList().IndexOf(curVal), variants, Enumerable.Range(0, variants.Length).ToArray());
            if (i == -1) i = 0;

            prop.stringValue = variants[i];

            EditorGUI.EndProperty();
        }
    }
}
#endif