#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Configurable_AdvancedSettings
    {
        #region Properties
        [System.Serializable]
        private class HierarchyDesigner_AdvancedSettings
        {
            #region Core
            public HierarchyDesignerLocation HierarchyLocation = HierarchyDesignerLocation.Tools;
            public UpdateMode MainIconUpdateMode = UpdateMode.Dynamic;
            public UpdateMode ComponentsIconsUpdateMode = UpdateMode.Dynamic;
            public UpdateMode HierarchyTreeUpdateMode = UpdateMode.Dynamic;
            public UpdateMode TagUpdateMode = UpdateMode.Dynamic;
            public UpdateMode LayerUpdateMode = UpdateMode.Dynamic;
            #endregion
            #region Main Icon
            public bool EnableDynamicBackgroundForGameObjectMainIcon = true;
            public bool EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = true;
            #endregion
            #region Component Icons
            public bool EnableCustomizationForGameObjectComponentIcons = true;
            public bool EnableTooltipOnComponentIconHovered = true;
            public bool EnableActiveStateEffectForComponentIcons = true;
            public bool DisableComponentIconsForInactiveGameObjects = true;
            #endregion
            #region Folder
            public bool IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder = true;
            #endregion
            #region Separator
            public bool IncludeBackgroundImageForGradientBackground = true;
            #endregion
            #region Hierarchy Tools
            public bool ExcludeFoldersFromCountSelectToolCalculations = true;
            public bool ExcludeSeparatorsFromCountSelectToolCalculations = true;
            #endregion
        }
        public enum HierarchyDesignerLocation { Author, Plugins, Tools, TopBar, Window };
        public enum UpdateMode { Dynamic, Smart }
        private static HierarchyDesigner_AdvancedSettings advancedSettings = new HierarchyDesigner_AdvancedSettings();
        private const string settingsFileName = "HierarchyDesigner_SavedData_AdvancedSettings.json";
        #endregion

        #region Initialization
        public static void Initialize()
        {
            LoadSettings();

            string currentBaseHierarchyDesigner = ReadBaseHierarchyDesigner();
            string expectedBaseHierarchyDesigner = GetBaseHierarchyDesigner(advancedSettings.HierarchyLocation);
            if (currentBaseHierarchyDesigner != expectedBaseHierarchyDesigner)
            {
                GenerateConstantsFile(HierarchyLocation);
            }

            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static string ReadBaseHierarchyDesigner()
        {
            string filePath = HierarchyDesigner_Manager_Data.GetScriptsFilePath("HierarchyDesigner_Shared_Constants.cs");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (line.Contains("public const string Base_HierarchyDesigner ="))
                    {
                        int startIndex = line.IndexOf("\"") + 1;
                        int endIndex = line.LastIndexOf("\"");
                        return line.Substring(startIndex, endIndex - startIndex);
                    }
                }
            }
            return null;
        }

        private static string GetBaseHierarchyDesigner(HierarchyDesignerLocation hierarchyLocation)
        {
            return hierarchyLocation switch
            {
                HierarchyDesignerLocation.Author => "Verpha/Hierarchy Designer",
                HierarchyDesignerLocation.Plugins => "Plugins/Hierarchy Designer",
                HierarchyDesignerLocation.Tools => "Tools/Hierarchy Designer",
                HierarchyDesignerLocation.TopBar => "Hierarchy Designer/Open Window",
                HierarchyDesignerLocation.Window => "Window/Hierarchy Designer",
                _ => "Hierarchy Designer"
            };
        }

        public static void GenerateConstantsFile(HierarchyDesignerLocation tempHierarchyLocation)
        {
            string baseHierarchyDesigner = GetBaseHierarchyDesigner(tempHierarchyLocation);

            string filePath = HierarchyDesigner_Manager_Data.GetScriptsFilePath("HierarchyDesigner_Shared_Constants.cs");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#if UNITY_EDITOR");
            sb.AppendLine("namespace Verpha.HierarchyDesigner");
            sb.AppendLine("{");
            sb.AppendLine("    public static class HierarchyDesigner_Shared_Constants");
            sb.AppendLine("    {");
            sb.AppendFormat("        public const string Base_HierarchyDesigner = \"{0}\";", baseHierarchyDesigner);
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine("#endif");

            File.WriteAllText(filePath, sb.ToString());
            AssetDatabase.Refresh();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            HierarchyDesigner_Manager_GameObject.MainIconUpdateModeCache = MainIconUpdateMode;
            HierarchyDesigner_Manager_GameObject.ComponentsIconsUpdateModeCache = ComponentsIconsUpdateMode;
            HierarchyDesigner_Manager_GameObject.HierarchyTreeUpdateModeCache = HierarchyTreeUpdateMode;
            HierarchyDesigner_Manager_GameObject.TagUpdateModeCache = TagUpdateMode;
            HierarchyDesigner_Manager_GameObject.LayerUpdateModeCache = LayerUpdateMode;
            HierarchyDesigner_Manager_GameObject.EnableDynamicBackgroundForGameObjectMainIconCache = EnableDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Manager_GameObject.EnablePreciseRectForDynamicBackgroundForGameObjectMainIconCache = EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Manager_GameObject.DisableComponentIconsForInactiveGameObjectsCache = DisableComponentIconsForInactiveGameObjects;
            HierarchyDesigner_Manager_GameObject.EnableCustomizationForGameObjectComponentIconsCache = EnableCustomizationForGameObjectComponentIcons;
            HierarchyDesigner_Manager_GameObject.EnableTooltipOnComponentIconHoveredCache = EnableTooltipOnComponentIconHovered;
            HierarchyDesigner_Manager_GameObject.EnableActiveStateEffectForComponentIconsCache = EnableActiveStateEffectForComponentIcons;
            HierarchyDesigner_Manager_GameObject.IncludeBackgroundImageForGradientBackgroundCache = IncludeBackgroundImageForGradientBackground;
        }
        #endregion

        #region Accessors
        #region Core
        public static HierarchyDesignerLocation HierarchyLocation
        {
            get => advancedSettings.HierarchyLocation;
            set
            {
                if (advancedSettings.HierarchyLocation != value)
                {
                    advancedSettings.HierarchyLocation = value;
                }
            }
        }

        public static UpdateMode MainIconUpdateMode
        {
            get => advancedSettings.MainIconUpdateMode;
            set
            {
                if (advancedSettings.MainIconUpdateMode != value)
                {
                    advancedSettings.MainIconUpdateMode = value;
                    HierarchyDesigner_Manager_GameObject.MainIconUpdateModeCache = value;
                }
            }
        }

        public static UpdateMode ComponentsIconsUpdateMode
        {
            get => advancedSettings.ComponentsIconsUpdateMode;
            set
            {
                if (advancedSettings.ComponentsIconsUpdateMode != value)
                {
                    advancedSettings.ComponentsIconsUpdateMode = value;
                    HierarchyDesigner_Manager_GameObject.ComponentsIconsUpdateModeCache = value;
                }
            }
        }

        public static UpdateMode HierarchyTreeUpdateMode
        {
            get => advancedSettings.HierarchyTreeUpdateMode;
            set
            {
                if (advancedSettings.HierarchyTreeUpdateMode != value)
                {
                    advancedSettings.HierarchyTreeUpdateMode = value;
                    HierarchyDesigner_Manager_GameObject.HierarchyTreeUpdateModeCache = value;
                }
            }
        }

        public static UpdateMode TagUpdateMode
        {
            get => advancedSettings.TagUpdateMode;
            set
            {
                if (advancedSettings.TagUpdateMode != value)
                {
                    advancedSettings.TagUpdateMode = value;
                    HierarchyDesigner_Manager_GameObject.TagUpdateModeCache = value;
                }
            }
        }

        public static UpdateMode LayerUpdateMode
        {
            get => advancedSettings.LayerUpdateMode;
            set
            {
                if (advancedSettings.LayerUpdateMode != value)
                {
                    advancedSettings.LayerUpdateMode = value;
                    HierarchyDesigner_Manager_GameObject.LayerUpdateModeCache = value;
                }
            }
        }
        #endregion

        #region #region Main Icon
        public static bool EnableDynamicBackgroundForGameObjectMainIcon
        {
            get => advancedSettings.EnableDynamicBackgroundForGameObjectMainIcon;
            set
            {
                if (advancedSettings.EnableDynamicBackgroundForGameObjectMainIcon != value)
                {
                    advancedSettings.EnableDynamicBackgroundForGameObjectMainIcon = value;
                    HierarchyDesigner_Manager_GameObject.EnableDynamicBackgroundForGameObjectMainIconCache = value;
                }
            }
        }

        public static bool EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon
        {
            get => advancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            set
            {
                if (advancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon != value)
                {
                    advancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = value;
                    HierarchyDesigner_Manager_GameObject.EnablePreciseRectForDynamicBackgroundForGameObjectMainIconCache = value;
                }
            }
        }
        #endregion

        #region #region Component Icons
        public static bool EnableCustomizationForGameObjectComponentIcons
        {
            get => advancedSettings.EnableCustomizationForGameObjectComponentIcons;
            set
            {
                if (advancedSettings.EnableCustomizationForGameObjectComponentIcons != value)
                {
                    advancedSettings.EnableCustomizationForGameObjectComponentIcons = value;
                    HierarchyDesigner_Manager_GameObject.EnableCustomizationForGameObjectComponentIconsCache = value;
                }
            }
        }

        public static bool EnableTooltipOnComponentIconHovered
        {
            get => advancedSettings.EnableTooltipOnComponentIconHovered;
            set
            {
                if (advancedSettings.EnableTooltipOnComponentIconHovered != value)
                {
                    advancedSettings.EnableTooltipOnComponentIconHovered = value;
                    HierarchyDesigner_Manager_GameObject.EnableTooltipOnComponentIconHoveredCache = value;
                }
            }
        }

        public static bool EnableActiveStateEffectForComponentIcons
        {
            get => advancedSettings.EnableActiveStateEffectForComponentIcons;
            set
            {
                if (advancedSettings.EnableActiveStateEffectForComponentIcons != value)
                {
                    advancedSettings.EnableActiveStateEffectForComponentIcons = value;
                    HierarchyDesigner_Manager_GameObject.EnableActiveStateEffectForComponentIconsCache = value;
                }
            }
        }

        public static bool DisableComponentIconsForInactiveGameObjects
        {
            get => advancedSettings.DisableComponentIconsForInactiveGameObjects;
            set
            {
                if (advancedSettings.DisableComponentIconsForInactiveGameObjects != value)
                {
                    advancedSettings.DisableComponentIconsForInactiveGameObjects = value;
                    HierarchyDesigner_Manager_GameObject.DisableComponentIconsForInactiveGameObjectsCache = value;
                }
            }
        }
        #endregion

        #region Folder
        public static bool IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder
        {
            get => advancedSettings.IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder;
            set
            {
                if (advancedSettings.IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder != value)
                {
                    advancedSettings.IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder = value;
                }
            }
        }
        #endregion

        #region Separator
        public static bool IncludeBackgroundImageForGradientBackground
        {
            get => advancedSettings.IncludeBackgroundImageForGradientBackground;
            set
            {
                if (advancedSettings.IncludeBackgroundImageForGradientBackground != value)
                {
                    advancedSettings.IncludeBackgroundImageForGradientBackground = value;
                    HierarchyDesigner_Manager_GameObject.IncludeBackgroundImageForGradientBackgroundCache = value;
                }
            }
        }
        #endregion

        #region Hierarchy Tools
        public static bool ExcludeFoldersFromCountSelectToolCalculations
        {
            get => advancedSettings.ExcludeFoldersFromCountSelectToolCalculations;
            set
            {
                if (advancedSettings.ExcludeFoldersFromCountSelectToolCalculations != value)
                {
                    advancedSettings.ExcludeFoldersFromCountSelectToolCalculations = value;
                }
            }
        }

        public static bool ExcludeSeparatorsFromCountSelectToolCalculations
        {
            get => advancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations;
            set
            {
                if (advancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations != value)
                {
                    advancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations = value;
                }
            }
        }
        #endregion
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            string json = JsonUtility.ToJson(advancedSettings, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_AdvancedSettings loadedSettings = JsonUtility.FromJson<HierarchyDesigner_AdvancedSettings>(json);
                loadedSettings.HierarchyLocation = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.HierarchyLocation.ToString(), HierarchyDesignerLocation.Tools);
                loadedSettings.MainIconUpdateMode = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.MainIconUpdateMode.ToString(), UpdateMode.Dynamic);
                loadedSettings.ComponentsIconsUpdateMode = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.ComponentsIconsUpdateMode.ToString(), UpdateMode.Dynamic);
                loadedSettings.HierarchyTreeUpdateMode = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.HierarchyTreeUpdateMode.ToString(), UpdateMode.Dynamic);
                loadedSettings.TagUpdateMode = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.TagUpdateMode.ToString(), UpdateMode.Dynamic);
                loadedSettings.LayerUpdateMode = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.LayerUpdateMode.ToString(), UpdateMode.Dynamic);
                advancedSettings = loadedSettings;
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            advancedSettings = new HierarchyDesigner_AdvancedSettings()
            {
                HierarchyLocation = HierarchyDesignerLocation.Tools,
                MainIconUpdateMode = UpdateMode.Dynamic,
                ComponentsIconsUpdateMode = UpdateMode.Dynamic,
                HierarchyTreeUpdateMode = UpdateMode.Dynamic,
                TagUpdateMode = UpdateMode.Dynamic,
                LayerUpdateMode = UpdateMode.Dynamic,
                EnableDynamicBackgroundForGameObjectMainIcon = true,
                EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = true,
                EnableCustomizationForGameObjectComponentIcons = true,
                EnableTooltipOnComponentIconHovered = true,
                EnableActiveStateEffectForComponentIcons = true,
                DisableComponentIconsForInactiveGameObjects = true,
                IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder = true,
                IncludeBackgroundImageForGradientBackground = true,
                ExcludeFoldersFromCountSelectToolCalculations = true,
                ExcludeSeparatorsFromCountSelectToolCalculations = true,
            };
        }
        #endregion
    }
}
#endif