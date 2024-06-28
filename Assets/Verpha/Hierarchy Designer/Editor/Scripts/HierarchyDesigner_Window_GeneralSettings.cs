#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_GeneralSettings : EditorWindow
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
        private const float enumPopupLabelWidth = 90;
        private const float toggleLabelWidth = 270;
        private const float maskFieldLabelWidth = 105;
        #endregion
        #region Temporary Values
        private HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode tempLayoutMode;
        private bool tempEnableGameObjectMainIcon;
        private bool tempEnableGameObjectComponentIcons;
        private bool tempEnableHierarchyTree;
        private bool tempEnableGameObjectTag;
        private bool tempEnableGameObjectLayer;
        private bool tempEnableHierarchyRows;
        private bool tempEnableHierarchyButtons;
        private bool tempEnableMajorShortcuts;
        private bool tempDisableHierarchyDesignerDuringPlayMode;
        private bool tempExcludeFolderProperties;
        private bool tempExcludeTransformForGameObjectComponentIcons;
        private bool tempExcludeCanvasRendererForGameObjectComponentIcons;
        private List<string> tempExcludedTags;
        private List<string> tempExcludedLayers;
        private static bool hasModifiedChanges = false;
        #endregion
        #endregion

        #region Window
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Configurations + "/General Settings", false, HierarchyDesigner_Shared_MenuItems.LayerTen)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_GeneralSettings editorWindow = GetWindow<HierarchyDesigner_Window_GeneralSettings>("General Settings");
            editorWindow.minSize = new Vector2(500, 300);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            LoadTempValues();
        }

        private void LoadTempValues()
        {
            tempLayoutMode = HierarchyDesigner_Configurable_GeneralSettings.LayoutMode;
            tempEnableGameObjectMainIcon = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectMainIcon;
            tempEnableGameObjectComponentIcons = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectComponentIcons;
            tempEnableGameObjectTag = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectTag;
            tempEnableGameObjectLayer = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectLayer;
            tempEnableHierarchyTree = HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyTree;
            tempEnableHierarchyRows = HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyRows;
            tempEnableHierarchyButtons = HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyButtons;
            tempEnableMajorShortcuts = HierarchyDesigner_Configurable_GeneralSettings.EnableMajorShortcuts;
            tempDisableHierarchyDesignerDuringPlayMode = HierarchyDesigner_Configurable_GeneralSettings.DisableHierarchyDesignerDuringPlayMode;
            tempExcludeFolderProperties = HierarchyDesigner_Configurable_GeneralSettings.ExcludeFolderProperties;
            tempExcludeTransformForGameObjectComponentIcons = HierarchyDesigner_Configurable_GeneralSettings.ExcludeTransformForGameObjectComponentIcons;
            tempExcludeCanvasRendererForGameObjectComponentIcons = HierarchyDesigner_Configurable_GeneralSettings.ExcludeCanvasRendererForGameObjectComponentIcons;
            tempExcludedTags = HierarchyDesigner_Configurable_GeneralSettings.ExcludedTags;
            tempExcludedLayers = HierarchyDesigner_Configurable_GeneralSettings.ExcludedLayers;
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out GUIStyle _, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("General Settings", headerGUIStyle);
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
            tempLayoutMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Layout Mode", enumPopupLabelWidth, tempLayoutMode);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Main Features
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Main Features", contentGUIStyle);
            GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            tempEnableGameObjectMainIcon = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Main Icon", toggleLabelWidth, tempEnableGameObjectMainIcon);
            tempEnableGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Component Icons", toggleLabelWidth, tempEnableGameObjectComponentIcons);
            tempEnableGameObjectTag = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Tag", toggleLabelWidth, tempEnableGameObjectTag);
            tempEnableGameObjectLayer = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Layer", toggleLabelWidth, tempEnableGameObjectLayer);
            tempEnableHierarchyTree = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Hierarchy Tree", toggleLabelWidth, tempEnableHierarchyTree);
            tempEnableHierarchyRows = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Hierarchy Rows", toggleLabelWidth, tempEnableHierarchyRows);
            tempEnableHierarchyButtons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Hierarchy Buttons", toggleLabelWidth, tempEnableHierarchyButtons);
            tempEnableMajorShortcuts = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Major Shortcuts", toggleLabelWidth, tempEnableMajorShortcuts);
            tempDisableHierarchyDesignerDuringPlayMode = HierarchyDesigner_Shared_GUI.DrawToggle("Disable Hierarchy Designer During PlayMode", toggleLabelWidth, tempDisableHierarchyDesignerDuringPlayMode);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Filtering Features
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Filtering Features", contentGUIStyle);
            GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            tempExcludeFolderProperties = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Folder Properties", toggleLabelWidth, tempExcludeFolderProperties);
            tempExcludeTransformForGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Transform Component", toggleLabelWidth, tempExcludeTransformForGameObjectComponentIcons);
            tempExcludeCanvasRendererForGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Canvas Renderer Component", toggleLabelWidth, tempExcludeCanvasRendererForGameObjectComponentIcons);

            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            GUILayout.Space(4);

            #region Tag
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            int tagMask = GetMaskFromList(tempExcludedTags, tags);
            EditorGUI.BeginChangeCheck();
            tagMask = HierarchyDesigner_Shared_GUI.DrawMaskField("Excluded Tags", maskFieldLabelWidth, tagMask, tags);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            tempExcludedTags = GetListFromMask(tagMask, tags);
            #endregion

            #region Layer
            string[] layers = UnityEditorInternal.InternalEditorUtility.layers;
            int layerMask = GetMaskFromList(tempExcludedLayers, layers);
            layerMask = HierarchyDesigner_Shared_GUI.DrawMaskField("Excluded Layers", maskFieldLabelWidth, layerMask, layers);
            EditorGUI.BeginChangeCheck();
            tempExcludedLayers = GetListFromMask(layerMask, layers);
            if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            #endregion
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
                bool shouldSave = EditorUtility.DisplayDialog("General Settings Have Been Modified!",
                    "Do you want to save the changes you made to the general settings?",
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
            HierarchyDesigner_Configurable_GeneralSettings.LayoutMode = tempLayoutMode;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectMainIcon = tempEnableGameObjectMainIcon;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectComponentIcons = tempEnableGameObjectComponentIcons;
            HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyTree = tempEnableHierarchyTree;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectTag = tempEnableGameObjectTag;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectLayer = tempEnableGameObjectLayer;
            HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyRows = tempEnableHierarchyRows;
            HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyButtons = tempEnableHierarchyButtons;
            HierarchyDesigner_Configurable_GeneralSettings.EnableMajorShortcuts = tempEnableMajorShortcuts;
            HierarchyDesigner_Configurable_GeneralSettings.DisableHierarchyDesignerDuringPlayMode = tempDisableHierarchyDesignerDuringPlayMode;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludeFolderProperties = tempExcludeFolderProperties;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludeTransformForGameObjectComponentIcons = tempExcludeTransformForGameObjectComponentIcons;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludeCanvasRendererForGameObjectComponentIcons = tempExcludeCanvasRendererForGameObjectComponentIcons;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludedTags = tempExcludedTags;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludedLayers = tempExcludedLayers;
            HierarchyDesigner_Configurable_GeneralSettings.SaveSettings();
            hasModifiedChanges = false;
        }

        private void EnableAllFeatures(bool enable)
        {
            tempEnableGameObjectMainIcon = enable;
            tempEnableGameObjectComponentIcons = enable;
            tempEnableGameObjectTag = enable;
            tempEnableGameObjectLayer = enable;
            tempEnableHierarchyTree = enable;
            tempEnableHierarchyRows = enable;
            tempEnableHierarchyButtons = enable;
            tempEnableMajorShortcuts = enable;
            tempDisableHierarchyDesignerDuringPlayMode = enable;
            tempExcludeFolderProperties = enable;
            tempExcludeTransformForGameObjectComponentIcons = enable;
            tempExcludeCanvasRendererForGameObjectComponentIcons = enable;
            hasModifiedChanges = true;
        }
        #endregion

        #region Operations
        private int GetMaskFromList(List<string> list, string[] allItems)
        {
            int mask = 0;
            for (int i = 0; i < allItems.Length; i++)
            {
                if (list.Contains(allItems[i]))
                {
                    mask |= 1 << i;
                }
            }
            return mask;
        }

        private List<string> GetListFromMask(int mask, string[] allItems)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < allItems.Length; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    list.Add(allItems[i]);
                }
            }
            return list;
        }
        #endregion
    }
}
#endif