#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Configurable_Folder
    {
        #region Properties
        [System.Serializable]
        public class HierarchyDesigner_FolderData
        {
            public string Name = "Folder";
            public Color TextColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultTextColor;
            public int FontSize = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontSize;
            public FontStyle FontStyle = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontStyle;
            public Color ImageColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageColor;
            public FolderImageType ImageType = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageType;
        }
        public enum FolderImageType
        { 
            Default, 
            DefaultOutline,
            ModernI,
            ModernII,
            ModernIII,
            ModernOutline,
        }
        private const string settingsFileName = "HierarchyDesigner_SavedData_Folders.json";
        private static Dictionary<string, HierarchyDesigner_FolderData> folders = new Dictionary<string, HierarchyDesigner_FolderData>();
        #endregion

        #region Initialization
        public static void Initialize()
        {
            LoadSettings();
            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            Dictionary<int, (Color textColor, int fontSize, FontStyle fontStyle, Color folderColor, FolderImageType folderImageType)> folderCache = new Dictionary<int, (Color textColor, int fontSize, FontStyle fontStyle, Color folderColor, FolderImageType folderImageType)>();
            foreach (KeyValuePair<string, HierarchyDesigner_FolderData> folder in folders)
            {
                int instanceID = folder.Key.GetHashCode();
                folderCache[instanceID] = (folder.Value.TextColor, folder.Value.FontSize, folder.Value.FontStyle, folder.Value.ImageColor, folder.Value.ImageType);
            }
            HierarchyDesigner_Manager_GameObject.FolderCache = folderCache;
        }
        #endregion

        #region Accessors
        public static void SetFolderData(string folderName, Color textColor, int fontSize, FontStyle fontStyle, Color imageColor, FolderImageType imageType)
        {
            if (folders.TryGetValue(folderName, out HierarchyDesigner_FolderData folderData))
            {
                folderData.TextColor = textColor;
                folderData.FontSize = fontSize;
                folderData.FontStyle = fontStyle;
                folderData.ImageColor = imageColor;
                folderData.ImageType = imageType;
            }
            else
            {
                folders[folderName] = new HierarchyDesigner_FolderData
                {
                    Name = folderName,
                    TextColor = textColor,
                    FontSize = fontSize,
                    FontStyle = fontStyle,
                    ImageColor = imageColor,
                    ImageType = imageType
                };
            }
            SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearFolderCache();
        }

        public static void ApplyChangesToFolders(Dictionary<string, HierarchyDesigner_FolderData> tempFolders, List<string> foldersOrder)
        {
            Dictionary<string, HierarchyDesigner_FolderData> orderedFolders = new Dictionary<string, HierarchyDesigner_FolderData>();
            foreach (string key in foldersOrder)
            {
                if (tempFolders.TryGetValue(key, out HierarchyDesigner_FolderData folderData))
                {
                    orderedFolders[key] = folderData;
                }
            }
            folders = orderedFolders;
        }

        public static HierarchyDesigner_FolderData GetFolderData(string folderName)
        {
            if (folders.TryGetValue(folderName, out HierarchyDesigner_FolderData folderData))
            {
                return folderData;
            }
            return null;
        }

        public static Dictionary<string, HierarchyDesigner_FolderData> GetAllFoldersData(bool updateData)
        {
            if (updateData) { LoadSettings(); }
            return new Dictionary<string, HierarchyDesigner_FolderData>(folders);
        }

        public static bool RemoveFolderData(string folderName)
        {
            if (folders.TryGetValue(folderName, out _))
            {
                folders.Remove(folderName);
                SaveSettings();
                HierarchyDesigner_Manager_GameObject.ClearFolderCache();
                return true;
            }
            return false;
        }

        public static Dictionary<string, List<string>> GetFolderImageTypesGrouped()
        {
            return new Dictionary<string, List<string>>
            {
                { "Default", new List<string>
                    {
                        "Default",
                        "Default Outline"
                    }
                },
                { "Modern", new List<string>
                    {
                        "Modern I",
                        "Modern II",
                        "Modern III",
                        "Modern Outline"
                    }
                }
            };
        }

        public static FolderImageType ParseFolderImageType(string displayName)
        {
            switch (displayName)
            {
                case "Default":
                    return FolderImageType.Default;
                case "Default Outline":
                    return FolderImageType.DefaultOutline;
                case "Modern I":
                    return FolderImageType.ModernI;
                case "Modern II":
                    return FolderImageType.ModernII;
                case "Modern III":
                    return FolderImageType.ModernIII;
                case "Modern Outline":
                    return FolderImageType.ModernOutline;
                default:
                    return FolderImageType.Default;
            }
        }

        public static string GetFolderImageTypeDisplayName(FolderImageType imageType)
        {
            switch (imageType)
            {
                case FolderImageType.Default:
                    return "Default";
                case FolderImageType.DefaultOutline:
                    return "Default Outline";
                case FolderImageType.ModernI:
                    return "Modern I";
                case FolderImageType.ModernII:
                    return "Modern II";
                case FolderImageType.ModernIII:
                    return "Modern III";
                case FolderImageType.ModernOutline:
                    return "Modern Outline";
                default:
                    return imageType.ToString();
            }
        }
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData> serializableList = new HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData>(new List<HierarchyDesigner_FolderData>(folders.Values));
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
                HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData> loadedFolders = JsonUtility.FromJson<HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData>>(json);
                folders.Clear();
                foreach (HierarchyDesigner_FolderData folder in loadedFolders.items)
                {
                    folder.ImageType = HierarchyDesigner_Shared_EnumFilter.ParseEnum(folder.ImageType.ToString(), FolderImageType.Default);
                    folders[folder.Name] = folder;
                }
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            folders = new Dictionary<string, HierarchyDesigner_FolderData>();
        }
        #endregion
    }
}
#endif