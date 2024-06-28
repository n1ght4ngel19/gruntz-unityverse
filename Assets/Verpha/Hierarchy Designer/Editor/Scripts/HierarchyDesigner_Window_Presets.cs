#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_Presets : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 outerScroll;
        private GUIStyle headerGUIStyle;
        private GUIStyle contentGUIStyle;
        private GUIStyle outerBackgroundGUIStyle;
        private GUIStyle innerBackgroundGUIStyle;
        private GUIStyle contentBackgroundGUIStyle;
        #endregion
        #region Const
        private const float labelWidth = 140;
        private const float toggleLabelWidth = 150;
        #endregion
        #region Presets Values
        private int selectedPresetIndex = 0;
        private string[] presetNames;
        private bool applyToFolders = true;
        private bool applyToSeparators = true;
        private bool applyToTag = true;
        private bool applyToLayer = true;
        private bool applyToTree = true;
        #endregion
        #endregion

        #region Window
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Helpers + "/Presets", false, HierarchyDesigner_Shared_MenuItems.LayerSeven + 1)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_Presets window = GetWindow<HierarchyDesigner_Window_Presets>("Presets");
            window.minSize = new Vector2(300, 150);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            presetNames = HierarchyDesigner_Configurable_Presets.GetPresetNames();
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out GUIStyle _, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Presets", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region Choose a Preset
            EditorGUILayout.BeginHorizontal(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Choose A Preset:", contentGUIStyle, GUILayout.Width(labelWidth));
            GUILayout.Space(4);
            selectedPresetIndex = EditorGUILayout.Popup(selectedPresetIndex, presetNames);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4);
            #endregion

            #region Apply Preset
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Apply Preset To:", contentGUIStyle, GUILayout.Width(labelWidth));
            GUILayout.Space(4);

            applyToFolders = HierarchyDesigner_Shared_GUI.DrawToggle("Folders", toggleLabelWidth, applyToFolders);
            applyToSeparators = HierarchyDesigner_Shared_GUI.DrawToggle("Separators", toggleLabelWidth, applyToSeparators);
            applyToTag = HierarchyDesigner_Shared_GUI.DrawToggle("GameObject's Tag", toggleLabelWidth, applyToTag);
            applyToLayer = HierarchyDesigner_Shared_GUI.DrawToggle("GameObject's Layer", toggleLabelWidth, applyToLayer);
            applyToTree = HierarchyDesigner_Shared_GUI.DrawToggle("Hierarchy Tree", toggleLabelWidth, applyToTree);
            GUILayout.Space(4);
            EditorGUILayout.EndVertical();
            #endregion
            #endregion

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            #region Footer
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable All Features", GUILayout.Height(25)))
            {
                EnableAllFeatures(true);
            }
            if (GUILayout.Button("Disable All Features", GUILayout.Height(25)))
            {
                EnableAllFeatures(false);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Confirm and Apply Preset", GUILayout.Height(30)))
            {
                ApplySelectedPreset();
            }
            EditorGUILayout.EndVertical();
            #endregion
        }

        private void ApplySelectedPreset()
        {
            if (selectedPresetIndex < 0 || selectedPresetIndex >= presetNames.Length) return;

            HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset selectedPreset = HierarchyDesigner_Configurable_Presets.Presets[selectedPresetIndex];

            string message = "Are you sure you want to override your current values for: ";
            List<string> changesList = new List<string>();
            if (applyToFolders) changesList.Add("Folders");
            if (applyToSeparators) changesList.Add("Separators");
            if (applyToTag) changesList.Add("GameObject's Tag");
            if (applyToLayer) changesList.Add("GameObject's Layer");
            if (applyToTree) changesList.Add("Hierarchy Tree");
            message += string.Join(", ", changesList) + "?\n\n*If you select 'confirm' all values will be overridden and saved.*";

            if (EditorUtility.DisplayDialog("Confirm Preset Application", message, "Confirm", "Cancel"))
            {
                if (applyToFolders)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToFolders(selectedPreset);
                }
                if (applyToSeparators)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToSeparators(selectedPreset);
                }
                if (applyToTag)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToTag(selectedPreset);
                }
                if (applyToLayer)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToLayer(selectedPreset);
                }
                if (applyToTree)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToTree(selectedPreset);
                }
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        private void EnableAllFeatures(bool enable)
        {
            applyToFolders = enable;
            applyToSeparators = enable;
            applyToTag = enable;
            applyToLayer = enable;
            applyToTree = enable;
        }
        #endregion
    }
}
#endif