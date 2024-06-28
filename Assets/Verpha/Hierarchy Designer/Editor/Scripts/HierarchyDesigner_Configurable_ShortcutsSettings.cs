#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Configurable_ShortcutsSettings
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
        [Shortcut("Hierarchy Designer/Open Folder Manager Window", KeyCode.Alpha1, ShortcutModifiers.Alt)]
        private static void OpenFolderManagerWindow()
        {
            HierarchyDesigner_Window_Folder.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Separator Manager Window", KeyCode.Alpha2, ShortcutModifiers.Alt)]
        private static void OpenSeparatorManagerWindow()
        {
            HierarchyDesigner_Window_Separator.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Presets Window", KeyCode.Alpha3, ShortcutModifiers.Alt)]
        private static void OpenPresetsWindow()
        {
            HierarchyDesigner_Window_Presets.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Tools Master Window", KeyCode.Alpha4, ShortcutModifiers.Alt)]
        private static void OpenToolsMasterWindow()
        {
            HierarchyDesigner_Window_ToolsMaster.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open General Settings Window", KeyCode.Alpha1, ShortcutModifiers.Alt | ShortcutModifiers.Shift)]
        private static void OpenGeneralSettingsWindow()
        {
            HierarchyDesigner_Window_GeneralSettings.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Design Settings Window", KeyCode.Alpha2, ShortcutModifiers.Alt | ShortcutModifiers.Shift)]
        private static void OpenDesignSettingsWindow()
        {
            HierarchyDesigner_Window_DesignSettings.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Shortcut Settings Window", KeyCode.Alpha3, ShortcutModifiers.Alt | ShortcutModifiers.Shift)]
        private static void OpenShortcutSettingsWindow()
        {
            HierarchyDesigner_Window_ShortcutSettings.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Advanced Settings Window", KeyCode.Alpha4, ShortcutModifiers.Alt | ShortcutModifiers.Shift)]
        private static void OpenAdvancedSettingsWindow()
        {
            HierarchyDesigner_Window_AdvancedSettings.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Open Rename Tool Window", KeyCode.Alpha1, ShortcutModifiers.Control | ShortcutModifiers.Alt | ShortcutModifiers.Shift)]
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
        #endregion
    }
}
#endif