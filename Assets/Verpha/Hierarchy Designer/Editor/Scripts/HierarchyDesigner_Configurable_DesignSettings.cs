#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Configurable_DesignSettings
    {
        #region Properties
        [System.Serializable]
        private class HierarchyDesigner_DesignSettings
        {
            public float ComponentIconsSize = 1f;
            public float ComponentIconsSpacing = 2f;
            public int ComponentIconsOffset = 20;
            public Color HierarchyTreeColor = Color.white;
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
            public Color LockColor = Color.white;
            public TextAnchor LockTextAnchor = TextAnchor.MiddleCenter;
            public FontStyle LockFontStyle = FontStyle.BoldAndItalic;
            public int LockFontSize = 11;
        }
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
            HierarchyDesigner_Manager_GameObject.ComponentIconsSpacingCache = ComponentIconsSpacing;
            HierarchyDesigner_Manager_GameObject.ComponentIconsOffsetCache = ComponentIconsOffset;
            HierarchyDesigner_Manager_GameObject.HierarchyTreeColorCache = HierarchyTreeColor;
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
            HierarchyDesigner_Manager_GameObject.LockColorCache = LockColor;
            HierarchyDesigner_Manager_GameObject.LockTextAnchorCache = LockTextAnchor;
            HierarchyDesigner_Manager_GameObject.LockFontStyleCache = LockFontStyle;
            HierarchyDesigner_Manager_GameObject.LockFontSizeCache = LockFontSize;
        }
        #endregion

        #region Accessors
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
                ComponentIconsSpacing = 2f,
                ComponentIconsOffset = 20,
                HierarchyTreeColor = Color.white,
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