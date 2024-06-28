#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_DesignSettings : EditorWindow
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
        private const float labelWidth = 170;
        #endregion
        #region Temporary Values
        private float tempComponentIconsSize;
        private float tempComponentIconsSpacing;
        private int tempComponentIconsOffset;
        private Color tempHierarchyTreeColor;
        private Color tempTagColor;
        private TextAnchor tempTagTextAnchor;
        private FontStyle tempTagFontStyle;
        private int tempTagFontSize;
        private Color tempLayerColor;
        private TextAnchor tempLayerTextAnchor;
        private FontStyle tempLayerFontStyle;
        private int tempLayerFontSize;
        private int tempTagLayerOffset;
        private int tempTagLayerSpacing;
        private Color tempLockColor;
        private TextAnchor tempLockTextAnchor;
        private FontStyle tempLockFontStyle;
        private int tempLockFontSize;
        private static bool hasModifiedChanges = false;
        #endregion
        #endregion

        #region Window
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Configurations + "/Design Settings", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_DesignSettings editorWindow = GetWindow<HierarchyDesigner_Window_DesignSettings>("Design Settings");
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
            tempComponentIconsSize = HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSize;
            tempComponentIconsSpacing = HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSpacing;
            tempComponentIconsOffset = HierarchyDesigner_Configurable_DesignSettings.ComponentIconsOffset;
            tempHierarchyTreeColor = HierarchyDesigner_Configurable_DesignSettings.HierarchyTreeColor;
            tempTagColor = HierarchyDesigner_Configurable_DesignSettings.TagColor;
            tempTagTextAnchor = HierarchyDesigner_Configurable_DesignSettings.TagTextAnchor;
            tempTagFontStyle = HierarchyDesigner_Configurable_DesignSettings.TagFontStyle;
            tempTagFontSize = HierarchyDesigner_Configurable_DesignSettings.TagFontSize;
            tempLayerColor = HierarchyDesigner_Configurable_DesignSettings.LayerColor;
            tempLayerTextAnchor = HierarchyDesigner_Configurable_DesignSettings.LayerTextAnchor;
            tempLayerFontStyle = HierarchyDesigner_Configurable_DesignSettings.LayerFontStyle;
            tempLayerFontSize = HierarchyDesigner_Configurable_DesignSettings.LayerFontSize;
            tempTagLayerOffset = HierarchyDesigner_Configurable_DesignSettings.TagLayerOffset;
            tempTagLayerSpacing = HierarchyDesigner_Configurable_DesignSettings.TagLayerSpacing;
            tempLockColor = HierarchyDesigner_Configurable_DesignSettings.LockColor;
            tempLockTextAnchor = HierarchyDesigner_Configurable_DesignSettings.LockTextAnchor;
            tempLockFontStyle = HierarchyDesigner_Configurable_DesignSettings.LockFontStyle;
            tempLockFontSize = HierarchyDesigner_Configurable_DesignSettings.LockFontSize;
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out GUIStyle _, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Design Settings", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region Component Icons
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("GameObject's Component Icons", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(labelWidth))
            {
                EditorGUI.BeginChangeCheck();
                tempComponentIconsSize = EditorGUILayout.Slider("Component Icons Size", tempComponentIconsSize, 0.5f, 1.0f);
                tempComponentIconsSpacing = EditorGUILayout.Slider("Component Icons Spacing", tempComponentIconsSpacing, 0.0f, 10.0f);
                tempComponentIconsOffset = EditorGUILayout.IntSlider("Component Icons Offset", tempComponentIconsOffset, 15, 30);
                if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Tag
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("GameObject's Tag", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(labelWidth))
            {
                EditorGUI.BeginChangeCheck();
                tempTagColor = EditorGUILayout.ColorField("Tag Color", tempTagColor);
                tempTagFontSize = EditorGUILayout.IntSlider("Tag Font Size", tempTagFontSize, 7, 21);
                tempTagFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Tag Font Style", tempTagFontStyle);
                tempTagTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Tag Text Anchor", tempTagTextAnchor);
                if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Layer
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("GameObject's Layer", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(labelWidth))
            {
                EditorGUI.BeginChangeCheck();
                tempLayerColor = EditorGUILayout.ColorField("Layer Color", tempLayerColor);
                tempLayerFontSize = EditorGUILayout.IntSlider("Layer Font Size", tempLayerFontSize, 7, 21);
                tempLayerFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Layer Font Style", tempLayerFontStyle);
                tempLayerTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Layer Text Anchor", tempLayerTextAnchor);
                if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Tag and Layer
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("GameObject's Tag, Layer", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(labelWidth))
            {
                EditorGUI.BeginChangeCheck();
                tempTagLayerOffset = EditorGUILayout.IntSlider("Tag, Layer Offset", tempTagLayerOffset, 0, 20);
                tempTagLayerSpacing = EditorGUILayout.IntSlider("Tag, Layer Spacing", tempTagLayerSpacing, 0, 20);
                if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Hierarchy Tree
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Hierarchy Tree", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(labelWidth))
            {
                EditorGUI.BeginChangeCheck();
                tempHierarchyTreeColor = EditorGUILayout.ColorField("Tree Color", tempHierarchyTreeColor);
                if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Lock
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Lock Label", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(labelWidth))
            {
                EditorGUI.BeginChangeCheck();
                tempLockColor = EditorGUILayout.ColorField("Lock Color", tempLockColor);
                tempLockFontSize = EditorGUILayout.IntSlider("Lock Font Size", tempLockFontSize, 7, 21);
                tempLockFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Lock Font Style", tempLockFontStyle);
                tempLockTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Lock Text Anchor", tempLockTextAnchor);
                if (EditorGUI.EndChangeCheck()) { hasModifiedChanges = true; }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion
            #endregion

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            #region Footer
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
                bool shouldSave = EditorUtility.DisplayDialog("Design Settings Have Been Modified!",
                    "Do you want to save the changes you made to the design settings?",
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
            HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSize = tempComponentIconsSize;
            HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSpacing = tempComponentIconsSpacing;
            HierarchyDesigner_Configurable_DesignSettings.ComponentIconsOffset = tempComponentIconsOffset;
            HierarchyDesigner_Configurable_DesignSettings.HierarchyTreeColor = tempHierarchyTreeColor;
            HierarchyDesigner_Configurable_DesignSettings.TagColor = tempTagColor;
            HierarchyDesigner_Configurable_DesignSettings.TagTextAnchor = tempTagTextAnchor;
            HierarchyDesigner_Configurable_DesignSettings.TagFontStyle = tempTagFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.TagFontSize = tempTagFontSize;
            HierarchyDesigner_Configurable_DesignSettings.LayerColor = tempLayerColor;
            HierarchyDesigner_Configurable_DesignSettings.LayerTextAnchor = tempLayerTextAnchor;
            HierarchyDesigner_Configurable_DesignSettings.LayerFontStyle = tempLayerFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.LayerFontSize = tempLayerFontSize;
            HierarchyDesigner_Configurable_DesignSettings.TagLayerOffset = tempTagLayerOffset;
            HierarchyDesigner_Configurable_DesignSettings.TagLayerSpacing = tempTagLayerSpacing;
            HierarchyDesigner_Configurable_DesignSettings.LockColor = tempLockColor;
            HierarchyDesigner_Configurable_DesignSettings.LockTextAnchor = tempLockTextAnchor;
            HierarchyDesigner_Configurable_DesignSettings.LockFontStyle = tempLockFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.LockFontSize = tempLockFontSize;
            HierarchyDesigner_Configurable_DesignSettings.SaveSettings();
            hasModifiedChanges = false;
        }
        #endregion
    }
}
#endif