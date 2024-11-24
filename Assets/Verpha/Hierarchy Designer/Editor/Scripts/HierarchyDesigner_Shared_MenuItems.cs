#if UNITY_EDITOR
namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Shared_MenuItems
    {
        #region Priority
        public const int LayerZero = 0;
        public const int LayerOne = 11;
        public const int LayerTwo = 22;
        public const int LayerThree = 33;
        public const int LayerFour = 44;
        public const int LayerFive = 55;
        public const int LayerSix = 66;
        public const int LayerSeven = 77;
        public const int LayerEight = 88;
        public const int LayerNine = 99;
        public const int LayerTen = 110;
        public const int LayerEleven = 121;
        public const int LayerTwelve = 132;
        public const int LayerThirteen = 143;
        public const int LayerFourteen = 154;
        public const int LayerFifteen = 165;
        public const int LayerSixteen = 176;
        public const int LayerSeventeen = 187;
        public const int LayerEighteen = 198;
        public const int LayerNineteen = 209;
        public const int LayerTwenty = 220;
        #endregion

        #region Items Name
        public const string Base_HierarchyDesigner = HierarchyDesigner_Shared_Constants.Base_HierarchyDesigner;
        public const string Base_Group = "GameObject/Hierarchy Designer";
        public const string Group_Folders = Base_Group + "/Folders";
        public const string Group_Separators = Base_Group + "/Separators";
        public const string Group_Tools = Base_Group + "/Tools";
        public const string Group_Refresh = Base_Group + "/Refresh";
        public const string Section_Activate = Group_Tools + "/Activate";
        public const string Section_Activate_General = Section_Activate + "/General";
        public const string Section_Activate_Type = Section_Activate + "/Activate by Type";
        public const string Section_Activate_Type_2D = Section_Activate_Type + "/2D Objects";
        public const string Section_Activate_Type_2D_Sprites = Section_Activate_Type_2D + "/Sprites";
        public const string Section_Activate_Type_2D_Physics = Section_Activate_Type_2D + "/Physics";
        public const string Section_Activate_Type_3D = Section_Activate_Type + "/3D Objects";
        public const string Section_Activate_Type_3D_Legacy = Section_Activate_Type_3D + "/Legacy";
        public const string Section_Activate_Type_Audio = Section_Activate_Type + "/Audio";
        public const string Section_Activate_Type_Effects = Section_Activate_Type + "/Effects";
        public const string Section_Activate_Type_Light = Section_Activate_Type + "/Light";
        public const string Section_Activate_Type_Video = Section_Activate_Type + "/Video";
        public const string Section_Activate_Type_UIToolkit = Section_Activate_Type + "/UI Toolkit";
        public const string Section_Activate_Type_UI = Section_Activate_Type + "/UI";
        public const string Section_Activate_Type_UI_Legacy = Section_Activate_Type_UI + "/Legacy";
        public const string Section_Activate_Type_UI_Effects = Section_Activate_Type_UI + "/Effects";
        public const string Section_Deactivate_Type = Section_Activate + "/Deactivate by Type";
        public const string Section_Deactivate_Type_2D = Section_Deactivate_Type + "/2D Objects";
        public const string Section_Deactivate_Type_2D_Sprites = Section_Deactivate_Type_2D + "/Sprites";
        public const string Section_Deactivate_Type_2D_Physics = Section_Deactivate_Type_2D + "/Physics";
        public const string Section_Deactivate_Type_3D = Section_Deactivate_Type + "/3D Objects";
        public const string Section_Deactivate_Type_3D_Legacy = Section_Deactivate_Type_3D + "/Legacy";
        public const string Section_Deactivate_Type_Audio = Section_Deactivate_Type + "/Audio";
        public const string Section_Deactivate_Type_Effects = Section_Deactivate_Type + "/Effects";
        public const string Section_Deactivate_Type_Light = Section_Deactivate_Type + "/Light";
        public const string Section_Deactivate_Type_Video = Section_Deactivate_Type + "/Video";
        public const string Section_Deactivate_Type_UIToolkit = Section_Deactivate_Type + "/UI Toolkit";
        public const string Section_Deactivate_Type_UI = Section_Deactivate_Type + "/UI";
        public const string Section_Deactivate_Type_UI_Legacy = Section_Deactivate_Type_UI + "/Legacy";
        public const string Section_Deactivate_Type_UI_Effects = Section_Deactivate_Type_UI + "/Effects";
        public const string Section_Count = Group_Tools + "/Count";
        public const string Section_Count_General = Section_Count + "/General";
        public const string Section_Count_Type = Section_Count + "/Count by Type";
        public const string Section_Count_Type_2D = Section_Count_Type + "/2D Objects";
        public const string Section_Count_Type_2D_Sprites = Section_Count_Type_2D + "/Sprites";
        public const string Section_Count_Type_2D_Physics = Section_Count_Type_2D + "/Physics";
        public const string Section_Count_Type_3D = Section_Count_Type + "/3D Objects";
        public const string Section_Count_Type_3D_Legacy = Section_Count_Type_3D + "/Legacy";
        public const string Section_Count_Type_Audio = Section_Count_Type + "/Audio";
        public const string Section_Count_Type_Effects = Section_Count_Type + "/Effects";
        public const string Section_Count_Type_Light = Section_Count_Type + "/Light";
        public const string Section_Count_Type_Video = Section_Count_Type + "/Video";
        public const string Section_Count_Type_UIToolkit = Section_Count_Type + "/UI Toolkit";
        public const string Section_Count_Type_UI = Section_Count_Type + "/UI";
        public const string Section_Count_Type_UI_Legacy = Section_Count_Type_UI + "/Legacy";
        public const string Section_Count_Type_UI_Effects = Section_Count_Type_UI + "/Effects";
        public const string Section_Lock = Group_Tools + "/Lock";
        public const string Section_Lock_General = Section_Lock + "/General";
        public const string Section_Lock_Type = Section_Lock + "/Lock by Type";
        public const string Section_Lock_Type_2D = Section_Lock_Type + "/2D Objects";
        public const string Section_Lock_Type_2D_Sprites = Section_Lock_Type_2D + "/Sprites";
        public const string Section_Lock_Type_2D_Physics = Section_Lock_Type_2D + "/Physics";
        public const string Section_Lock_Type_3D = Section_Lock_Type + "/3D Objects";
        public const string Section_Lock_Type_3D_Legacy = Section_Lock_Type_3D + "/Legacy";
        public const string Section_Lock_Type_Audio = Section_Lock_Type + "/Audio";
        public const string Section_Lock_Type_Effects = Section_Lock_Type + "/Effects";
        public const string Section_Lock_Type_Light = Section_Lock_Type + "/Light";
        public const string Section_Lock_Type_Video = Section_Lock_Type + "/Video";
        public const string Section_Lock_Type_UIToolkit = Section_Lock_Type + "/UI Toolkit";
        public const string Section_Lock_Type_UI = Section_Lock_Type + "/UI";
        public const string Section_Lock_Type_UI_Legacy = Section_Lock_Type_UI + "/Legacy";
        public const string Section_Lock_Type_UI_Effects = Section_Lock_Type_UI + "/Effects";
        public const string Section_Unlock_Type = Section_Lock + "/Unlock by Type";
        public const string Section_Unlock_Type_2D = Section_Unlock_Type + "/2D Objects";
        public const string Section_Unlock_Type_2D_Sprites = Section_Unlock_Type_2D + "/Sprites";
        public const string Section_Unlock_Type_2D_Physics = Section_Unlock_Type_2D + "/Physics";
        public const string Section_Unlock_Type_3D = Section_Unlock_Type + "/3D Objects";
        public const string Section_Unlock_Type_3D_Legacy = Section_Unlock_Type_3D + "/Legacy";
        public const string Section_Unlock_Type_Audio = Section_Unlock_Type + "/Audio";
        public const string Section_Unlock_Type_Effects = Section_Unlock_Type + "/Effects";
        public const string Section_Unlock_Type_Light = Section_Unlock_Type + "/Light";
        public const string Section_Unlock_Type_Video = Section_Unlock_Type + "/Video";
        public const string Section_Unlock_Type_UIToolkit = Section_Unlock_Type + "/UI Toolkit";
        public const string Section_Unlock_Type_UI = Section_Unlock_Type + "/UI";
        public const string Section_Unlock_Type_UI_Legacy = Section_Unlock_Type_UI + "/Legacy";
        public const string Section_Unlock_Type_UI_Effects = Section_Unlock_Type_UI + "/Effects";
        public const string Section_Rename = Group_Tools + "/Rename";
        public const string Section_Select = Group_Tools + "/Select";
        public const string Section_Select_General = Section_Select + "/General";
        public const string Section_Select_Type = Section_Select + "/Select by Type";
        public const string Section_Select_Type_2D = Section_Select_Type + "/2D Objects";
        public const string Section_Select_Type_2D_Sprites = Section_Select_Type_2D + "/Sprites";
        public const string Section_Select_Type_2D_Physics = Section_Select_Type_2D + "/Physics";
        public const string Section_Select_Type_3D = Section_Select_Type + "/3D Objects";
        public const string Section_Select_Type_3D_Legacy = Section_Select_Type_3D + "/Legacy";
        public const string Section_Select_Type_Audio = Section_Select_Type + "/Audio";
        public const string Section_Select_Type_Effects = Section_Select_Type + "/Effects";
        public const string Section_Select_Type_Light = Section_Select_Type + "/Light";
        public const string Section_Select_Type_Video = Section_Select_Type + "/Video";
        public const string Section_Select_Type_UIToolkit = Section_Select_Type + "/UI Toolkit";
        public const string Section_Select_Type_UI = Section_Select_Type + "/UI";
        public const string Section_Select_Type_UI_Legacy = Section_Select_Type_UI + "/Legacy";
        public const string Section_Select_Type_UI_Effects = Section_Select_Type_UI + "/Effects";
        public const string Section_Sort = Group_Tools + "/Sort";
        #endregion
    }
}
#endif