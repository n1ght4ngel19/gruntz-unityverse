#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Configurable_Presets
    {
        #region Properties
        [System.Serializable]
        public class HierarchyDesigner_Preset
        {
            public string presetName;
            public Color folderTextColor;
            public int folderFontSize;
            public FontStyle folderFontStyle;
            public Color folderColor;
            public HierarchyDesigner_Configurable_Folder.FolderImageType folderImageType;
            public Color separatorTextColor;
            public bool separatorIsGradientBackground;
            public Color separatorBackgroundColor;
            public Gradient separatorBackgroundGradient;
            public FontStyle separatorFontStyle;
            public int separatorFontSize;
            public TextAnchor separatorTextAlignment;
            public HierarchyDesigner_Configurable_Separator.SeparatorImageType separatorBackgroundImageType;
            public Color tagTextColor;
            public FontStyle tagFontStyle;
            public int tagFontSize;
            public TextAnchor tagTextAnchor;
            public Color layerTextColor;
            public FontStyle layerFontStyle;
            public int layerFontSize;
            public TextAnchor layerTextAnchor;
            public Color treeColor;
            public Color hierarchyLineColor;
            public Color lockColor;
            public int lockFontSize;
            public FontStyle lockFontStyle;
            public TextAnchor lockTextAnchor;

            public HierarchyDesigner_Preset(
                string name,
                Color folderTextColor, 
                int folderFontSize, 
                FontStyle folderFontStyle,
                Color folderColor,
                HierarchyDesigner_Configurable_Folder.FolderImageType folderImageType,
                Color separatorTextColor,
                bool separatorIsGradientBackground,
                Color separatorBackgroundColor,
                Gradient separatorBackgroundGradient,
                FontStyle separatorFontStyle,
                int separatorFontSize,
                TextAnchor separatorTextAlignment,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType separatorBackgroundImageType,
                Color tagTextColor,
                FontStyle tagFontStyle,
                int tagFontSize,
                TextAnchor tagTextAnchor,
                Color layerTextColor,
                FontStyle layerFontStyle,
                int layerFontSize,
                TextAnchor layerTextAnchor,
                Color treeColor,
                Color hierarchyLineColor,
                Color lockColor,
                int lockFontSize,
                FontStyle lockFontStyle,
                TextAnchor lockTextAnchor)
            {
                this.presetName = name;
                this.folderTextColor = folderTextColor;
                this.folderFontSize = folderFontSize;
                this.folderFontStyle = folderFontStyle;
                this.folderColor = folderColor;
                this.folderImageType = folderImageType;
                this.separatorTextColor = separatorTextColor;
                this.separatorIsGradientBackground = separatorIsGradientBackground;
                this.separatorBackgroundColor = separatorBackgroundColor;
                this.separatorBackgroundGradient = separatorBackgroundGradient;
                this.separatorFontStyle = separatorFontStyle;
                this.separatorFontSize = separatorFontSize;
                this.separatorTextAlignment = separatorTextAlignment;
                this.separatorBackgroundImageType = separatorBackgroundImageType;
                this.tagTextColor = tagTextColor;
                this.tagFontStyle = tagFontStyle;
                this.tagFontSize = tagFontSize;
                this.tagTextAnchor = tagTextAnchor;
                this.layerTextColor = layerTextColor;
                this.layerFontStyle = layerFontStyle;
                this.layerFontSize = layerFontSize;
                this.layerTextAnchor = layerTextAnchor;
                this.treeColor = treeColor;
                this.hierarchyLineColor = hierarchyLineColor;
                this.lockColor = lockColor;
                this.lockFontSize = lockFontSize;
                this.lockFontStyle = lockFontStyle;
                this.lockTextAnchor = lockTextAnchor;
            }
        }

        private static List<HierarchyDesigner_Preset> presets;
        #endregion

        #region Initialization
        public static void Initialize()
        {
            LoadPresets();
        }

        private static void LoadPresets()
        {
            presets = new List<HierarchyDesigner_Preset>
            {
                HierarchyDesigner_Utility_Presets.AgeOfEnlightenmentPreset(),
                HierarchyDesigner_Utility_Presets.AzureDreamscapePreset(),
                HierarchyDesigner_Utility_Presets.BlackAndGoldPreset(),
                HierarchyDesigner_Utility_Presets.BlackAndWhitePreset(),
                HierarchyDesigner_Utility_Presets.BloodyMaryPreset(),
                HierarchyDesigner_Utility_Presets.BlueHarmonyPreset(),
                HierarchyDesigner_Utility_Presets.DeepOceanPreset(),
                HierarchyDesigner_Utility_Presets.DunesPreset(),
                HierarchyDesigner_Utility_Presets.FreshSwampPreset(),
                HierarchyDesigner_Utility_Presets.FrostyFogPreset(),
                HierarchyDesigner_Utility_Presets.IronCinderPreset(),
                HierarchyDesigner_Utility_Presets.JadeLakePreset(),
                HierarchyDesigner_Utility_Presets.LittleRedPreset(),
                HierarchyDesigner_Utility_Presets.MinimalBlackPreset(),
                HierarchyDesigner_Utility_Presets.MinimalWhitePreset(),
                HierarchyDesigner_Utility_Presets.NaturePreset(),
                HierarchyDesigner_Utility_Presets.NavyBlueLightPreset(),
                HierarchyDesigner_Utility_Presets.OldSchoolPreset(),
                HierarchyDesigner_Utility_Presets.PrettyPinkPreset(),
                HierarchyDesigner_Utility_Presets.PrismaticPreset(),
                HierarchyDesigner_Utility_Presets.RedDawnPreset(),
                HierarchyDesigner_Utility_Presets.StrawberrySalmonPreset(),
                HierarchyDesigner_Utility_Presets.SunflowerPreset(),
                HierarchyDesigner_Utility_Presets.TheTwoRealmsPreset(),
                HierarchyDesigner_Utility_Presets.WildcatsPreset(),
                HierarchyDesigner_Utility_Presets.YoungMonarchPreset(),
            };
        }
        #endregion

        #region Methods
        public static List<HierarchyDesigner_Preset> Presets
        {
            get
            {
                if (presets == null) { LoadPresets(); }
                return presets;
            }
        }

        public static string[] GetPresetNames()
        {
            List<HierarchyDesigner_Preset> presetList = Presets;
            string[] presetNames = new string[presetList.Count];
            for (int i = 0; i < presetList.Count; i++)
            {
                presetNames[i] = presetList[i].presetName;
            }
            return presetNames;
        }

        public static Dictionary<string, List<string>> GetPresetNamesGrouped()
        {
            List<HierarchyDesigner_Preset> presetList = Presets;
            Dictionary<string, List<string>> groupedPresets = new Dictionary<string, List<string>>
            {
                { "A-E", new List<string>() },
                { "F-J", new List<string>() },
                { "K-O", new List<string>() },
                { "P-T", new List<string>() },
                { "U-Z", new List<string>() }
            };

            foreach (HierarchyDesigner_Preset preset in presetList)
            {
                char firstChar = preset.presetName.ToUpper()[0];
                if (firstChar >= 'A' && firstChar <= 'E') { groupedPresets["A-E"].Add(preset.presetName); }
                else if (firstChar >= 'F' && firstChar <= 'J') { groupedPresets["F-J"].Add(preset.presetName); }
                else if (firstChar >= 'K' && firstChar <= 'O') { groupedPresets["K-O"].Add(preset.presetName); }
                else if (firstChar >= 'P' && firstChar <= 'T') { groupedPresets["P-T"].Add(preset.presetName); }
                else if (firstChar >= 'U' && firstChar <= 'Z') { groupedPresets["U-Z"].Add(preset.presetName); }
            }
            return groupedPresets;
        }
        #endregion
    }
}
#endif