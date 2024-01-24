using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    [CustomPropertyDrawer(typeof(TypeRule))]
    internal class TypeRulePropertyDrawer : LibraryRuleBasePropertyDrawer
    {
        protected override void CreateGUIElements(VisualElement rootElement, SerializedProperty property)
        {
            var field = new TypeField();
            field.style.flexGrow = 1;
            field.style.flexShrink = 1;
            field.BindProperty(property.FindPropertyRelative("_type._typeName"));
            rootElement.Add(field);
        }
    } 
}
