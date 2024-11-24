#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Shared_GUI
    {
        #region Properties
        #region Consts
        private const float extraLabelWidthSpacing = 20f;
        private static readonly Color clearColor = new(0f, 0f, 0f, 0f);
        #endregion
        #region GUIStyles
        private static GUIStyle inspectorContentGUIStyleGUIStyle = null;
        private static GUIStyle inspectorMessageItalicGUIStyle = null;
        private static GUIStyle inspectorMessageBoldGUIStyle = null;
        private static GUIStyle inactiveLabelGUIStyle = null;
        #endregion
        #region Cache
        private static GUIStyle cachedLeftPanelFixed;
        private static GUIStyle cachedLeftPanel;
        private static GUIStyle cachedTitleLabel;
        private static GUIStyle cachedVersionLabel;
        private static GUIStyle cachedRightPanel;
        private static GUIStyle cachedCategoryLabel;
        private static GUIStyle cachedPrimaryButtonStyle;
        private static GUIStyle cachedSecondaryButtonStyle;
        private static GUIStyle cachedContentPanel;
        private static GUIStyle cachedContentLabel;
        private static GUIStyle cachedFieldsLabel;
        private static GUIStyle cachedUnassignedLabel;
        private static GUIStyle cachedMessageLabel;
        private static readonly Dictionary<Color, Texture2D> textureCache = new();
        #endregion
        #endregion

        #region GUIStyles
        public static GUIStyle InspectorContentGUIStyle
        {
            get
            {
                if (inspectorContentGUIStyleGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorContentGUIStyleGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 14,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft
                        };
                    }
                }
                return inspectorContentGUIStyleGUIStyle;
            }
        }

        public static GUIStyle InspectorMessageBoldGUIStyle
        {
            get
            {
                if (inspectorMessageBoldGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorMessageBoldGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                            fontStyle = FontStyle.Bold,
                        };
                    }
                }
                return inspectorMessageBoldGUIStyle;
            }
        }

        public static GUIStyle InspectorMessageItalicGUIStyle
        {
            get
            {
                if (inspectorMessageItalicGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorMessageItalicGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                            fontStyle = FontStyle.Italic,
                        };
                    }
                }
                return inspectorMessageItalicGUIStyle;
            }
        }

        public static GUIStyle InactiveLabelGUIStyle
        {
            get
            {
                if (inactiveLabelGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inactiveLabelGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                        };
                        Color textColor = inactiveLabelGUIStyle.normal.textColor;
                        textColor.a = 0.5f;
                        inactiveLabelGUIStyle.normal.textColor = textColor;
                    }
                }
                return inactiveLabelGUIStyle;
            }
        }
        #endregion

        #region Methods
        public static void GetHierarchyDesignerGUIStyles(out GUIStyle leftPanelFixed, out GUIStyle leftPanel, out GUIStyle titleLabel, out GUIStyle versionLabel, out GUIStyle rightPanel, out GUIStyle categoryLabel, out GUIStyle primaryButtonStyle, out GUIStyle secondaryButtonStyle, out GUIStyle contentPanel, out GUIStyle contentLabel, out GUIStyle fieldsLabel, out GUIStyle unassignedLabel, out GUIStyle messageLabel)
        {
            if (cachedLeftPanelFixed == null)
            {
                cachedLeftPanelFixed = new GUIStyle
                {
                    name = "Left Panel Fixed",
                    normal = new GUIStyleState
                    {
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorI()),
                    },
                    border = new RectOffset(4, 4, 4, 4),
                    padding = new RectOffset(4, 4, 10, 4),
                    margin = new RectOffset(4, 4, 4, 4),
                    overflow = new RectOffset(2, 2, 2, 2),
                    fixedWidth = 150,
                    stretchWidth = true,
                    stretchHeight = true,
                };
            }

            if (cachedLeftPanel == null)
            {
                cachedLeftPanel = new GUIStyle
                {
                    name = "Left Panel",
                    normal = new GUIStyleState
                    {
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorI()),
                    },
                    border = new RectOffset(4, 4, 4, 4),
                    padding = new RectOffset(1, 4, 10, 4),
                    margin = new RectOffset(4, 4, 4, 4),
                    overflow = new RectOffset(2, 2, 2, 2),
                    fixedWidth = 32,
                    stretchWidth = true,
                    stretchHeight = true,
                };
            }

            if (cachedTitleLabel == null)
            {
                cachedTitleLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Title Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                    },
                    padding = new RectOffset(3, 3, -2, 0),
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    fontSize = 20,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.UpperLeft,
                    wordWrap = true,
                    clipping = TextClipping.Overflow,
                    richText = true,
                    fixedHeight = 64,
                };
            }

            if (cachedVersionLabel == null)
            {
                cachedVersionLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Version Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.HexToColor("FFFFFF80"),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    fontSize = 11,
                    fontStyle = FontStyle.Italic,
                    alignment = TextAnchor.LowerLeft,
                    wordWrap = true,
                    clipping = TextClipping.Overflow,
                    richText = true,
                    stretchHeight = true,
                };
            }

            if (cachedRightPanel == null)
            {
                cachedRightPanel = new GUIStyle
                {
                    name = "Right Panel",
                    normal = new GUIStyleState
                    {
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorI()),
                    },
                    border = new RectOffset(4, 4, 4, 4),
                    padding = new RectOffset(4, 4, 4, 4),
                    margin = new RectOffset(4, 4, 4, 4),
                    overflow = new RectOffset(2, 2, 2, 2),
                    stretchWidth = true,
                    stretchHeight = true,
                };
            }

            if (cachedCategoryLabel == null)
            {
                cachedCategoryLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Category Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorIII()),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    fontSize = 21,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleCenter,
                    contentOffset = new Vector2(0, 2),
                    wordWrap = true,
                    clipping = TextClipping.Overflow,
                    richText = true,
                    fixedHeight = 32
                };
            }

            if (cachedPrimaryButtonStyle == null)
            {
                cachedPrimaryButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    name = "Primary Button Style",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    hover = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorHighlight(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    active = new GUIStyleState
                    {
                        textColor = Color.gray,
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    border = new RectOffset(2, 2, 2, 2),
                    padding = new RectOffset(0, 0, 0, 0),
                    fontSize = 13,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                    stretchWidth = true,
                };
            }

            if (cachedSecondaryButtonStyle == null)
            {
                cachedSecondaryButtonStyle = new GUIStyle(GUI.skin.button)
                {
                    name = "Secondary Button Style",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    hover = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorHighlight(),
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    active = new GUIStyleState
                    {
                        textColor = Color.gray,
                        background = GetOrCreateTexture(2, 2, clearColor)
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    border = new RectOffset(2, 2, 2, 2),
                    padding = new RectOffset(14, 11, 2, 2),
                    fontSize = 11,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                };
            }

            if(cachedContentPanel == null)
            {
                cachedContentPanel = new GUIStyle
                {
                    name = "Content Panel",
                    normal = new GUIStyleState
                    {
                        background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorII()),
                    },
                    border = new RectOffset(2, 2, 2, 2),
                    padding = new RectOffset(2, 2, 2, 2),
                    margin = new RectOffset(2, 2, 4, 2),
                    overflow = new RectOffset(2, 2, 2, 2),
                    stretchWidth = true,
                    stretchHeight = false,
                };
            }

            if (cachedContentLabel == null)
            {
                cachedContentLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Content Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorHighlight(),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFontBold,
                    fontSize = 16,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Overflow,
                    richText = true,
                };
            }

            if (cachedFieldsLabel == null)
            {
                cachedFieldsLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Fields Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    fontSize = 12,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Overflow,
                    richText = true,
                };
            }

            if (cachedUnassignedLabel == null)
            {
                cachedUnassignedLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Unassigned Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.HexToColor("B4B4B4"),
                    },
                    font = HierarchyDesigner_Shared_Resources.DefaultFont,
                    fontSize = 12,
                    fontStyle = FontStyle.Italic,
                    alignment = TextAnchor.MiddleLeft,
                    clipping = TextClipping.Overflow,
                };
            }

            if (cachedMessageLabel == null)
            {
                cachedMessageLabel = new GUIStyle(EditorStyles.label)
                {
                    name = "Message Label",
                    normal = new GUIStyleState
                    {
                        textColor = HierarchyDesigner_Shared_ColorUtility.GetHierarchyDesignerColorText(),
                    },
                    fontSize = 12,
                    fontStyle = FontStyle.Normal,
                    alignment = TextAnchor.UpperLeft,
                    clipping = TextClipping.Overflow,
                    wordWrap = true,
                    richText = true
                };
            }

            leftPanelFixed = cachedLeftPanelFixed;
            leftPanel = cachedLeftPanel;
            titleLabel = cachedTitleLabel;
            versionLabel = cachedVersionLabel;
            rightPanel = cachedRightPanel;
            categoryLabel = cachedCategoryLabel;
            primaryButtonStyle = cachedPrimaryButtonStyle;
            secondaryButtonStyle = cachedSecondaryButtonStyle;
            contentPanel = cachedContentPanel;
            contentLabel = cachedContentLabel;
            fieldsLabel = cachedFieldsLabel;
            unassignedLabel = cachedUnassignedLabel;
            messageLabel = cachedMessageLabel;
        }

        public static GUIContent GetLeftPanelButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconMenu;
            return new GUIContent(icon);
        }

        public static GUIContent GetHomeButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconHome;
            return new GUIContent(icon);
        }

        public static GUIContent GetFolderButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconFolder;
            return new GUIContent(icon);
        }

        public static GUIContent GetSeparatorButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon;
            return new GUIContent(icon);
        }

        public static GUIContent GetToolsButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconTools;
            return new GUIContent(icon);
        }

        public static GUIContent GetPresetsButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconPresets;
            return new GUIContent(icon);
        }

        public static GUIContent GetGeneralSettingsButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconGeneralSettings;
            return new GUIContent(icon);
        }

        public static GUIContent GetDesignSettingsButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconDesignSettings;
            return new GUIContent(icon);
        }

        public static GUIContent GetShortcutSettingsButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconShortcutSettings;
            return new GUIContent(icon);
        }

        public static GUIContent GetAdvancedSettingsButtonContent()
        {
            Texture2D icon = HierarchyDesigner_Shared_Resources.IconAdvancedSettings;
            return new GUIContent(icon);
        }

        public static float CalculateMaxLabelWidth(IEnumerable<string> names)
        {
            float labelWidth = 0;
            foreach (string name in names)
            {
                GUIContent content = new GUIContent(name);
                Vector2 size = cachedFieldsLabel.CalcSize(content);
                if (size.x > labelWidth) labelWidth = size.x;
            }
            return labelWidth + extraLabelWidthSpacing;
        }

        public static float CalculateMaxLabelWidth(Transform parent)
        {
            float maxWidth = 0;
            GatherChildNamesAndCalculateMaxWidth(parent, ref maxWidth);
            return maxWidth + 18f;
        }
        #endregion

        #region GUILayout
        public static bool DrawToggle(string label, float labelWidth, bool currentValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            bool newValue = EditorGUILayout.Toggle(currentValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static T DrawEnumPopup<T>(string label, float labelWidth, T selectedValue) where T : Enum
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            T newValue = (T)EditorGUILayout.EnumPopup(selectedValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static int DrawMaskField(string label, float labelWidth, int mask, string[] options)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            int newMask = EditorGUILayout.MaskField(mask, options);
            EditorGUILayout.EndHorizontal();
            return newMask;
        }

        public static float DrawSlider(string label, float labelWidth, float value, float leftValue, float rightValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            float newValue = EditorGUILayout.Slider(value, leftValue, rightValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static int DrawIntSlider(string label, float labelWidth, int value, int leftValue, int rightValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            int newValue = EditorGUILayout.IntSlider(value, leftValue, rightValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static Color DrawColorField(string label, float labelWidth, Color colorValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            Color newColorValue = EditorGUILayout.ColorField(colorValue);
            EditorGUILayout.EndHorizontal();
            return newColorValue;
        }

        public static Gradient DrawGradientField(string label, float labelWidth, Gradient gradientValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            Gradient newGradientValue = EditorGUILayout.GradientField(gradientValue);
            EditorGUILayout.EndHorizontal();
            return newGradientValue;
        }

        public static string DrawTextField(string label, float labelWidth, string textValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            string newTextValue = EditorGUILayout.TextField(textValue);
            EditorGUILayout.EndHorizontal();
            return newTextValue;
        }

        public static int DrawDelayedIntField(string label, float labelWidth, int intValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, cachedFieldsLabel, GUILayout.Width(labelWidth));
            int newIntValue = EditorGUILayout.DelayedIntField(intValue);
            EditorGUILayout.EndHorizontal();
            return newIntValue;
        }
        #endregion

        #region Operations
        private static Texture2D GetOrCreateTexture(int width, int height, Color color)
        {
            if (textureCache.TryGetValue(color, out Texture2D existingTexture) && existingTexture != null)
            {
                return existingTexture;
            }

            Texture2D newTexture = new Texture2D(width, height);
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }

            newTexture.SetPixels(pix);
            newTexture.Apply();
            newTexture.hideFlags = HideFlags.DontSave;

            textureCache[color] = newTexture;
            return newTexture;
        }

        private static void GatherChildNamesAndCalculateMaxWidth(Transform parent, ref float maxWidth)
        {
            GUIStyle labelStyle = GUI.skin.label;
            foreach (Transform child in parent)
            {
                GUIContent content = new GUIContent(child.name);
                Vector2 size = labelStyle.CalcSize(content);
                if (size.x > maxWidth) maxWidth = size.x;
                GatherChildNamesAndCalculateMaxWidth(child, ref maxWidth);
            }
        }
        #endregion
    }
}
#endif