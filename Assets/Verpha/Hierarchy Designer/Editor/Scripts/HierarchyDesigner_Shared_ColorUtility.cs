#if UNITY_EDITOR
using System;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_ColorUtility
    {
        #region Properties
        public static readonly Color DefaultDarkThemeSkinColor = HexToColor("#383838");
        public static readonly Color DefaultLightThemeSkinColor = HexToColor("#C8C8C8");
        public static readonly Color DefaultDarkThemeSkinRowColor = HexToColor("#333333");
        public static readonly Color DefaultLightThemeSkinRowColor = HexToColor("#CFCFCF");
        public static readonly Color HighlightedDarkThemeSkinColor = HexToColor("#464646");
        public static readonly Color HighlightedLightThemeSkinColor = HexToColor("#B5B5B5");
        public static readonly Color HighlightedDarkThemeSkinRowColor = HexToColor("#414141");
        public static readonly Color HighlightedLightThemeSkinRowColor = HexToColor("#BCBCBC");
        public static readonly Color HighlightedFocusedDarkThemeSkinColor = HexToColor("#4D4D4D");
        public static readonly Color HighlightedFocusedLightThemeSkinColor = HexToColor("#AEAEAE");
        public static readonly Color HighlightedFocusedDarkThemeSkinRowColor = HexToColor("#484848");
        public static readonly Color HighlightedFocusedLightThemeSkinRowColor = HexToColor("#B5B5B5");
        public static readonly Color SelectedDarkThemeSkinColor = HexToColor("#2D5C8E");
        public static readonly Color SelectedLightThemeSkinColor = HexToColor("#3372B7");
        public static readonly Color SelectedDarkThemeSkinRowColor = HexToColor("#225585");
        public static readonly Color SelectedLightThemeSkinRowColor = HexToColor("#4A82C1");
        public static readonly Color RowColorDarkThemeSkinColor = HexToColor("00000015");
        public static readonly Color RowColorLightThemeSkinColor = HexToColor("FFFFFF20");
        public static readonly Color OuterGUIDarkThemeColor = HexToColor("#282828");
        public static readonly Color OuterGUILightThemeColor = HexToColor("#E1E1E1");
        public static readonly Color InnerGUIDarkThemeColor = HexToColor("#00000064");
        public static readonly Color InnerGUILightThemeColor = HexToColor("#E1E1E1");
        public static readonly Color ContentGUIDarkThemeColor = HexToColor("#232323");
        public static readonly Color ContentGUILightThemeColor = HexToColor("#F0F0F0");
        #endregion

        #region Getters Methods
        public static Color GetDefaultEditorBackgroundColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? DefaultDarkThemeSkinColor : DefaultLightThemeSkinColor;
        }
        public static Color GetDefaultEditorRowBackgroundColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? DefaultDarkThemeSkinRowColor : DefaultLightThemeSkinRowColor;
        }
        public static Color GetHighlightedEditorColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? HighlightedDarkThemeSkinColor : HighlightedLightThemeSkinColor;
        }
        public static Color GetHighlightedEditorRowColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? HighlightedDarkThemeSkinRowColor : HighlightedLightThemeSkinRowColor;
        }
        public static Color GetHighlightedFocusedEditorColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? HighlightedFocusedDarkThemeSkinColor : HighlightedFocusedLightThemeSkinColor;
        }
        public static Color GetHighlightedFocusedEditorRowColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? HighlightedFocusedDarkThemeSkinRowColor : HighlightedFocusedLightThemeSkinRowColor;
        }
        public static Color GetSelectedEditorColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? SelectedDarkThemeSkinColor : SelectedLightThemeSkinColor;
        }
        public static Color GetSelectedEditorRowColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? SelectedDarkThemeSkinRowColor : SelectedLightThemeSkinRowColor;
        }
        public static Color GetRowColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? RowColorDarkThemeSkinColor : RowColorLightThemeSkinColor;
        }
        public static Color GetOuterGUIColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? OuterGUIDarkThemeColor : OuterGUILightThemeColor;
        }
        public static Color GetInnerGUIColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? InnerGUIDarkThemeColor : InnerGUILightThemeColor;
        }
        public static Color GetContentGUIColor()
        {
            return HierarchyDesigner_Manager_Editor.IsProSkin ? ContentGUIDarkThemeColor : ContentGUILightThemeColor;
        }
        #endregion

        #region Conversion Methods
        public static Color HexToColor(string hex)
        {
            try
            {
                hex = hex.Replace("0x", "").Replace("#", "");
                byte a = 255;
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                if (hex.Length == 8)
                {
                    a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                }

                return new Color32(r, g, b, a);
            }
            catch (Exception ex)
            {
                Debug.LogError("Color parsing failed: " + ex.Message);
                return Color.white;
            }
        }

        public static Gradient CreateGradient(GradientMode mode, params (string hexColor, int alpha, float locationPercentage)[] colorAlphaPairs)
        {
            int length = colorAlphaPairs.Length;
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[length];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[length];

            for (int i = 0; i < length; i++)
            {
                float location = colorAlphaPairs[i].locationPercentage / 100f;
                colorKeys[i] = new GradientColorKey(HexToColor(colorAlphaPairs[i].hexColor), location);
                alphaKeys[i] = new GradientAlphaKey(colorAlphaPairs[i].alpha / 255f, location);
            }

            gradient.colorKeys = colorKeys;
            gradient.alphaKeys = alphaKeys;
            gradient.mode = mode;

            return gradient;
        }
        #endregion
    }
}
#endif