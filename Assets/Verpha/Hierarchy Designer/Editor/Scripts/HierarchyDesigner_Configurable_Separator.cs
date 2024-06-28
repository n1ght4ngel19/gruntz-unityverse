#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Configurable_Separator
    {
        #region Properties
        [System.Serializable]
        public class HierarchyDesigner_SeparatorData
        {
            public string Name = "Separator";
            public Color TextColor = Color.white;
            public bool IsGradientBackground = false;
            public Color BackgroundColor = Color.gray;
            public Gradient BackgroundGradient = new Gradient();
            public int FontSize = 12;
            public FontStyle FontStyle = FontStyle.Normal;
            public TextAnchor TextAnchor = TextAnchor.MiddleCenter;
            public SeparatorImageType ImageType = SeparatorImageType.Default;
        }
        public enum SeparatorImageType
        {
            Default,
            DefaultFadedTop,
            DefaultFadedLeft,
            DefaultFadedRight,
            DefaultFadedBottom,
            DefaultFadedLeftAndRight,
            ModernI,
            ModernII,
            ModernIII,
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
            if (separators.ContainsKey(separatorName))
            {
                separators[separatorName].TextColor = textColor;
                separators[separatorName].IsGradientBackground = isGradientBackground;
                separators[separatorName].BackgroundColor = backgroundColor;
                separators[separatorName].BackgroundGradient = backgroundGradient;
                separators[separatorName].FontSize = fontSize;
                separators[separatorName].FontStyle = fontStyle;
                separators[separatorName].TextAnchor = textAnchor;
                separators[separatorName].ImageType = imageType;
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
                if (tempSeparators.ContainsKey(key))
                {
                    orderedSeparators[key] = tempSeparators[key];
                }
            }
            separators = orderedSeparators;
        }

        public static HierarchyDesigner_SeparatorData GetSeparatorData(string separatorName)
        {
            separatorName = StripPrefix(separatorName);
            if (separators.TryGetValue(separatorName, out var separatorData))
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
        private static string StripPrefix(string name)
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