using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    [CustomPropertyDrawer(typeof(PrefabRule))]
    internal class PrefabRulePropertyDrawer : LibraryRuleBasePropertyDrawer
    {
        protected override void CreateGUIElements(VisualElement rootElement, SerializedProperty property)
        {
            var enumField = new EnumField();
            enumField.style.flexShrink = 1;
            enumField.BindProperty(property.FindPropertyRelative("_prefabType"));
            rootElement.Add(enumField);
        }
    }
}
