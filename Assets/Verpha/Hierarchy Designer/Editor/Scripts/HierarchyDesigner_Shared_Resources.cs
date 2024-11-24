#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Shared_Resources
    {
        #region General
        private static readonly string defaultFontBoldName = "Hierarchy Designer Default Font Bold";
        private static readonly string defaultFontRegularName = "Hierarchy Designer Default Font Regular";
        private static readonly string iconMenuName = "Hierarchy Designer Icon Menu";
        private static readonly string iconHomeName = "Hierarchy Designer Icon Home";
        private static readonly string iconFolderName = "Hierarchy Designer Icon Folder";
        private static readonly string iconToolsName = "Hierarchy Designer Icon Tools";
        private static readonly string iconPresetsName = "Hierarchy Designer Icon Presets";
        private static readonly string iconGeneralSettingsName = "Hierarchy Designer Icon General Settings";
        private static readonly string iconDesignSettingsName = "Hierarchy Designer Icon Design Settings";
        private static readonly string iconShortcutSettingsName = "Hierarchy Designer Icon Shortcut Settings";
        private static readonly string iconAdvancedSettingsName = "Hierarchy Designer Icon Advanced Settings";
        private static readonly string defaultTextureName = "Hierarchy Designer Default Texture";
        private static readonly string lockIconName = "Hierarchy Designer Lock Icon";
        private static readonly string unlockIconName = "Hierarchy Designer Unlock Icon";
        private static readonly string visibilityOnIconName = "Hierarchy Designer Visibility On Icon";
        private static readonly string visibilityOffIconName = "Hierarchy Designer Visibility Off Icon";

        public static readonly Font DefaultFontBold = HierarchyDesigner_Shared_TextureLoader.LoadFont(defaultFontBoldName);
        public static readonly Font DefaultFont = HierarchyDesigner_Shared_TextureLoader.LoadFont(defaultFontRegularName);
        public static readonly Texture2D IconMenu = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconMenuName);
        public static readonly Texture2D IconHome = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconHomeName);
        public static readonly Texture2D IconFolder = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconFolderName);
        public static readonly Texture2D IconTools = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconToolsName);
        public static readonly Texture2D IconPresets = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconPresetsName);
        public static readonly Texture2D IconGeneralSettings = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconGeneralSettingsName);
        public static readonly Texture2D IconDesignSettings = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconDesignSettingsName);
        public static readonly Texture2D IconShortcutSettings = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconShortcutSettingsName);
        public static readonly Texture2D IconAdvancedSettings = HierarchyDesigner_Shared_TextureLoader.LoadTexture(iconAdvancedSettingsName);
        public static readonly Texture2D DefaultTexture = HierarchyDesigner_Shared_TextureLoader.LoadTexture(defaultTextureName);
        public static readonly Texture2D LockIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(lockIconName);
        public static readonly Texture2D UnlockIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(unlockIconName);
        public static readonly Texture2D VisibilityOnIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(visibilityOnIconName);
        public static readonly Texture2D VisibilityOffIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(visibilityOffIconName);
        #endregion

        #region Tree Branches
        private static readonly string treeBranchIconDefaultIName = "Hierarchy Designer Tree Branch Icon Default I";
        private static readonly string treeBranchIconDefaultLName = "Hierarchy Designer Tree Branch Icon Default L";
        private static readonly string treeBranchIconDefaultTName = "Hierarchy Designer Tree Branch Icon Default T";
        private static readonly string treeBranchIconDefaultTerminalBudName = "Hierarchy Designer Tree Branch Icon Default Terminal Bud";
        private static readonly string treeBranchIconCurvedIName = "Hierarchy Designer Tree Branch Icon Curved I";
        private static readonly string treeBranchIconCurvedLName = "Hierarchy Designer Tree Branch Icon Curved L";
        private static readonly string treeBranchIconCurvedTName = "Hierarchy Designer Tree Branch Icon Curved T";
        private static readonly string treeBranchIconCurvedTerminalBudName = "Hierarchy Designer Tree Branch Icon Curved Terminal Bud";
        private static readonly string treeBranchIconDottedIName = "Hierarchy Designer Tree Branch Icon Dotted I";
        private static readonly string treeBranchIconDottedLName = "Hierarchy Designer Tree Branch Icon Dotted L";
        private static readonly string treeBranchIconDottedTName = "Hierarchy Designer Tree Branch Icon Dotted T";
        private static readonly string treeBranchIconDottedTerminalBudName = "Hierarchy Designer Tree Branch Icon Dotted Terminal Bud";
        private static readonly string treeBranchIconSegmentedIName = "Hierarchy Designer Tree Branch Icon Segmented I";
        private static readonly string treeBranchIconSegmentedLName = "Hierarchy Designer Tree Branch Icon Segmented L";
        private static readonly string treeBranchIconSegmentedTName = "Hierarchy Designer Tree Branch Icon Segmented T";
        private static readonly string treeBranchIconSegmentedTerminalBudName = "Hierarchy Designer Tree Branch Icon Segmented Terminal Bud";

        public static readonly Texture2D TreeBranchIconDefault_I = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDefaultIName);
        public static readonly Texture2D TreeBranchIconDefault_L = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDefaultLName);
        public static readonly Texture2D TreeBranchIconDefault_T = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDefaultTName);
        public static readonly Texture2D TreeBranchIconDefault_TerminalBud = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDefaultTerminalBudName);
        public static readonly Texture2D TreeBranchIconCurved_I = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconCurvedIName);
        public static readonly Texture2D TreeBranchIconCurved_L = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconCurvedLName);
        public static readonly Texture2D TreeBranchIconCurved_T = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconCurvedTName);
        public static readonly Texture2D TreeBranchIconCurved_TerminalBud = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconCurvedTerminalBudName);
        public static readonly Texture2D TreeBranchIconDotted_I = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDottedIName);
        public static readonly Texture2D TreeBranchIconDotted_L = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDottedLName);
        public static readonly Texture2D TreeBranchIconDotted_T = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDottedTName);
        public static readonly Texture2D TreeBranchIconDotted_TerminalBud = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconDottedTerminalBudName);
        public static readonly Texture2D TreeBranchIconSegmented_I = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconSegmentedIName);
        public static readonly Texture2D TreeBranchIconSegmented_L = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconSegmentedLName);
        public static readonly Texture2D TreeBranchIconSegmented_T = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconSegmentedTName);
        public static readonly Texture2D TreeBranchIconSegmented_TerminalBud = HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconSegmentedTerminalBudName);

        public static Texture2D GetTreeBranchIconI(HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType imageType)
        {
            switch (imageType)
            {
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Curved:
                    return TreeBranchIconCurved_I;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Dotted:
                    return TreeBranchIconDotted_I;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Segmented:
                    return TreeBranchIconSegmented_I;
                default:
                    return TreeBranchIconDefault_I;
            }
        }

        public static Texture2D GetTreeBranchIconL(HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType imageType)
        {
            switch (imageType)
            {
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Curved:
                    return TreeBranchIconCurved_L;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Dotted:
                    return TreeBranchIconDotted_L;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Segmented:
                    return TreeBranchIconSegmented_L;
                default:
                    return TreeBranchIconDefault_L;
            }
        }

        public static Texture2D GetTreeBranchIconT(HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType imageType)
        {
            switch (imageType)
            {
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Curved:
                    return TreeBranchIconCurved_T;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Dotted:
                    return TreeBranchIconDotted_T;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Segmented:
                    return TreeBranchIconSegmented_T;
                default:
                    return TreeBranchIconDefault_T;
            }
        }

        public static Texture2D GetTreeBranchIconTerminalBud(HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType imageType)
        {
            switch (imageType)
            {
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Curved:
                    return TreeBranchIconCurved_TerminalBud;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Dotted:
                    return TreeBranchIconDotted_TerminalBud;
                case HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType.Segmented:
                    return TreeBranchIconSegmented_TerminalBud;
                default:
                    return TreeBranchIconDefault_TerminalBud;
            }
        }
        #endregion

        #region Folder Images
        private static readonly string folderDefaultIconName = "Hierarchy Designer Folder Icon Default";
        private static readonly string folderDefaultOutlineIconName = "Hierarchy Designer Folder Icon Default Outline";
        private static readonly string folderModernIIconName = "Hierarchy Designer Folder Icon Modern I";
        private static readonly string folderModernIIIconName = "Hierarchy Designer Folder Icon Modern II";
        private static readonly string folderModernIIIIconName = "Hierarchy Designer Folder Icon Modern III";
        private static readonly string folderModernOutlineIconName = "Hierarchy Designer Folder Icon Modern Outline";
        private static readonly string folderInspectorIconName = "Hierarchy Designer Folder Icon Inspector";

        public static readonly Texture2D FolderDefaultIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderDefaultIconName);
        public static readonly Texture2D FolderDefaultOutlineIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderDefaultOutlineIconName);
        public static readonly Texture2D FolderModernIIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderModernIIconName);
        public static readonly Texture2D FolderModernIIIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderModernIIIconName);
        public static readonly Texture2D FolderModernIIIIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderModernIIIIconName);
        public static readonly Texture2D FolderModernOutlineIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderModernOutlineIconName);
        public static readonly Texture2D FolderInspectorIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderInspectorIconName);

        public static Texture2D FolderImageType(HierarchyDesigner_Configurable_Folder.FolderImageType folderImageType)
        {
            switch (folderImageType)
            {
                case HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline:
                    return FolderDefaultOutlineIcon;
                case HierarchyDesigner_Configurable_Folder.FolderImageType.ModernI:
                    return FolderModernIIcon;
                case HierarchyDesigner_Configurable_Folder.FolderImageType.ModernII:
                    return FolderModernIIIcon;
                case HierarchyDesigner_Configurable_Folder.FolderImageType.ModernIII:
                    return FolderModernIIIIcon;
                case HierarchyDesigner_Configurable_Folder.FolderImageType.ModernOutline:
                    return FolderModernOutlineIcon;
                default:
                    return FolderDefaultIcon;
            }
        }
        #endregion

        #region Separator Images
        private static readonly string separatorBackgroundImageDefaultName = "Hierarchy Designer Separator Background Image Default";
        private static readonly string separatorBackgroundImageDefaultFadedBottomName = "Hierarchy Designer Separator Background Image Default Faded Bottom";
        private static readonly string separatorBackgroundImageDefaultFadedLeftName = "Hierarchy Designer Separator Background Image Default Faded Left";
        private static readonly string separatorBackgroundImageDefaultFadedSidewaysName = "Hierarchy Designer Separator Background Image Default Faded Sideways";
        private static readonly string separatorBackgroundImageDefaultFadedRightName = "Hierarchy Designer Separator Background Image Default Faded Right";
        private static readonly string separatorBackgroundImageDefaultFadedTopName = "Hierarchy Designer Separator Background Image Default Faded Top";
        private static readonly string separatorBackgroundImageClassicIName = "Hierarchy Designer Separator Background Image Classic I";
        private static readonly string separatorBackgroundImageClassicIIName = "Hierarchy Designer Separator Background Image Classic II";
        private static readonly string separatorBackgroundImageModernIName = "Hierarchy Designer Separator Background Image Modern I";
        private static readonly string separatorBackgroundImageModernIIName = "Hierarchy Designer Separator Background Image Modern II";
        private static readonly string separatorBackgroundImageModernIIIName = "Hierarchy Designer Separator Background Image Modern III";
        private static readonly string separatorBackgroundImageNeoIName = "Hierarchy Designer Separator Background Image Neo I";
        private static readonly string separatorBackgroundImageNeoIIName = "Hierarchy Designer Separator Background Image Neo II";
        private static readonly string separatorBackgroundImageNextGenIName = "Hierarchy Designer Separator Background Image Next-Gen I";
        private static readonly string separatorBackgroundImageNextGenIIName = "Hierarchy Designer Separator Background Image Next-Gen II";
        private static readonly string separatorInspectorIconName = "Hierarchy Designer Separator Icon Inspector";

        public static readonly Texture2D SeparatorBackgroundImageDefault = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultName);
        public static readonly Texture2D SeparatorBackgroundImageDefaultFadedBottom = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedBottomName);
        public static readonly Texture2D SeparatorBackgroundImageDefaultFadedLeft = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedLeftName);
        public static readonly Texture2D SeparatorBackgroundImageDefaultFadedSideways = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedSidewaysName);
        public static readonly Texture2D SeparatorBackgroundImageDefaultFadedRight = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedRightName);
        public static readonly Texture2D SeparatorBackgroundImageDefaultFadedTop = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedTopName);
        public static readonly Texture2D SeparatorBackgroundImageClassicI = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageClassicIName);
        public static readonly Texture2D SeparatorBackgroundImageClassicII = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageClassicIIName);
        public static readonly Texture2D SeparatorBackgroundImageModernI = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageModernIName);
        public static readonly Texture2D SeparatorBackgroundImageModernII = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageModernIIName);
        public static readonly Texture2D SeparatorBackgroundImageModernIII = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageModernIIIName);
        public static readonly Texture2D SeparatorBackgroundImageNeoI = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageNeoIName);
        public static readonly Texture2D SeparatorBackgroundImageNeoII = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageNeoIIName);
        public static readonly Texture2D SeparatorBackgroundImageNextGenI = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageNextGenIName);
        public static readonly Texture2D SeparatorBackgroundImageNextGenII = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageNextGenIIName);
        public static readonly Texture2D SeparatorInspectorIcon = HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorInspectorIconName);

        public static Texture2D SeparatorImageType(HierarchyDesigner_Configurable_Separator.SeparatorImageType separatorImageType)
        {
            switch (separatorImageType)
            {
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedBottom:
                    return SeparatorBackgroundImageDefaultFadedBottom;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedLeft:
                    return SeparatorBackgroundImageDefaultFadedLeft;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedSideways:
                    return SeparatorBackgroundImageDefaultFadedSideways;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedRight:
                    return SeparatorBackgroundImageDefaultFadedRight;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedTop:
                    return SeparatorBackgroundImageDefaultFadedTop;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ClassicI:
                    return SeparatorBackgroundImageClassicI;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ClassicII:
                    return SeparatorBackgroundImageClassicII;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernI:
                    return SeparatorBackgroundImageModernI;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernII:
                    return SeparatorBackgroundImageModernII;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernIII:
                    return SeparatorBackgroundImageModernIII;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.NeoI:
                    return SeparatorBackgroundImageNeoI;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.NeoII:
                    return SeparatorBackgroundImageNeoII;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.NextGenI:
                    return SeparatorBackgroundImageNextGenI;
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.NextGenII:
                    return SeparatorBackgroundImageNextGenII;
                default:
                    return SeparatorBackgroundImageDefault;
            }
        }
        #endregion
    }
}
#endif