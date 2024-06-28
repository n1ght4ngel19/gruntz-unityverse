#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Configurable_GeneralSettings
    {
        #region Properties
        [System.Serializable]
        private class HierarchyDesigner_GeneralSettings
        {
            public HierarchyLayoutMode LayoutMode = HierarchyLayoutMode.Docked;
            public bool EnableGameObjectMainIcon = true;
            public bool EnableGameObjectComponentIcons = true;
            public bool EnableHierarchyTree = true;
            public bool EnableGameObjectTag = true;
            public bool EnableGameObjectLayer = true;
            public bool EnableHierarchyRows = true;
            public bool EnableHierarchyButtons = true;
            public bool EnableMajorShortcuts = true;
            public bool DisableHierarchyDesignerDuringPlayMode = true;
            public bool ExcludeFolderProperties = true;
            public bool ExcludeTransformForGameObjectComponentIcons = true;
            public bool ExcludeCanvasRendererForGameObjectComponentIcons = true;
            public List<string> ExcludedTags = new List<string>();
            public List<string> ExcludedLayers = new List<string>();
        }
        public enum HierarchyLayoutMode { Consecutive, Docked, Split };
        private static HierarchyDesigner_GeneralSettings generalSettings = new HierarchyDesigner_GeneralSettings();
        private const string settingsFileName = "HierarchyDesigner_SavedData_GeneralSettings.json";
        #endregion

        #region Initialization
        public static void Initialize()
        {
            LoadSettings();
            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            HierarchyDesigner_Manager_GameObject.LayoutModeCache = LayoutMode;
            HierarchyDesigner_Manager_GameObject.EnableGameObjectMainIconCache = EnableGameObjectMainIcon;
            HierarchyDesigner_Manager_GameObject.EnableGameObjectComponentIconsCache = EnableGameObjectComponentIcons;
            HierarchyDesigner_Manager_GameObject.EnableHierarchyTreeCache = EnableHierarchyTree;
            HierarchyDesigner_Manager_GameObject.EnableGameObjectTagCache = EnableGameObjectTag;
            HierarchyDesigner_Manager_GameObject.EnableGameObjectLayerCache = EnableGameObjectLayer;
            HierarchyDesigner_Manager_GameObject.EnableHierarchyRowsCache = EnableHierarchyRows;
            HierarchyDesigner_Manager_GameObject.EnableHierarchyButtonsCache = EnableHierarchyButtons;
            HierarchyDesigner_Manager_GameObject.EnableMajorShortcutsCache = EnableMajorShortcuts;
            HierarchyDesigner_Manager_GameObject.DisableHierarchyDesignerDuringPlayModeCache = DisableHierarchyDesignerDuringPlayMode;
            HierarchyDesigner_Manager_GameObject.ExcludeFolderProperties = ExcludeFolderProperties;
            HierarchyDesigner_Manager_GameObject.ExcludeTransformComponentCache = ExcludeTransformForGameObjectComponentIcons;
            HierarchyDesigner_Manager_GameObject.ExcludeCanvasRendererComponentCache = ExcludeCanvasRendererForGameObjectComponentIcons;
            HierarchyDesigner_Manager_GameObject.ExcludedTagsCache = ExcludedTags;
            HierarchyDesigner_Manager_GameObject.ExcludedLayersCache = ExcludedLayers;
        }
        #endregion

        #region Accessors
        public static HierarchyLayoutMode LayoutMode
        {
            get => generalSettings.LayoutMode;
            set
            {
                if (generalSettings.LayoutMode != value)
                {
                    generalSettings.LayoutMode = value;
                    HierarchyDesigner_Manager_GameObject.LayoutModeCache = value;
                }
            }
        }

        public static bool EnableGameObjectMainIcon
        {
            get => generalSettings.EnableGameObjectMainIcon;
            set
            {
                if (generalSettings.EnableGameObjectMainIcon != value)
                {
                    generalSettings.EnableGameObjectMainIcon = value;
                    HierarchyDesigner_Manager_GameObject.EnableGameObjectMainIconCache = value;
                }
            }
        }

        public static bool EnableGameObjectComponentIcons
        {
            get => generalSettings.EnableGameObjectComponentIcons;
            set
            {
                if (generalSettings.EnableGameObjectComponentIcons != value)
                {
                    generalSettings.EnableGameObjectComponentIcons = value;
                    HierarchyDesigner_Manager_GameObject.EnableGameObjectComponentIconsCache = value;
                }
            }
        }

        public static bool EnableHierarchyTree
        {
            get => generalSettings.EnableHierarchyTree;
            set
            {
                if (generalSettings.EnableHierarchyTree != value)
                {
                    generalSettings.EnableHierarchyTree = value;
                    HierarchyDesigner_Manager_GameObject.EnableHierarchyTreeCache = value;
                }
            }
        }

        public static bool EnableGameObjectTag
        {
            get => generalSettings.EnableGameObjectTag;
            set
            {
                if (generalSettings.EnableGameObjectTag != value)
                {
                    generalSettings.EnableGameObjectTag = value;
                    HierarchyDesigner_Manager_GameObject.EnableGameObjectTagCache = value;
                }
            }
        }

        public static bool EnableGameObjectLayer
        {
            get => generalSettings.EnableGameObjectLayer;
            set
            {
                if (generalSettings.EnableGameObjectLayer != value)
                {
                    generalSettings.EnableGameObjectLayer = value;
                    HierarchyDesigner_Manager_GameObject.EnableGameObjectLayerCache = value;
                }
            }
        }

        public static bool EnableHierarchyRows
        {
            get => generalSettings.EnableHierarchyRows;
            set
            {
                if (generalSettings.EnableHierarchyRows != value)
                {
                    generalSettings.EnableHierarchyRows = value;
                    HierarchyDesigner_Manager_GameObject.EnableHierarchyRowsCache = value;
                }
            }
        }

        public static bool EnableHierarchyButtons
        {
            get => generalSettings.EnableHierarchyButtons;
            set
            {
                if (generalSettings.EnableHierarchyButtons != value)
                {
                    generalSettings.EnableHierarchyButtons = value;
                    HierarchyDesigner_Manager_GameObject.EnableHierarchyButtonsCache = value;
                }
            }
        }

        public static bool EnableMajorShortcuts
        {
            get => generalSettings.EnableMajorShortcuts;
            set
            {
                if (generalSettings.EnableMajorShortcuts != value)
                {
                    generalSettings.EnableMajorShortcuts = value;
                    HierarchyDesigner_Manager_GameObject.EnableMajorShortcutsCache = value;
                }
            }
        }

        public static bool DisableHierarchyDesignerDuringPlayMode
        {
            get => generalSettings.DisableHierarchyDesignerDuringPlayMode;
            set
            {
                if (generalSettings.DisableHierarchyDesignerDuringPlayMode != value)
                {
                    generalSettings.DisableHierarchyDesignerDuringPlayMode = value;
                    HierarchyDesigner_Manager_GameObject.DisableHierarchyDesignerDuringPlayModeCache = value;
                }
            }
        }

        public static bool ExcludeFolderProperties
        {
            get => generalSettings.ExcludeFolderProperties;
            set
            {
                if (generalSettings.ExcludeFolderProperties != value)
                {
                    generalSettings.ExcludeFolderProperties = value;
                    HierarchyDesigner_Manager_GameObject.ExcludeFolderProperties = value;
                }
            }
        }

        public static bool ExcludeTransformForGameObjectComponentIcons
        {
            get => generalSettings.ExcludeTransformForGameObjectComponentIcons;
            set
            {
                if (generalSettings.ExcludeTransformForGameObjectComponentIcons != value)
                {
                    generalSettings.ExcludeTransformForGameObjectComponentIcons = value;
                    HierarchyDesigner_Manager_GameObject.ExcludeTransformComponentCache = value;
                    HierarchyDesigner_Manager_GameObject.ClearComponentIconsCache();
                }
            }
        }

        public static bool ExcludeCanvasRendererForGameObjectComponentIcons
        {
            get => generalSettings.ExcludeCanvasRendererForGameObjectComponentIcons;
            set
            {
                if (generalSettings.ExcludeCanvasRendererForGameObjectComponentIcons != value)
                {
                    generalSettings.ExcludeCanvasRendererForGameObjectComponentIcons = value;
                    HierarchyDesigner_Manager_GameObject.ExcludeCanvasRendererComponentCache = value;
                    HierarchyDesigner_Manager_GameObject.ClearComponentIconsCache();
                }
            }
        }

        public static List<string> ExcludedTags
        {
            get => generalSettings.ExcludedTags;
            set
            {
                if (generalSettings.ExcludedTags != value)
                {
                    generalSettings.ExcludedTags = value;
                    HierarchyDesigner_Manager_GameObject.ExcludedTagsCache = value;
                }
            }
        }

        public static List<string> ExcludedLayers
        {
            get => generalSettings.ExcludedLayers;
            set
            {
                if (generalSettings.ExcludedLayers != value)
                {
                    generalSettings.ExcludedLayers = value;
                    HierarchyDesigner_Manager_GameObject.ExcludedLayersCache = value;
                }
            }
        }
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            string json = JsonUtility.ToJson(generalSettings, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_GeneralSettings loadedSettings = JsonUtility.FromJson<HierarchyDesigner_GeneralSettings>(json);
                loadedSettings.LayoutMode = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.LayoutMode.ToString(), HierarchyLayoutMode.Docked);
                generalSettings = loadedSettings;
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            generalSettings = new HierarchyDesigner_GeneralSettings()
            {
                LayoutMode = HierarchyLayoutMode.Docked,
                EnableGameObjectMainIcon = true,
                EnableGameObjectComponentIcons = true,
                EnableHierarchyTree = true,
                EnableGameObjectTag = true,
                EnableGameObjectLayer = true,
                EnableHierarchyRows = true,
                EnableHierarchyButtons = true,
                EnableMajorShortcuts = true,
                DisableHierarchyDesignerDuringPlayMode = true,
                ExcludeFolderProperties = true,
                ExcludeTransformForGameObjectComponentIcons = true,
                ExcludeCanvasRendererForGameObjectComponentIcons = true,
                ExcludedTags = new List<string>(),
                ExcludedLayers = new List<string>()
            };
        }
        #endregion
    }
}
#endif