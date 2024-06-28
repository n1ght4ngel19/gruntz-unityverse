#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_AdvancedSettings : EditorWindow
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
        private const float enumPopupLabelWidth = 170;
        private const float toggleLabelWidth = 335;
        #endregion
        #region Temporary Values
        private HierarchyDesigner_Configurable_AdvancedSettings.HierarchyDesignerLocation tempHierarchyLocation;
        private bool tempEnableDynamicChangesCheckForGameObjectMainIcon;
        private bool tempEnableDynamicBackgroundForGameObjectMainIcon;
        private bool tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
        private bool tempEnableCustomizationForGameObjectComponentIcons;
        private bool tempEnableTooltipOnComponentIconHovered;
        private bool tempEnableActiveStateEffectForComponentIcons;
        private bool tempDisableComponentIconsForInactiveGameObjects;
        private bool tempIncludeBackgroundImageForGradientBackground;
        private bool tempExcludeFoldersFromCountSelectToolCalculations;
        private bool tempExcludeSeparatorsFromCountSelectToolCalculations;
        private static bool hasModifiedChanges = false;
        #endregion
        #endregion

        #region Window
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Configurations + "/Advanced Settings", false, HierarchyDesigner_Shared_MenuItems.LayerTwelve)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_AdvancedSettings  editorWindow = GetWindow<HierarchyDesigner_Window_AdvancedSettings >("Advanced Settings");
            editorWindow.minSize = new Vector2(400, 200);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            LoadTempValues();
        }

        private void LoadTempValues()
        {
            tempHierarchyLocation = HierarchyDesigner_Configurable_AdvancedSettings.HierarchyLocation;
            tempEnableDynamicChangesCheckForGameObjectMainIcon = HierarchyDesigner_Configurable_AdvancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon;
            tempEnableDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Configurable_AdvancedSettings.EnableDynamicBackgroundForGameObjectMainIcon;
            tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Configurable_AdvancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            tempEnableCustomizationForGameObjectComponentIcons = HierarchyDesigner_Configurable_AdvancedSettings.EnableCustomizationForGameObjectComponentIcons;
            tempEnableTooltipOnComponentIconHovered = HierarchyDesigner_Configurable_AdvancedSettings.EnableTooltipOnComponentIconHovered;
            tempEnableActiveStateEffectForComponentIcons = HierarchyDesigner_Configurable_AdvancedSettings.EnableActiveStateEffectForComponentIcons;
            tempDisableComponentIconsForInactiveGameObjects = HierarchyDesigner_Configurable_AdvancedSettings.DisableComponentIconsForInactiveGameObjects;
            tempIncludeBackgroundImageForGradientBackground = HierarchyDesigner_Configurable_AdvancedSettings.IncludeBackgroundImageForGradientBackground;
            tempExcludeFoldersFromCountSelectToolCalculations = HierarchyDesigner_Configurable_AdvancedSettings.ExcludeFoldersFromCountSelectToolCalculations;
            tempExcludeSeparatorsFromCountSelectToolCalculations = HierarchyDesigner_Configurable_AdvancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations;
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out GUIStyle _, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Advanced Settings", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region Core Features
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Core Features", contentGUIStyle);
            GUILayout.Space(4);
            EditorGUI.BeginChangeCheck();
            tempHierarchyLocation = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Designer Location", enumPopupLabelWidth, tempHierarchyLocation);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region GameObject's Main Icon
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("GameObject's Main Icon", contentGUIStyle);
            GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            tempEnableDynamicChangesCheckForGameObjectMainIcon = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Dynamic Changes Check For Main Icon", toggleLabelWidth, tempEnableDynamicChangesCheckForGameObjectMainIcon);
            tempEnableDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Dynamic Background", toggleLabelWidth, tempEnableDynamicBackgroundForGameObjectMainIcon);
            tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Precise Rect For Dynamic Background", toggleLabelWidth, tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region GameObject's Component Icons
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("GameObject's Component Icons", contentGUIStyle);
            GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            tempEnableCustomizationForGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Design Customization For Component Icons", toggleLabelWidth, tempEnableCustomizationForGameObjectComponentIcons);
            tempEnableTooltipOnComponentIconHovered = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Tooltip For Component Icons", toggleLabelWidth, tempEnableTooltipOnComponentIconHovered);
            tempEnableActiveStateEffectForComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Active State Effect For Component Icons", toggleLabelWidth, tempEnableActiveStateEffectForComponentIcons);
            tempDisableComponentIconsForInactiveGameObjects = HierarchyDesigner_Shared_GUI.DrawToggle("Disable Component Icons For Inactive GameObjects", toggleLabelWidth, tempDisableComponentIconsForInactiveGameObjects);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Separators
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Separators", contentGUIStyle);
            GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            tempIncludeBackgroundImageForGradientBackground = HierarchyDesigner_Shared_GUI.DrawToggle("Include Background Image For Gradient Background", toggleLabelWidth, tempIncludeBackgroundImageForGradientBackground);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Hierarchy Tools
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Hierarchy Tools", contentGUIStyle);
            GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            tempExcludeFoldersFromCountSelectToolCalculations = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Folders From Count-Select Tool Calculations", toggleLabelWidth, tempExcludeFoldersFromCountSelectToolCalculations);
            tempExcludeSeparatorsFromCountSelectToolCalculations = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Separators From Count-Select Tool Calculations", toggleLabelWidth, tempExcludeSeparatorsFromCountSelectToolCalculations);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
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

            if (GUILayout.Button("Update and Save Settings", GUILayout.Height(30)))
            {
                SaveSettings();
            }
            EditorGUILayout.EndVertical();
            #endregion
        }

        private void OnDestroy()
        {
            if (hasModifiedChanges)
            {
                bool shouldSave = EditorUtility.DisplayDialog("Advanced Settings Have Been Modified!",
                    "Do you want to save the changes you made to the advanced settings?",
                    "Save", "Don't Save");

                if (shouldSave)
                {
                    SaveSettings();
                }
            }
            hasModifiedChanges = false;
        }

        private void SaveSettings()
        {
            bool hierarchyLocationChanged = tempHierarchyLocation != HierarchyDesigner_Configurable_AdvancedSettings.HierarchyLocation;

            HierarchyDesigner_Configurable_AdvancedSettings.HierarchyLocation = tempHierarchyLocation;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon = tempEnableDynamicChangesCheckForGameObjectMainIcon;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableDynamicBackgroundForGameObjectMainIcon = tempEnableDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Configurable_AdvancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableCustomizationForGameObjectComponentIcons = tempEnableCustomizationForGameObjectComponentIcons;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableTooltipOnComponentIconHovered = tempEnableTooltipOnComponentIconHovered;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableActiveStateEffectForComponentIcons = tempEnableActiveStateEffectForComponentIcons;
            HierarchyDesigner_Configurable_AdvancedSettings.DisableComponentIconsForInactiveGameObjects = tempDisableComponentIconsForInactiveGameObjects;
            HierarchyDesigner_Configurable_AdvancedSettings.IncludeBackgroundImageForGradientBackground = tempIncludeBackgroundImageForGradientBackground;
            HierarchyDesigner_Configurable_AdvancedSettings.ExcludeFoldersFromCountSelectToolCalculations = tempExcludeFoldersFromCountSelectToolCalculations;
            HierarchyDesigner_Configurable_AdvancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations = tempExcludeSeparatorsFromCountSelectToolCalculations;
            HierarchyDesigner_Configurable_AdvancedSettings.SaveSettings();
            hasModifiedChanges = false;

            if (hierarchyLocationChanged)
            {
                HierarchyDesigner_Configurable_AdvancedSettings.GenerateConstantsFile(tempHierarchyLocation);
            }
        }

        private void EnableAllFeatures(bool enable)
        {
            tempEnableDynamicChangesCheckForGameObjectMainIcon = enable;
            tempEnableDynamicBackgroundForGameObjectMainIcon = enable;
            tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = enable;
            tempEnableCustomizationForGameObjectComponentIcons = enable;
            tempEnableTooltipOnComponentIconHovered = enable;
            tempEnableActiveStateEffectForComponentIcons = enable;
            tempDisableComponentIconsForInactiveGameObjects = enable;
            tempIncludeBackgroundImageForGradientBackground = enable;
            tempExcludeFoldersFromCountSelectToolCalculations = enable;
            tempExcludeSeparatorsFromCountSelectToolCalculations = enable;
            hasModifiedChanges = true;
        }
        #endregion
    }
}
#endif