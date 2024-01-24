using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    /// <summary>
    /// The base <see cref="Editor"/> used for <see cref="LibraryCollection"/>s.
    /// Inherit from it to create custom editors for collections.
    /// </summary>
    [CustomEditor(typeof(LibraryCollection), true)]
    public class LibraryCollectionEditor : Editor
    {
        public static readonly string emptyPromptTextUssClassName = "bewildered-library-empty-collection-text";

        public virtual string EmptyCollectionPromptText 
        {
            get { return LibraryConstants.DefaultEmptyCollectionPromptText + '"' + target.name + '"'; }
        }

        protected virtual void Awake() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected virtual void OnDestroy() { }

        protected override void OnHeaderGUI()
        {
            var collection = (LibraryCollection)target;
            
            using (new GUILayout.HorizontalScope("IN BigTitle"))
            {
                GUILayout.Label(collection.Icon, GUILayout.Width(32), GUILayout.Height(32));

                GUILayout.Label(collection.name, Styles.titleStyle, GUILayout.ExpandWidth(false), GUILayout.Height(32));

                using (new EditorGUI.DisabledScope(true))
                {
                    GUILayout.Label($"({collection.GetType().Name})", Styles.typeStyle, GUILayout.Height(32)); 
                }
            }
        }

        public sealed override VisualElement CreateInspectorGUI()
        {
            var rootElement = new VisualElement();
            rootElement.styleSheets.Add(LibraryUtility.LoadStyleSheet("LibraryCollection"));
            rootElement.styleSheets.Add(LibraryUtility.LoadStyleSheet($"LibraryCollection{(EditorGUIUtility.isProSkin ? "Dark" : "Light")}"));
            rootElement.styleSheets.Add(LibraryUtility.LoadStyleSheet("Common"));
            rootElement.styleSheets.Add(LibraryUtility.LoadStyleSheet($"Common{(EditorGUIUtility.isProSkin ? "Dark" : "Light")}"));

            CreateGUIElements(rootElement);
            
            return rootElement;
        }

        /// <summary>
        /// Override to add <see cref="VisualElement"/>s to the GUI for the <see cref="LibraryCollection"/>.
        /// </summary>
        /// <param name="rootElement">The root <see cref="VisualElement"/> to add elements to.</param>
        /// <remarks>The base implementation uses <see cref="CreateRulesListElement"/> and <see cref="CreateUpdateItemsButton"/>.</remarks>
        protected virtual void CreateGUIElements(VisualElement rootElement)
        {
            rootElement.Add(CreateRulesListElement());
            rootElement.Add(CreateUpdateItemsButton());
        }

        /// <summary>
        /// Creates a button that when pressed, will update the items in the <see cref="LibraryCollection"/>.
        /// </summary>
        /// <returns>The <see cref="VisualElement"/> for the button.</returns>
        protected VisualElement CreateUpdateItemsButton()
        {
            var updateItemsButton = new Button(UpdateItems)
            {
                name = "updateItemsButton",
                text = "Update Items In Collection",
                tooltip = "Update the items in the collection."
            };

            return updateItemsButton;
        }

        private Dictionary<Type, HelpBox> _activeRuleWarnings = new Dictionary<Type, HelpBox>();
        private static List<Type> _ruleTypes = new List<Type>(TypeCache.GetTypesDerivedFrom<LibraryRuleBase>());

        /// <summary>
        /// Creates a list showing the <see cref="RuleSet"/> of the <see cref="LibraryCollection"/>.
        /// </summary>
        /// <returns>The <see cref="VisualElement"/> for the <see cref="RuleSet"/>.</returns>
        protected VisualElement CreateRulesListElement()
        {
            var container = new VisualElement();
            var rulesView = new RulesView(serializedObject.FindProperty("_rules._rules"));
            container.Add(rulesView);

            UpdateRuleWarnings(container);
            container.schedule.Execute(() => { UpdateRuleWarnings(container); }).Every(100);

            return container;
        }

        private void UpdateRuleWarnings(VisualElement container)
        {
            List<Type> missingRuleTypes = new List<Type>(_ruleTypes);
            foreach (LibraryRuleBase rule in ((LibraryCollection) target).Rules)
            {
                Type ruleType = rule.GetType();
                missingRuleTypes.Remove(ruleType);

                if (!rule.IsRuleValid() && !_activeRuleWarnings.ContainsKey(ruleType))
                {
                    var helpBox = new HelpBox($"{ruleType.Name} - {rule.InvalidSettingsWarning} Some rules will be ignored.",
                        HelpBoxMessageType.Warning);
                    container.Add(helpBox);
                    _activeRuleWarnings.Add(ruleType, helpBox);
                }
                else if (rule.IsRuleValid() && _activeRuleWarnings.ContainsKey(ruleType))
                {
                    container.Remove(_activeRuleWarnings[ruleType]);
                    _activeRuleWarnings.Remove(ruleType);
                }
            }

            // Remove warnings for rules that were removed and were the
            // only instance of their type and while still having a warning.
            foreach (Type missingRuleType in missingRuleTypes)
            {
                if (_activeRuleWarnings.TryGetValue(missingRuleType, out HelpBox helpBox))
                {
                    container.Remove(helpBox);
                    _activeRuleWarnings.Remove(missingRuleType);
                }
            }
        }

        /// <summary>
        /// Creates the <see cref="VisualElement"/> to show when the <see cref="LibraryCollection"/> has no items in it.
        /// </summary>
        /// <returns>The <see cref="VisualElement"/> to show.</returns>
        public virtual VisualElement CreateEmptyCollectionPrompt()
        {
            var label = new Label(EmptyCollectionPromptText);
            label.AddToClassList(emptyPromptTextUssClassName);
            return label;
        }

        private void UpdateItems()
        {
            ((LibraryCollection)target).UpdateItems();
        }

        private static class Styles
        {
            public static readonly GUIStyle titleStyle;
            public static readonly GUIStyle typeStyle;

            static Styles()
            {
                titleStyle = new GUIStyle(EditorStyles.boldLabel);
                titleStyle.fontSize = 16;
                titleStyle.alignment = TextAnchor.MiddleLeft;

                typeStyle = new GUIStyle(EditorStyles.miniLabel);
                typeStyle.alignment = TextAnchor.MiddleLeft;
            }
        }
    } 
}
