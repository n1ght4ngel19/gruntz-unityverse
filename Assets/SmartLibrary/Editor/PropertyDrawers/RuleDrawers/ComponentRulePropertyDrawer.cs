using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace Bewildered.SmartLibrary.UI
{
    [CustomPropertyDrawer(typeof(ComponentRule))]
    internal class ComponentRulePropertyDrawer : LibraryRuleBasePropertyDrawer
    {
        protected override void CreateGUIElements(VisualElement rootElement, SerializedProperty property)
        {
            var field = new ComponentTypeField();
            field.style.flexGrow = 1;
            field.style.flexShrink = 1;
            field.BindProperty(property.FindPropertyRelative("_type._typeName"));
            rootElement.Add(field);
        }
    } 
}
