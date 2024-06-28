#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Configurable_AdvancedSettings
    {
        #region Properties
        [System.Serializable]
        private class HierarchyDesigner_AdvancedSettings
        {
            public HierarchyDesignerLocation HierarchyLocation = HierarchyDesignerLocation.TopBar;
            public bool EnableDynamicChangesCheckForGameObjectMainIcon = true;
            public bool EnableDynamicBackgroundForGameObjectMainIcon = true;
            public bool EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = true;
            public bool EnableCustomizationForGameObjectComponentIcons = true;
            public bool EnableTooltipOnComponentIconHovered = true;
            public bool EnableActiveStateEffectForComponentIcons = true;
            public bool DisableComponentIconsForInactiveGameObjects = true;
            public bool IncludeBackgroundImageForGradientBackground = true;
            public bool ExcludeFoldersFromCountSelectToolCalculations = true;
            public bool ExcludeSeparatorsFromCountSelectToolCalculations = true;
        }
        public enum HierarchyDesignerLocation { Author, Plugins, Tools, TopBar, Window };
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
                HierarchyDesignerLocation.TopBar => "Hierarchy Designer",
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

            string fileContent = $@"#if UNITY_EDITOR
namespace Verpha.HierarchyDesigner
{{
    public static class HierarchyDesigner_Shared_Constants
    {{
        public const string Base_HierarchyDesigner = ""{baseHierarchyDesigner}"";
    }}
}}
#endif";
            File.WriteAllText(filePath, fileContent);
            AssetDatabase.Refresh();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            HierarchyDesigner_Manager_GameObject.EnableDynamicChangesCheckForGameObjectMainIconCache = EnableDynamicChangesCheckForGameObjectMainIcon;
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

        public static bool EnableDynamicChangesCheckForGameObjectMainIcon
        {
            get => advancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon;
            set
            {
                if (advancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon != value)
                {
                    advancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon = value;
                    HierarchyDesigner_Manager_GameObject.EnableDynamicChangesCheckForGameObjectMainIconCache = value;
                }
            }
        }

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
                loadedSettings.HierarchyLocation = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.HierarchyLocation.ToString(), HierarchyDesignerLocation.TopBar);
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
                HierarchyLocation = HierarchyDesignerLocation.TopBar,
                EnableDynamicChangesCheckForGameObjectMainIcon = true,
                EnableDynamicBackgroundForGameObjectMainIcon = true,
                EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = true,
                EnableCustomizationForGameObjectComponentIcons = true,
                EnableTooltipOnComponentIconHovered = true,
                EnableActiveStateEffectForComponentIcons = true,
                DisableComponentIconsForInactiveGameObjects = true,
                IncludeBackgroundImageForGradientBackground = true,
                ExcludeFoldersFromCountSelectToolCalculations = true,
                ExcludeSeparatorsFromCountSelectToolCalculations = true,
            };
        }
        #endregion
    }
}
#endif