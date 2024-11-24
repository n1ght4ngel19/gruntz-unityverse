#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Configurable_DesignSettings
    {
        #region Properties
        [System.Serializable]
        private class HierarchyDesigner_DesignSettings
        {
            #region Component Icons
            public float ComponentIconsSize = 1f;
            public int ComponentIconsOffset = 21;
            public float ComponentIconsSpacing = 2f;
            #endregion
            #region Hierarchy Tree
            public Color HierarchyTreeColor = Color.white;
            public TreeBranchImageType TreeBranchImageType_I = TreeBranchImageType.Default;
            public TreeBranchImageType TreeBranchImageType_L = TreeBranchImageType.Default;
            public TreeBranchImageType TreeBranchImageType_T = TreeBranchImageType.Default;
            public TreeBranchImageType TreeBranchImageType_TerminalBud = TreeBranchImageType.Default;
            #endregion
            #region Tag and Layer
            public Color TagColor = Color.gray;
            public TextAnchor TagTextAnchor = TextAnchor.MiddleRight;
            public FontStyle TagFontStyle = FontStyle.BoldAndItalic;
            public int TagFontSize = 10;
            public Color LayerColor = Color.gray;
            public TextAnchor LayerTextAnchor = TextAnchor.MiddleLeft;
            public FontStyle LayerFontStyle = FontStyle.BoldAndItalic;
            public int LayerFontSize = 10;
            public int TagLayerOffset = 5;
            public int TagLayerSpacing = 5;
            #endregion
            #region Hierarchy Line
            public Color HierarchyLineColor = HierarchyDesigner_Shared_ColorUtility.HexToColor("00000080");
            public int HierarchyLineThickness = 1;
            #endregion
            #region Folder
            public Color FolderDefaultTextColor = Color.white;
            public int FolderDefaultFontSize = 12;
            public FontStyle FolderDefaultFontStyle = FontStyle.Normal;
            public Color FolderDefaultImageColor = Color.white;
            public HierarchyDesigner_Configurable_Folder.FolderImageType FolderDefaultImageType = HierarchyDesigner_Configurable_Folder.FolderImageType.Default;
            #endregion
            #region Separator
            public Color SeparatorDefaultTextColor = Color.white;
            public bool SeparatorDefaultIsGradientBackground = false;
            public Color SeparatorDefaultBackgroundColor = Color.gray;
            public Gradient SeparatorDefaultBackgroundGradient = new Gradient();
            public int SeparatorDefaultFontSize = 12;
            public FontStyle SeparatorDefaultFontStyle = FontStyle.Normal;
            public TextAnchor SeparatorDefaultTextAnchor = TextAnchor.MiddleCenter;
            public HierarchyDesigner_Configurable_Separator.SeparatorImageType SeparatorDefaultImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
            public int SeparatorLeftSideTextAnchorOffset = 3;
            public int SeparatorRightSideTextAnchorOffset = 36;
            #endregion
            #region Lock Label
            public Color LockColor = Color.white;
            public TextAnchor LockTextAnchor = TextAnchor.MiddleCenter;
            public FontStyle LockFontStyle = FontStyle.BoldAndItalic;
            public int LockFontSize = 11;
            #endregion
        }
        public enum TreeBranchImageType { Default, Curved, Dotted, Segmented }
        private static HierarchyDesigner_DesignSettings designSettings = new HierarchyDesigner_DesignSettings();
        private const string settingsFileName = "HierarchyDesigner_SavedData_DesignSettings.json";
        #endregion

        #region Initialization
        public static void Initialize()
        {
            LoadSettings();
            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            HierarchyDesigner_Manager_GameObject.ComponentIconsSizeCache = ComponentIconsSize;
            HierarchyDesigner_Manager_GameObject.ComponentIconsOffsetCache = ComponentIconsOffset;
            HierarchyDesigner_Manager_GameObject.ComponentIconsSpacingCache = ComponentIconsSpacing;
            HierarchyDesigner_Manager_GameObject.HierarchyTreeColorCache = HierarchyTreeColor;
            HierarchyDesigner_Manager_GameObject.TreeBranchImageType_ICache = TreeBranchImageType_I;
            HierarchyDesigner_Manager_GameObject.TreeBranchImageType_LCache = TreeBranchImageType_L;
            HierarchyDesigner_Manager_GameObject.TreeBranchImageType_TCache = TreeBranchImageType_T;
            HierarchyDesigner_Manager_GameObject.TreeBranchImageType_TerminalBudCache = TreeBranchImageType_TerminalBud;
            HierarchyDesigner_Manager_GameObject.TagColorCache = TagColor;
            HierarchyDesigner_Manager_GameObject.TagTextAnchorCache = TagTextAnchor;
            HierarchyDesigner_Manager_GameObject.TagFontStyleCache = TagFontStyle;
            HierarchyDesigner_Manager_GameObject.TagFontSizeCache = TagFontSize;
            HierarchyDesigner_Manager_GameObject.LayerColorCache = LayerColor;
            HierarchyDesigner_Manager_GameObject.LayerTextAnchorCache = LayerTextAnchor;
            HierarchyDesigner_Manager_GameObject.LayerFontStyleCache = LayerFontStyle;
            HierarchyDesigner_Manager_GameObject.LayerFontSizeCache = LayerFontSize;
            HierarchyDesigner_Manager_GameObject.TagLayerOffsetCache = TagLayerOffset;
            HierarchyDesigner_Manager_GameObject.TagLayerSpacingCache = TagLayerSpacing;
            HierarchyDesigner_Manager_GameObject.HierarchyLineColorCache = HierarchyLineColor;
            HierarchyDesigner_Manager_GameObject.HierarchyLineThicknessCache = HierarchyLineThickness;
            HierarchyDesigner_Manager_GameObject.SeparatorLeftSideTextAnchorOffsetCache = SeparatorLeftSideTextAnchorOffset;
            HierarchyDesigner_Manager_GameObject.SeparatorRightSideTextAnchorOffsetCache = SeparatorRightSideTextAnchorOffset;
            HierarchyDesigner_Manager_GameObject.LockColorCache = LockColor;
            HierarchyDesigner_Manager_GameObject.LockTextAnchorCache = LockTextAnchor;
            HierarchyDesigner_Manager_GameObject.LockFontStyleCache = LockFontStyle;
            HierarchyDesigner_Manager_GameObject.LockFontSizeCache = LockFontSize;
        }
        #endregion

        #region Accessors
        #region Component Icons
        public static float ComponentIconsSize
        {
            get => designSettings.ComponentIconsSize;
            set
            {
                float clampedValue = Mathf.Clamp(value, 0.5f, 1.0f);
                if (designSettings.ComponentIconsSize != clampedValue)
                {
                    designSettings.ComponentIconsSize = clampedValue;
                    HierarchyDesigner_Manager_GameObject.ComponentIconsSizeCache = clampedValue;
                }
            }
        }

        public static int ComponentIconsOffset
        {
            get => designSettings.ComponentIconsOffset;
            set
            {
                int clampedValue = Mathf.Clamp(value, 15, 30);
                if (designSettings.ComponentIconsOffset != clampedValue)
                {
                    designSettings.ComponentIconsOffset = clampedValue;
                    HierarchyDesigner_Manager_GameObject.ComponentIconsOffsetCache = clampedValue;
                }
            }
        }

        public static float ComponentIconsSpacing
        {
            get => designSettings.ComponentIconsSpacing;
            set
            {
                float clampedValue = Mathf.Clamp(value, 0.0f, 10.0f);
                if (designSettings.ComponentIconsSpacing != clampedValue)
                {
                    designSettings.ComponentIconsSpacing = clampedValue;
                    HierarchyDesigner_Manager_GameObject.ComponentIconsSpacingCache = clampedValue;
                }
            }
        }
        #endregion

        #region Hierarchy Tree
        public static Color HierarchyTreeColor
        {
            get => designSettings.HierarchyTreeColor;
            set
            {
                if (designSettings.HierarchyTreeColor != value)
                {
                    designSettings.HierarchyTreeColor = value;
                    HierarchyDesigner_Manager_GameObject.HierarchyTreeColorCache = value;
                }
            }
        }

        public static TreeBranchImageType TreeBranchImageType_I
        {
            get => designSettings.TreeBranchImageType_I;
            set
            {
                if (designSettings.TreeBranchImageType_I != value)
                {
                    designSettings.TreeBranchImageType_I = value;
                    HierarchyDesigner_Manager_GameObject.TreeBranchImageType_ICache = value;
                }
            }
        }

        public static TreeBranchImageType TreeBranchImageType_L
        {
            get => designSettings.TreeBranchImageType_L;
            set
            {
                if (designSettings.TreeBranchImageType_L != value)
                {
                    designSettings.TreeBranchImageType_L = value;
                    HierarchyDesigner_Manager_GameObject.TreeBranchImageType_LCache = value;
                }
            }
        }

        public static TreeBranchImageType TreeBranchImageType_T
        {
            get => designSettings.TreeBranchImageType_T;
            set
            {
                if (designSettings.TreeBranchImageType_T != value)
                {
                    designSettings.TreeBranchImageType_T = value;
                    HierarchyDesigner_Manager_GameObject.TreeBranchImageType_TCache = value;
                }
            }
        }

        public static TreeBranchImageType TreeBranchImageType_TerminalBud
        {
            get => designSettings.TreeBranchImageType_TerminalBud;
            set
            {
                if (designSettings.TreeBranchImageType_TerminalBud != value)
                {
                    designSettings.TreeBranchImageType_TerminalBud = value;
                    HierarchyDesigner_Manager_GameObject.TreeBranchImageType_TerminalBudCache = value;
                }
            }
        }
        #endregion

        #region Tag and Layer
        public static Color TagColor
        {
            get => designSettings.TagColor;
            set
            {
                if (designSettings.TagColor != value)
                {
                    designSettings.TagColor = value;
                    HierarchyDesigner_Manager_GameObject.TagColorCache = value;
                }
            }
        }

        public static TextAnchor TagTextAnchor
        {
            get => designSettings.TagTextAnchor;
            set
            {
                if (designSettings.TagTextAnchor != value)
                {
                    designSettings.TagTextAnchor = value;
                    HierarchyDesigner_Manager_GameObject.TagTextAnchorCache = value;
                }
            }
        }

        public static FontStyle TagFontStyle
        {
            get => designSettings.TagFontStyle;
            set
            {
                if (designSettings.TagFontStyle != value)
                {
                    designSettings.TagFontStyle = value;
                    HierarchyDesigner_Manager_GameObject.TagFontStyleCache = value;
                }
            }
        }

        public static int TagFontSize
        {
            get => designSettings.TagFontSize;
            set
            {
                int clampedValue = Mathf.Clamp(value, 7, 21);
                if (designSettings.TagFontSize != clampedValue)
                {
                    designSettings.TagFontSize = clampedValue;
                    HierarchyDesigner_Manager_GameObject.TagFontSizeCache = clampedValue;
                }
            }
        }

        public static Color LayerColor
        {
            get => designSettings.LayerColor;
            set
            {
                if (designSettings.LayerColor != value)
                {
                    designSettings.LayerColor = value;
                    HierarchyDesigner_Manager_GameObject.LayerColorCache = value;
                }
            }
        }

        public static TextAnchor LayerTextAnchor
        {
            get => designSettings.LayerTextAnchor;
            set
            {
                if (designSettings.LayerTextAnchor != value)
                {
                    designSettings.LayerTextAnchor = value;
                    HierarchyDesigner_Manager_GameObject.LayerTextAnchorCache = value;
                }
            }
        }

        public static FontStyle LayerFontStyle
        {
            get => designSettings.LayerFontStyle;
            set
            {
                if (designSettings.LayerFontStyle != value)
                {
                    designSettings.LayerFontStyle = value;
                    HierarchyDesigner_Manager_GameObject.LayerFontStyleCache = value;
                }
            }
        }

        public static int LayerFontSize
        {
            get => designSettings.LayerFontSize;
            set
            {
                int clampedValue = Mathf.Clamp(value, 7, 21);
                if (designSettings.LayerFontSize != clampedValue)
                {
                    designSettings.LayerFontSize = clampedValue;
                    HierarchyDesigner_Manager_GameObject.LayerFontSizeCache = clampedValue;
                }
            }
        }

        public static int TagLayerOffset
        {
            get => designSettings.TagLayerOffset;
            set
            {
                int clampedValue = Mathf.Clamp(value, 0, 20);
                if (designSettings.TagLayerOffset != clampedValue)
                {
                    designSettings.TagLayerOffset = clampedValue;
                    HierarchyDesigner_Manager_GameObject.TagLayerOffsetCache = clampedValue;
                }
            }
        }

        public static int TagLayerSpacing
        {
            get => designSettings.TagLayerSpacing;
            set
            {
                int clampedValue = Mathf.Clamp(value, 0, 20);
                if (designSettings.TagLayerSpacing != clampedValue)
                {
                    designSettings.TagLayerSpacing = clampedValue;
                    HierarchyDesigner_Manager_GameObject.TagLayerSpacingCache = clampedValue;
                }
            }
        }
        #endregion

        #region Hierarchy Line
        public static Color HierarchyLineColor
        {
            get => designSettings.HierarchyLineColor;
            set
            {
                if (designSettings.HierarchyLineColor != value)
                {
                    designSettings.HierarchyLineColor = value;
                    HierarchyDesigner_Manager_GameObject.HierarchyLineColorCache = value;
                }
            }
        }

        public static int HierarchyLineThickness
        {
            get => designSettings.HierarchyLineThickness;
            set
            {
                int clampedValue = Mathf.Clamp(value, 1, 3);
                if (designSettings.HierarchyLineThickness != clampedValue)
                {
                    designSettings.HierarchyLineThickness = clampedValue;
                    HierarchyDesigner_Manager_GameObject.HierarchyLineThicknessCache = clampedValue;
                }
            }
        }
        #endregion

        #region Folder
        public static Color FolderDefaultTextColor
        {
            get => designSettings.FolderDefaultTextColor;
            set
            {
                if (designSettings.FolderDefaultTextColor != value)
                {
                    designSettings.FolderDefaultTextColor = value;
                }
            }
        }

        public static int FolderDefaultFontSize
        {
            get => designSettings.FolderDefaultFontSize;
            set
            {
                int clampedValue = Mathf.Clamp(value, 7, 21);
                if (designSettings.FolderDefaultFontSize != clampedValue)
                {
                    designSettings.FolderDefaultFontSize = clampedValue;
                }
            }
        }

        public static FontStyle FolderDefaultFontStyle
        {
            get => designSettings.FolderDefaultFontStyle;
            set
            {
                if (designSettings.FolderDefaultFontStyle != value)
                {
                    designSettings.FolderDefaultFontStyle = value;
                }
            }
        }

        public static Color FolderDefaultImageColor
        {
            get => designSettings.FolderDefaultImageColor;
            set
            {
                if (designSettings.FolderDefaultImageColor != value)
                {
                    designSettings.FolderDefaultImageColor = value;
                }
            }
        }

        public static HierarchyDesigner_Configurable_Folder.FolderImageType FolderDefaultImageType
        {
            get => designSettings.FolderDefaultImageType;
            set
            {
                if (designSettings.FolderDefaultImageType != value)
                {
                    designSettings.FolderDefaultImageType = value;
                }
            }
        }
        #endregion

        #region Separator
        public static Color SeparatorDefaultTextColor
        {
            get => designSettings.SeparatorDefaultTextColor;
            set
            {
                if (designSettings.SeparatorDefaultTextColor != value)
                {
                    designSettings.SeparatorDefaultTextColor = value;
                }
            }
        }

        public static bool SeparatorDefaultIsGradientBackground
        {
            get => designSettings.SeparatorDefaultIsGradientBackground;
            set
            {
                if (designSettings.SeparatorDefaultIsGradientBackground != value)
                {
                    designSettings.SeparatorDefaultIsGradientBackground = value;
                }
            }
        }

        public static Color SeparatorDefaultBackgroundColor
        {
            get => designSettings.SeparatorDefaultBackgroundColor;
            set
            {
                if (designSettings.SeparatorDefaultBackgroundColor != value)
                {
                    designSettings.SeparatorDefaultBackgroundColor = value;
                }
            }
        }

        public static Gradient SeparatorDefaultBackgroundGradient
        {
            get => designSettings.SeparatorDefaultBackgroundGradient;
            set
            {
                if (designSettings.SeparatorDefaultBackgroundGradient != value)
                {
                    designSettings.SeparatorDefaultBackgroundGradient = value;
                }
            }
        }

        public static int SeparatorDefaultFontSize
        {
            get => designSettings.SeparatorDefaultFontSize;
            set
            {
                int clampedValue = Mathf.Clamp(value, 7, 21);
                if (designSettings.SeparatorDefaultFontSize != clampedValue)
                {
                    designSettings.SeparatorDefaultFontSize = clampedValue;
                }
            }
        }

        public static FontStyle SeparatorDefaultFontStyle
        {
            get => designSettings.SeparatorDefaultFontStyle;
            set
            {
                if (designSettings.SeparatorDefaultFontStyle != value)
                {
                    designSettings.SeparatorDefaultFontStyle = value;
                }
            }
        }

        public static TextAnchor SeparatorDefaultTextAnchor
        {
            get => designSettings.SeparatorDefaultTextAnchor;
            set
            {
                if (designSettings.SeparatorDefaultTextAnchor != value)
                {
                    designSettings.SeparatorDefaultTextAnchor = value;
                }
            }
        }

        public static HierarchyDesigner_Configurable_Separator.SeparatorImageType SeparatorDefaultImageType
        {
            get => designSettings.SeparatorDefaultImageType;
            set
            {
                if (designSettings.SeparatorDefaultImageType != value)
                {
                    designSettings.SeparatorDefaultImageType = value;
                }
            }
        }

        public static int SeparatorLeftSideTextAnchorOffset
        {
            get => designSettings.SeparatorLeftSideTextAnchorOffset;
            set
            {
                int clampedValue = Mathf.Clamp(value, 0, 33);
                if (designSettings.SeparatorLeftSideTextAnchorOffset != clampedValue)
                {
                    designSettings.SeparatorLeftSideTextAnchorOffset = clampedValue;
                    HierarchyDesigner_Manager_GameObject.SeparatorLeftSideTextAnchorOffsetCache = clampedValue;
                }
            }
        }

        public static int SeparatorRightSideTextAnchorOffset
        {
            get => designSettings.SeparatorRightSideTextAnchorOffset;
            set
            {
                int clampedValue = Mathf.Clamp(value, 33, 66);
                if (designSettings.SeparatorRightSideTextAnchorOffset != clampedValue)
                {
                    designSettings.SeparatorRightSideTextAnchorOffset = clampedValue;
                    HierarchyDesigner_Manager_GameObject.SeparatorRightSideTextAnchorOffsetCache = clampedValue;
                }
            }
        }
        #endregion

        #region Lock Label
        public static Color LockColor
        {
            get => designSettings.LockColor;
            set
            {
                if (designSettings.LockColor != value)
                {
                    designSettings.LockColor = value;
                    HierarchyDesigner_Manager_GameObject.LockColorCache = value;
                }
            }
        }

        public static TextAnchor LockTextAnchor
        {
            get => designSettings.LockTextAnchor;
            set
            {
                if (designSettings.LockTextAnchor != value)
                {
                    designSettings.LockTextAnchor = value;
                    HierarchyDesigner_Manager_GameObject.LockTextAnchorCache = value;
                }
            }
        }

        public static FontStyle LockFontStyle
        {
            get => designSettings.LockFontStyle;
            set
            {
                if (designSettings.LockFontStyle != value)
                {
                    designSettings.LockFontStyle = value;
                    HierarchyDesigner_Manager_GameObject.LockFontStyleCache = value;
                }
            }
        }

        public static int LockFontSize
        {
            get => designSettings.LockFontSize;
            set
            {
                int clampedValue = Mathf.Clamp(value, 7, 21);
                if (designSettings.LockFontSize != clampedValue)
                {
                    designSettings.LockFontSize = clampedValue;
                    HierarchyDesigner_Manager_GameObject.LockFontSizeCache = clampedValue;
                }
            }
        }
        #endregion
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            string json = JsonUtility.ToJson(designSettings, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_DesignSettings loadedSettings = JsonUtility.FromJson<HierarchyDesigner_DesignSettings>(json);
                designSettings = loadedSettings;
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            designSettings = new HierarchyDesigner_DesignSettings()
            {
                ComponentIconsSize = 1f,
                ComponentIconsOffset = 21,
                ComponentIconsSpacing = 2f,
                HierarchyTreeColor = Color.white,
                TreeBranchImageType_I = TreeBranchImageType.Default,
                TreeBranchImageType_L = TreeBranchImageType.Default,
                TreeBranchImageType_T = TreeBranchImageType.Default,
                TreeBranchImageType_TerminalBud = TreeBranchImageType.Default,
                TagColor = Color.gray,
                TagTextAnchor = TextAnchor.MiddleRight,
                TagFontStyle = FontStyle.BoldAndItalic,
                TagFontSize = 10,
                LayerColor = Color.gray,
                LayerTextAnchor = TextAnchor.MiddleLeft,
                LayerFontStyle = FontStyle.BoldAndItalic,
                LayerFontSize = 10,
                TagLayerOffset = 5,
                TagLayerSpacing = 5,
                HierarchyLineColor = HierarchyDesigner_Shared_ColorUtility.HexToColor("00000080"),
                HierarchyLineThickness = 1,
                FolderDefaultTextColor = Color.white,
                FolderDefaultFontSize = 12,
                FolderDefaultFontStyle = FontStyle.Normal,
                FolderDefaultImageColor = Color.white,
                FolderDefaultImageType = HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                SeparatorDefaultTextColor = Color.white,
                SeparatorDefaultIsGradientBackground = false,
                SeparatorDefaultBackgroundColor = Color.gray,
                SeparatorDefaultBackgroundGradient = new Gradient(),
                SeparatorDefaultFontSize = 12,
                SeparatorDefaultFontStyle = FontStyle.Normal,
                SeparatorDefaultTextAnchor = TextAnchor.MiddleCenter,
                SeparatorDefaultImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default,
                SeparatorLeftSideTextAnchorOffset = 3,
                SeparatorRightSideTextAnchorOffset = 36,
                LockColor = Color.white,
                LockTextAnchor = TextAnchor.MiddleCenter,
                LockFontStyle = FontStyle.BoldAndItalic,
                LockFontSize = 11
            };
        }
        #endregion
    }
}
#endif