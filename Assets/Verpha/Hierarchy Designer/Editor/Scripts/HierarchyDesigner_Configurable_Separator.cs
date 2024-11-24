#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Configurable_Separator
    {
        #region Properties
        [System.Serializable]
        public class HierarchyDesigner_SeparatorData
        {
            public string Name = "Separator";
            public Color TextColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextColor;
            public bool IsGradientBackground = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultIsGradientBackground;
            public Color BackgroundColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundColor;
            public Gradient BackgroundGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundGradient;
            public int FontSize = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontSize;
            public FontStyle FontStyle = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontStyle;
            public TextAnchor TextAnchor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextAnchor;
            public SeparatorImageType ImageType = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultImageType;
        }
        public enum SeparatorImageType
        {
            Default,
            DefaultFadedTop,
            DefaultFadedLeft,
            DefaultFadedRight,
            DefaultFadedBottom,
            DefaultFadedSideways,
            ClassicI,
            ClassicII,
            ModernI,
            ModernII,
            ModernIII,
            NeoI,
            NeoII,
            NextGenI,
            NextGenII,
        }
        private const string settingsFileName = "HierarchyDesigner_SavedData_Separators.json";
        private static Dictionary<string, HierarchyDesigner_SeparatorData> separators = new Dictionary<string, HierarchyDesigner_SeparatorData>();
        #endregion

        #region Initialization
        public static void Initialize()
        {
            LoadSettings();
            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            Dictionary<int, (Color textColor, bool isGradientBackground, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, SeparatorImageType separatorImageType)> separatorCache = new Dictionary<int, (Color textColor, bool isGradientBackground, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, SeparatorImageType separatorImageType)>();
            foreach (KeyValuePair<string, HierarchyDesigner_SeparatorData> separator in separators)
            {
                int instanceID = separator.Key.GetHashCode();
                separatorCache[instanceID] = (separator.Value.TextColor, separator.Value.IsGradientBackground, separator.Value.BackgroundColor, separator.Value.BackgroundGradient, separator.Value.FontSize, separator.Value.FontStyle, separator.Value.TextAnchor, separator.Value.ImageType);
            }
            HierarchyDesigner_Manager_GameObject.SeparatorCache = separatorCache;
        }
        #endregion

        #region Accessors
        public static void SetSeparatorData(string separatorName, Color textColor, bool isGradientBackground, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, SeparatorImageType imageType)
        {
            separatorName = StripPrefix(separatorName);
            if (separators.TryGetValue(separatorName, out HierarchyDesigner_SeparatorData separatorData))
            {
                separatorData.TextColor = textColor;
                separatorData.IsGradientBackground = isGradientBackground;
                separatorData.BackgroundColor = backgroundColor;
                separatorData.BackgroundGradient = backgroundGradient;
                separatorData.FontSize = fontSize;
                separatorData.FontStyle = fontStyle;
                separatorData.TextAnchor = textAnchor;
                separatorData.ImageType = imageType;
            }
            else
            {
                separators[separatorName] = new HierarchyDesigner_SeparatorData
                {
                    Name = separatorName,
                    TextColor = textColor,
                    IsGradientBackground = isGradientBackground,
                    BackgroundColor = backgroundColor,
                    BackgroundGradient = backgroundGradient,
                    FontSize = fontSize,
                    FontStyle = fontStyle,
                    TextAnchor = textAnchor,
                    ImageType = imageType
                };
            }
            SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearSeparatorCache();
        }

        public static void ApplyChangesToSeparators(Dictionary<string, HierarchyDesigner_SeparatorData> tempSeparators, List<string> separatorsOrder)
        {
            Dictionary<string, HierarchyDesigner_SeparatorData> orderedSeparators = new Dictionary<string, HierarchyDesigner_SeparatorData>();
            foreach (string key in separatorsOrder)
            {
                if (tempSeparators.TryGetValue(key, out HierarchyDesigner_SeparatorData separatorData))
                {
                    orderedSeparators[key] = separatorData;
                }
            }
            separators = orderedSeparators;
        }

        public static HierarchyDesigner_SeparatorData GetSeparatorData(string separatorName)
        {
            separatorName = StripPrefix(separatorName);
            if (separators.TryGetValue(separatorName, out HierarchyDesigner_SeparatorData separatorData))
            {
                return separatorData;
            }
            return null;
        }

        public static Dictionary<string, HierarchyDesigner_SeparatorData> GetAllSeparatorsData(bool updateData)
        {
            if (updateData) { LoadSettings(); }
            return new Dictionary<string, HierarchyDesigner_SeparatorData>(separators);
        }

        public static bool RemoveSeparatorData(string separatorName)
        {
            separatorName = StripPrefix(separatorName);
            if (separators.TryGetValue(separatorName, out _))
            {
                separators.Remove(separatorName);
                SaveSettings();
                HierarchyDesigner_Manager_GameObject.ClearSeparatorCache();
                return true;
            }
            return false;
        }

        public static Dictionary<string, List<string>> GetSeparatorImageTypesGrouped()
        {
            return new Dictionary<string, List<string>>
            {
                { "Default", new List<string>
                    {
                        "Default",
                        "Default Faded Top",
                        "Default Faded Left",
                        "Default Faded Right",
                        "Default Faded Bottom",
                        "Default Faded Sideways"
                    }
                },
                { "Classic", new List<string>
                    {
                        "Classic I",
                        "Classic II",
                    }
                },
                { "Modern", new List<string>
                    {
                        "Modern I",
                        "Modern II",
                        "Modern III"
                    }
                },
                { "Neo", new List<string>
                    {
                        "Neo I",
                        "Neo II"
                    } 
                },
                { "Next-Gen", new List<string>
                    {
                        "Next-Gen I",
                        "Next-Gen II"

                    }
                }
            };
        }

        public static SeparatorImageType ParseSeparatorImageType(string displayName)
        {
            switch (displayName)
            {
                case "Default":
                    return SeparatorImageType.Default;
                case "Default Faded Top":
                    return SeparatorImageType.DefaultFadedTop;
                case "Default Faded Left":
                    return SeparatorImageType.DefaultFadedLeft;
                case "Default Faded Right":
                    return SeparatorImageType.DefaultFadedRight;
                case "Default Faded Bottom":
                    return SeparatorImageType.DefaultFadedBottom;
                case "Default Faded Sideways":
                    return SeparatorImageType.DefaultFadedSideways;
                case "Classic I":
                    return SeparatorImageType.ClassicI;
                case "Classic II":
                    return SeparatorImageType.ClassicII;
                case "Modern I":
                    return SeparatorImageType.ModernI;
                case "Modern II":
                    return SeparatorImageType.ModernII;
                case "Modern III":
                    return SeparatorImageType.ModernIII;
                case "Neo I":
                    return SeparatorImageType.NeoI;
                case "Neo II":
                    return SeparatorImageType.NeoII;
                case "Next-Gen I":
                    return SeparatorImageType.NextGenI;
                case "Next-Gen II":
                    return SeparatorImageType.NextGenII;
                default:
                    return SeparatorImageType.Default;
            }
        }

        public static string GetSeparatorImageTypeDisplayName(SeparatorImageType imageType)
        {
            switch (imageType)
            {
                case SeparatorImageType.Default:
                    return "Default";
                case SeparatorImageType.DefaultFadedTop:
                    return "Default Faded Top";
                case SeparatorImageType.DefaultFadedLeft:
                    return "Default Faded Left";
                case SeparatorImageType.DefaultFadedRight:
                    return "Default Faded Right";
                case SeparatorImageType.DefaultFadedBottom:
                    return "Default Faded Bottom";
                case SeparatorImageType.DefaultFadedSideways:
                    return "Default Faded Sideways";
                case SeparatorImageType.ClassicI:
                    return "Classic I";
                case SeparatorImageType.ClassicII:
                    return "Classic II";
                case SeparatorImageType.ModernI:
                    return "Modern I";
                case SeparatorImageType.ModernII:
                    return "Modern II";
                case SeparatorImageType.ModernIII:
                    return "Modern III";
                case SeparatorImageType.NeoI:
                    return "Neo I";
                case SeparatorImageType.NeoII:
                    return "Neo II";
                case SeparatorImageType.NextGenI:
                    return "Next-Gen I";
                case SeparatorImageType.NextGenII:
                    return "Next-Gen II";
                default:
                    return imageType.ToString();
            }
        }
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_SeparatorData> serializableList = new HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_SeparatorData>(new List<HierarchyDesigner_SeparatorData>(separators.Values));
            string json = JsonUtility.ToJson(serializableList, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_SeparatorData> loadedSeparators = JsonUtility.FromJson<HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_SeparatorData>>(json);
                separators.Clear();
                foreach (HierarchyDesigner_SeparatorData separator in loadedSeparators.items)
                {
                    separator.ImageType = HierarchyDesigner_Shared_EnumFilter.ParseEnum(separator.ImageType.ToString(), SeparatorImageType.Default);
                    separators[separator.Name] = separator;
                }
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            separators = new Dictionary<string, HierarchyDesigner_SeparatorData>();
        }
        #endregion

        #region Operations
        public static string StripPrefix(string name)
        {
            if (name.StartsWith("//"))
            {
                return name.Substring(2).Trim();
            }
            return name.Trim();
        }
        #endregion
    }
}
#endif