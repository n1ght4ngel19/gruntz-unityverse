#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Configurable_ShortcutsSettings
    {
        #region Properties
        [System.Serializable]
        private class HierarchyDesigner_ShortcutsSettings
        {
            public KeyCode ToggleGameObjectActiveStateKeyCode = KeyCode.Mouse2;
            public KeyCode ToggleLockStateKeyCode = KeyCode.F1;
            public KeyCode ChangeTagLayerKeyCode = KeyCode.F2;
            public KeyCode RenameSelectedGameObjectsKeyCode = KeyCode.F3;
        }
        private static HierarchyDesigner_ShortcutsSettings shortcutsSettings = new HierarchyDesigner_ShortcutsSettings();
        private const string settingsFileName = "HierarchyDesigner_SavedData_ShortcutsSettings.json";
        #endregion

        #region Initialization
        public static void Initialize()
        {
            LoadSettings();
            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            HierarchyDesigner_Manager_GameObject.ToggleGameObjectActiveStateKeyCodeCache = ToggleGameObjectActiveStateKeyCode;
            HierarchyDesigner_Manager_GameObject.ToggleLockStateKeyCodeCache = ToggleLockStateKeyCode;
            HierarchyDesigner_Manager_GameObject.ChangeTagLayerKeyCodeCache = ChangeTagLayerKeyCode;
            HierarchyDesigner_Manager_GameObject.RenameSelectedGameObjectsKeyCodeCache = RenameSelectedGameObjectsKeyCode;
        }
        #endregion

        #region Accessors
        public static KeyCode ToggleGameObjectActiveStateKeyCode
        {
            get => shortcutsSettings.ToggleGameObjectActiveStateKeyCode;
            set
            {
                if (shortcutsSettings.ToggleGameObjectActiveStateKeyCode != value)
                {
                    shortcutsSettings.ToggleGameObjectActiveStateKeyCode = value;
                    HierarchyDesigner_Manager_GameObject.ToggleGameObjectActiveStateKeyCodeCache = value;
                }
            }
        }

        public static KeyCode ToggleLockStateKeyCode
        {
            get => shortcutsSettings.ToggleLockStateKeyCode;
            set
            {
                if (shortcutsSettings.ToggleLockStateKeyCode != value)
                {
                    shortcutsSettings.ToggleLockStateKeyCode = value;
                    HierarchyDesigner_Manager_GameObject.ToggleLockStateKeyCodeCache = value;
                }
            }
        }

        public static KeyCode ChangeTagLayerKeyCode
        {
            get => shortcutsSettings.ChangeTagLayerKeyCode;
            set
            {
                if (shortcutsSettings.ChangeTagLayerKeyCode != value)
                {
                    shortcutsSettings.ChangeTagLayerKeyCode = value;
                    HierarchyDesigner_Manager_GameObject.ChangeTagLayerKeyCodeCache = value;
                }
            }
        }

        public static KeyCode RenameSelectedGameObjectsKeyCode
        {
            get => shortcutsSettings.RenameSelectedGameObjectsKeyCode;
            set
            {
                if (shortcutsSettings.RenameSelectedGameObjectsKeyCode != value)
                {
                    shortcutsSettings.RenameSelectedGameObjectsKeyCode = value;
                    HierarchyDesigner_Manager_GameObject.RenameSelectedGameObjectsKeyCodeCache = value;
                }
            }
        }
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            string json = JsonUtility.ToJson(shortcutsSettings, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_ShortcutsSettings loadedSettings = JsonUtility.FromJson<HierarchyDesigner_ShortcutsSettings>(json);
                shortcutsSettings = loadedSettings;
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            shortcutsSettings = new HierarchyDesigner_ShortcutsSettings()
            {
                ToggleGameObjectActiveStateKeyCode = KeyCode.Mouse2,
                ToggleLockStateKeyCode = KeyCode.F1,
                ChangeTagLayerKeyCode = KeyCode.F2,
                RenameSelectedGameObjectsKeyCode = KeyCode.F3,
            };
        }
        #endregion

        #region Minor Shortcuts
        #region Windows
        [Shortcut("Hierarchy Designer/Open Hierarchy Designer Window", KeyCode.Alpha1, ShortcutModifiers.Alt)]
        private static void OpenHierarchyDesignerWindow()
        {
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Folder Panel", KeyCode.Alpha2, ShortcutModifiers.Alt)]
        private static void OpenFolderManagerPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.Folders);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Separator Panel", KeyCode.Alpha3, ShortcutModifiers.Alt)]
        private static void OpenSeparatorManagerPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.Separators);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Tools Panel")]
        private static void OpenToolsPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.Tools);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Presets Panel")]
        private static void OpenPresetsPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.Presets);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open General Settings Panel")]
        private static void OpenGeneralSettingsPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.GeneralSettings);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Design Settings Panel")]
        private static void OpenDesignSettingsPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.DesignSettings);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Shortcut Settings Panel")]
        private static void OpenShortcutSettingsPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.ShortcutSettings);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Advanced Settings Panel")]
        private static void OpenAdvancedSettingsPanel()
        {
            HierarchyDesigner_Window_Main.SetCurrentWindow(HierarchyDesigner_Window_Main.CurrentWindow.AdvancedSettings);
            HierarchyDesigner_Window_Main.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Rename Tool Window")]
        private static void OpenRenameToolWindow()
        {
            HierarchyDesigner_Window_RenameTool.OpenWindow(null, true, 0);
        }
        #endregion

        #region Create
        [Shortcut("Hierarchy Designer/Create All Folders")]
        private static void CreateAllHierarchyFolders()
        {
            HierarchyDesigner_Utility_Folder.CreateAllFolders();
        }

        [Shortcut("Hierarchy Designer/Create Default Folder")]
        private static void CreateDefaultHierarchyFolder()
        {
            HierarchyDesigner_Utility_Folder.CreateDefaultFolder();
        }

        [Shortcut("Hierarchy Designer/Create Missing Folders")]
        private static void CreateMissingHierarchyFolders()
        {
            HierarchyDesigner_Utility_Folder.CreateMissingFolders();
        }

        [Shortcut("Hierarchy Designer/Create All Separators")]
        private static void CreateAllHierarchySeparators()
        {
            HierarchyDesigner_Utility_Separator.CreateAllSeparators();
        }

        [Shortcut("Hierarchy Designer/Create Default Separator")]
        private static void CreateDefaultHierarchySeparator()
        {
            HierarchyDesigner_Utility_Separator.CreateDefaultSeparator();
        }

        [Shortcut("Hierarchy Designer/Create Missing Separators")]
        private static void CreateMissingHierarchySeparators()
        {
            HierarchyDesigner_Utility_Separator.CreateMissingSeparators();
        }
        #endregion

        #region Refresh
        [Shortcut("Hierarchy Designer/Refresh All GameObjects' Data", KeyCode.R, ShortcutModifiers.Shift)]
        private static void RefreshAllGameObjectsData()
        {
            HierarchyDesigner_Utility_GameObject.RefreshAllGameObjectsData();
        }

        [Shortcut("Hierarchy Designer/Refresh Selected GameObject's Data", KeyCode.R, ShortcutModifiers.Alt)]
        private static void RefreshSelectedGameObjectsData()
        {
            HierarchyDesigner_Utility_GameObject.RefreshSelectedGameObjectsData();
        }

        [Shortcut("Hierarchy Designer/Refresh Selected Main Icon")]
        private static void RefreshMainIconForSelectedGameObject()
        {
            HierarchyDesigner_Utility_GameObject.RefreshMainIconForSelectedGameObject();
        }

        [Shortcut("Hierarchy Designer/Refresh Selected Component Icons")]
        private static void RefreshComponentIconsForSelectedGameObjects()
        {
            HierarchyDesigner_Utility_GameObject.RefreshComponentIconsForSelectedGameObjects();
        }

        [Shortcut("Hierarchy Designer/Refresh Selected Hierarchy Tree Icon")]
        private static void RefreshHierarchyTreeIconForSelectedGameObjects()
        {
            HierarchyDesigner_Utility_GameObject.RefreshHierarchyTreeIconForSelectedGameObjects();
        }

        [Shortcut("Hierarchy Designer/Refresh Selected Tag")]
        private static void RefreshTagForSelectedGameObjects()
        {
            HierarchyDesigner_Utility_GameObject.RefreshTagForSelectedGameObjects();
        }

        [Shortcut("Hierarchy Designer/Refresh Selected Layer")]
        private static void RefreshLayerForSelectedGameObjects()
        {
            HierarchyDesigner_Utility_GameObject.RefreshLayerForSelectedGameObjects();
        }
        #endregion
        #endregion
    }
}
#endif