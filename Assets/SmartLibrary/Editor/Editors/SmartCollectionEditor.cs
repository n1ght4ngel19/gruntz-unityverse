using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    [CustomEditor(typeof(SmartCollection))]
    internal class SmartCollectionEditor : LibraryCollectionEditor
    {
        private SmartCollection _smartCollection;

        protected override void Awake()
        {
            _smartCollection = (SmartCollection) target;
        }

        protected override void CreateGUIElements(VisualElement rootElement)
        {
            rootElement.Add(CreateFoldersElement());
            
            // TODO: Remove once support can be re-enabled.
            #if UNITY_2021_3_OR_NEWER
            rootElement.Add(new HelpBox("Finding assets within folders is not currently support in 2021.3 and newer due to features Unity removed. Support will be re-enabled as soon as possible.", HelpBoxMessageType.Warning));
            #endif
            
            rootElement.Add(CreateRulesListElement());
            rootElement.Add(CreateMissingRulesFolderElement());
            rootElement.Add(CreateUpdateItemsButton());

            var spinnerContainer = new VisualElement();
            spinnerContainer.style.alignItems = Align.Center;
            var spinner = new ProcessSpinner();
            spinner.schedule.Execute(() => spinner.SetDisplay(_smartCollection.IsSearching)).Every(100);
            spinnerContainer.Add(spinner);
            rootElement.Add(spinnerContainer);
        }

        private ReorderableList CreateFoldersElement()
        {
            ReorderableList folders = new ReorderableList(serializedObject.FindProperty("_folders"));
            folders.IsReorderable = true;
            folders.AddItem += list =>
            {
                list.ListProperty.arraySize++;
                list.ListProperty.GetArrayElementAtIndex(list.ListProperty.arraySize - 1).FindPropertyRelative("_doInclude")
                    .boolValue = true;
                list.ListProperty.serializedObject.ApplyModifiedProperties();
            };

            // Add a label to the folders header to indicate what folders will be searched.
            // Mostly to indicate that even if no folders are set, it will still search everything.
            var searchingScopeLabel = new Label();
            searchingScopeLabel.tooltip = "Has little impact on the search speed.";
            searchingScopeLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
            searchingScopeLabel.schedule.Execute(() =>
            {
                var targetCollection = (SmartCollection) target;
                if (targetCollection.Folders.Count == 0)
                {
                    searchingScopeLabel.text = "Searches all folders";
                    return;
                }

                bool containsInclude = targetCollection.Folders.Any(folder => folder.DoInclude);
                searchingScopeLabel.text =
                    containsInclude ? "Searches only specified folders" : "Searches all folders excluding specified";
            }).Every(100);

            folders.Q("header").Add(searchingScopeLabel);
            return folders;
        }

        private VisualElement CreateMissingRulesFolderElement()
        {
            var helpBox = new HelpBox();
            helpBox.text = "At least one folder or rule is required for the Smart Collection to find assets.";
            helpBox.messageType = HelpBoxMessageType.Warning;
            helpBox.schedule.Execute(() => UpdateBoxDisplay(helpBox)).Every(100);
            helpBox.style.marginTop = 8;
            helpBox.style.marginBottom = 8;
            
            UpdateBoxDisplay(helpBox);

            return helpBox;
            
            void UpdateBoxDisplay(VisualElement helpBox)
            {
                if (serializedObject.FindProperty("_rules._rules").arraySize == 0 && serializedObject.FindProperty("_folders").arraySize == 0)
                    helpBox.SetDisplay(true);
                else
                    helpBox.SetDisplay(false);
            }
        }

        public override VisualElement CreateEmptyCollectionPrompt()
        {
            var targetCollection = (SmartCollection)target;

            var rootElement = new VisualElement();
            rootElement.Add(base.CreateEmptyCollectionPrompt());

            var instructionlabel = new Label();
            instructionlabel.AddToClassList(emptyPromptTextUssClassName);
            rootElement.Add(instructionlabel);

            if (targetCollection.Rules.Count == 0 && targetCollection.Folders.Count == 0)
            {
                instructionlabel.text = LibraryConstants.SmartCollectionEmptyNoFiltersFolders;
                var settingsButton = new Button(() => Selection.activeObject = targetCollection);
                settingsButton.text = "Settings";
                settingsButton.style.alignSelf = Align.Center;
                rootElement.Add(settingsButton);
            }
            else
            {
                instructionlabel.text = LibraryConstants.SmartCollectionEmpty;
            }

            return rootElement;
        }
    } 
}
