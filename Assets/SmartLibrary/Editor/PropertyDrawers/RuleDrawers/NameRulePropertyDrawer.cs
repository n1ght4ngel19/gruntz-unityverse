using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    [CustomPropertyDrawer(typeof(NameRule))]
    internal class NameRulePropertyDrawer : LibraryRuleBasePropertyDrawer
    {
        public new static readonly string ussClassName = "bewildered-library-name-rule";
        private static readonly string _ruleTypeUssClassName = ussClassName + "__rule-type";
        private static readonly string _ruleUssClassName = ussClassName + "__rule";

        protected override void CreateGUIElements(VisualElement rootElement, SerializedProperty property)
        {
            rootElement.AddToClassList(ussClassName);

            var matchTypeField = new EnumField();
            matchTypeField.BindProperty(property.FindPropertyRelative("_matchType"));
            matchTypeField.AddToClassList(_ruleTypeUssClassName);
            rootElement.Add(matchTypeField);

            var textField = new TextField();
            textField.BindProperty(property.FindPropertyRelative("_text"));
            textField.AddToClassList(_ruleUssClassName);
            rootElement.Add(textField);
        }
    } 
}
