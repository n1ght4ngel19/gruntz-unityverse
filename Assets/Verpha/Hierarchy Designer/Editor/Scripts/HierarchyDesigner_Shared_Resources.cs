#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_Resources
    {
        #region Properties
        #region General
        private static readonly string defaultTextureName = "Hierarchy Designer Default Texture";
        private static readonly string lockIconName = "Hierarchy Designer Lock Icon";
        #endregion
        #region Tree Branches
        private static readonly string treeBranchIconIName = "Hierarchy Designer Tree Branch Icon I";
        private static readonly string treeBranchIconLName = "Hierarchy Designer Tree Branch Icon L";
        private static readonly string treeBranchIconTName = "Hierarchy Designer Tree Branch Icon T";
        private static readonly string treeBranchIconTerminalBudName = "Hierarchy Designer Tree Branch Icon Terminal Bud";
        #endregion
        #region Folder Image Types
        private static readonly string folderDefaultIconName = "Hierarchy Designer Folder Icon Default";
        private static readonly string folderDefaultOutlineIconName = "Hierarchy Designer Folder Icon Default Outline";
        private static readonly string folderDefaultOutline2XIconName = "Hierarchy Designer Folder Icon Default Outline 2x";
        private static readonly string folderModernIIconName = "Hierarchy Designer Folder Icon Modern I";
        private static readonly string folderInspectorIconName = "Hierarchy Designer Folder Icon Inspector";
        #endregion
        #region Separator Image Types
        private static readonly string separatorBackgroundImageDefaultName = "Hierarchy Designer Separator Background Image Default";
        private static readonly string separatorBackgroundImageDefaultFadedBottomName = "Hierarchy Designer Separator Background Image Default Faded Bottom";
        private static readonly string separatorBackgroundImageDefaultFadedLeftName = "Hierarchy Designer Separator Background Image Default Faded Left";
        private static readonly string separatorBackgroundImageDefaultFadedLeftRightName = "Hierarchy Designer Separator Background Image Default Faded Left Right";
        private static readonly string separatorBackgroundImageDefaultFadedRightName = "Hierarchy Designer Separator Background Image Default Faded Right";
        private static readonly string separatorBackgroundImageDefaultFadedTopName = "Hierarchy Designer Separator Background Image Default Faded Top";
        private static readonly string separatorBackgroundImageModernIName = "Hierarchy Designer Separator Background Image Modern I";
        private static readonly string separatorBackgroundImageModernIIName = "Hierarchy Designer Separator Background Image Modern II";
        private static readonly string separatorBackgroundImageModernIIIName = "Hierarchy Designer Separator Background Image Modern III";
        private static readonly string separatorInspectorIconName = "Hierarchy Designer Separator Icon Inspector";
        #endregion
        #endregion

        #region Getters Methods
        public static Texture2D DefaultTexture { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(defaultTextureName); } }
        public static Texture2D LockIcon { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(lockIconName); } }
        public static Texture2D TreeBranchIcon_I { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconIName); } }
        public static Texture2D TreeBranchIcon_L { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconLName); } }
        public static Texture2D TreeBranchIcon_T { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconTName); } }
        public static Texture2D TreeBranchIcon_TerminalBud { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(treeBranchIconTerminalBudName); } }
        public static Texture2D FolderImageType(HierarchyDesigner_Configurable_Folder.FolderImageType folderImageType)
        {
            switch (folderImageType)
            {
                case HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderDefaultOutlineIconName);
                case HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline2X:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderDefaultOutline2XIconName);
                case HierarchyDesigner_Configurable_Folder.FolderImageType.ModernI:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderModernIIconName);
                default:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderDefaultIconName);
            }
        }
        public static Texture2D FolderInspectorIcon { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(folderInspectorIconName); } }
        public static Texture2D SeparatorImageType(HierarchyDesigner_Configurable_Separator.SeparatorImageType separatorImageType)
        {
            switch (separatorImageType)
            {
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedBottom:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedBottomName);
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedLeft:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedLeftName);
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedLeftAndRight:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedLeftRightName);
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedRight:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedRightName);
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedTop:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultFadedTopName);
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernI:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageModernIName);
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernII:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageModernIIName);
                case HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernIII:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageModernIIIName);
                default:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorBackgroundImageDefaultName);
            }
        }
        public static Texture2D SeparatorInspectorIcon { get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(separatorInspectorIconName); } }
        #endregion
    }
}
#endif