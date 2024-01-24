using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    [CustomPropertyDrawer(typeof(ExtensionRule))]
    internal class ExtensionRulePropertyDrawer : LibraryRuleBasePropertyDrawer
    {
        public new static readonly string ussClassName = "bewildered-library-extension-rule";

        private static readonly string _labelUssClassName = ussClassName + "__extension";

        protected override void CreateGUIElements(VisualElement rootElement, SerializedProperty property)
        {
            rootElement.AddToClassList(ussClassName);

            var text = new TextField();
            text.AddToClassList(_labelUssClassName);
            text.BindProperty(property.FindPropertyRelative("_extension"));
            rootElement.Add(text);
        }
    } 
}
