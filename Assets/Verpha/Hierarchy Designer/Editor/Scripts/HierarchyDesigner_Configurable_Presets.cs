#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Configurable_Presets
    {
        #region Properties
        [System.Serializable]
        public class HierarchyDesigner_Preset
        {
            public string presetName;
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
            public Color layerTextColor;
            public FontStyle layerFontStyle;
            public int layerFontSize;
            public Color treeColor;

            public HierarchyDesigner_Preset(
                string name,
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
                Color layerTextColor,
                FontStyle layerFontStyle,
                int layerFontSize,
                Color treeColor)
            {
                this.presetName = name;
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
                this.layerTextColor = layerTextColor;
                this.layerFontStyle = layerFontStyle;
                this.layerFontSize = layerFontSize;
                this.treeColor = treeColor;
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
                HierarchyDesigner_Utility_Presets.AzureDreamscapePreset(),
                HierarchyDesigner_Utility_Presets.BlackAndGoldPreset(),
                HierarchyDesigner_Utility_Presets.BlackAndWhitePreset(),
                HierarchyDesigner_Utility_Presets.BloodyMaryPreset(),
                HierarchyDesigner_Utility_Presets.BlueHarmonyPreset(),
                HierarchyDesigner_Utility_Presets.DeepOceanPreset(),
                HierarchyDesigner_Utility_Presets.DunesPreset(),
                HierarchyDesigner_Utility_Presets.MinimalBlackPreset(),
                HierarchyDesigner_Utility_Presets.MinimalWhitePreset(),
                HierarchyDesigner_Utility_Presets.NaturePreset(),
                HierarchyDesigner_Utility_Presets.NavyBlueLightPreset(),
                HierarchyDesigner_Utility_Presets.OldSchoolPreset(),
                HierarchyDesigner_Utility_Presets.PrettyPinkPreset(),
                HierarchyDesigner_Utility_Presets.PrismaticPreset(),
                HierarchyDesigner_Utility_Presets.RedDawnPreset(),
                HierarchyDesigner_Utility_Presets.SunflowerPreset(),
                HierarchyDesigner_Utility_Presets.WildcatsPreset(),
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
        #endregion
    }
}
#endif