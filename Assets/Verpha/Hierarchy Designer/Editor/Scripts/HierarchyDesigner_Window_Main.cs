#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Window_Main : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 buttonsScroll;
        private Vector2 buttonsCollapsedScroll;
        private GUIStyle leftPanelFixed;
        private GUIStyle leftPanel;
        private GUIStyle titleLabel;
        private GUIStyle versionLabel;
        private GUIStyle rightPanel;
        private GUIStyle categoryLabel;
        private GUIStyle primaryButtonStyle;
        private GUIStyle secondaryButtonStyle;
        private GUIStyle contentPanel;
        private GUIStyle contentLabel;
        private GUIStyle fieldsLabel;
        private GUIStyle unassignedLabel;
        private GUIStyle messageLabel;
        #endregion
        #region Consts
        private const int iconButtonsSize = 24;
        private const int iconSpacing = 4;
        private const int defaultMarginSpacing = 2;
        private const int defaultSpacing = 8;
        private const int primaryButtonsHeight = 30;
        private const int secondaryButtonsHeight = 25;
        private const float lineDivisorWidth = 130f;
        private const float miniLineDivisorWidth = 20f;
        private const float lineDivisorHeight = 1f;
        private const float lineDivisorSpace = 12;
        private const string collapsedSign = "→";
        private const string foldoutSign = "↓";
        private const int contentLabelSpacing = 4;
        private readonly int[] fontSizeOptions = new int[15];
        private const float moveItemInListButtonWidth = 25;
        private const float createButtonWidth = 52;
        private const float removeButtonWidth = 60;
        #endregion
        #region States
        public enum CurrentWindow { Home, Folders, Separators, Tools, Presets, GeneralSettings, DesignSettings, ShortcutSettings, AdvancedSettings }
        private static CurrentWindow currentWindow;
        public static void SetCurrentWindow(CurrentWindow desiredWindow) { currentWindow = desiredWindow; }
        #endregion
        #region Conditions
        private static bool isLeftPanelCollapsed = false;
        private static bool utilitiesFoldout = false;
        private static bool configurationsFoldout = false;
        #endregion
        #region Home
        private Vector2 homeMainScroll;
        private string patchNotesContent = string.Empty;
        private string updateBoardContent = string.Empty;
        private const string introMessage = "<b>Welcome to the Update Board!</b>\nThis is a real-time \"message center\" for the latest news, designed to keep you always up-to-date with Hierarchy Designer's updates.\n\n";
        private Vector2 homeUpdateBoardScroll;
        private Vector2 homePatchNotesScroll;
        #endregion
        #region Folder
        private Vector2 folderMainScroll;
        private Vector2 foldersListScroll;
        private const float folderCreationLabelWidth = 85;
        private Dictionary<string, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData> tempFolders;
        private List<string> foldersOrder;
        private string newFolderName = "";
        private Color newFolderTextColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultTextColor;
        private int newFolderFontSize = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontSize;
        private FontStyle newFolderFontStyle = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontStyle;
        private Color newFolderIconColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageColor;
        private HierarchyDesigner_Configurable_Folder.FolderImageType newFolderImageType = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageType;
        private bool folderHasModifiedChanges = false;
        private Color tempFolderGlobalTextColor = Color.white;
        private int tempFolderGlobalFontSize = 12;
        private FontStyle tempFolderGlobalFontStyle = FontStyle.Normal;
        private Color tempFolderGlobalIconColor = Color.white;
        private HierarchyDesigner_Configurable_Folder.FolderImageType tempGlobalFolderImageType = HierarchyDesigner_Configurable_Folder.FolderImageType.Default;
        #endregion
        #region Separator
        private Vector2 separatorMainScroll;
        private Vector2 separatorsListScroll;
        private const float separatorCreationLabelWidth = 160;
        private Dictionary<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> tempSeparators;
        private List<string> separatorsOrder;
        private bool separatorHasModifiedChanges = false;
        private string newSeparatorName = "";
        private Color newSeparatorTextColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextColor;
        private bool newSeparatorIsGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultIsGradientBackground;
        private Color newSeparatorBackgroundColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundColor;
        private Gradient newSeparatorBackgroundGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundGradient;
        private int newSeparatorFontSize = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontSize;
        private FontStyle newSeparatorFontStyle = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontStyle;
        private TextAnchor newSeparatorTextAnchor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextAnchor;
        private HierarchyDesigner_Configurable_Separator.SeparatorImageType newSeparatorImageType = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultImageType;
        private Color tempSeparatorGlobalTextColor = Color.white;
        private bool tempSeparatorGlobalIsGradient = false;
        private Color tempSeparatorGlobalBackgroundColor = Color.gray;
        private Gradient tempSeparatorGlobalBackgroundGradient = new Gradient();
        private int tempSeparatorGlobalFontSize = 12;
        private FontStyle tempSeparatorGlobalFontStyle = FontStyle.Normal;
        private TextAnchor tempSeparatorGlobalTextAnchor = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Configurable_Separator.SeparatorImageType tempSeparatorGlobalImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
        #endregion
        #region Tools
        private Vector2 toolsMainScroll;
        private const float labelWidth = 80;
        private HierarchyDesigner_Attribute_Tools selectedCategory = HierarchyDesigner_Attribute_Tools.Activate;
        private int selectedActionIndex = 0;
        private List<string> availableActionNames = new List<string>();
        private List<MethodInfo> availableActionMethods = new List<MethodInfo>();
        private static Dictionary<HierarchyDesigner_Attribute_Tools, List<(string Name, MethodInfo Method)>> cachedActions = new Dictionary<HierarchyDesigner_Attribute_Tools, List<(string Name, MethodInfo Method)>>();
        private static bool cacheInitialized = false;
        #endregion
        #region Presets
        private Vector2 presetsMainScroll;
        private const float presetslabelWidth = 135;
        private const float presetsToggleLabelWidth = 165;
        private int selectedPresetIndex = 0;
        private string[] presetNames;
        private bool applyToFolders = true;
        private bool applyToSeparators = true;
        private bool applyToTag = true;
        private bool applyToLayer = true;
        private bool applyToTree = true;
        private bool applyToLines = true;
        private bool applyToFolderDefaultValues = true;
        private bool applyToSeparatorDefaultValues = true;
        private bool applyToLock = true;
        #endregion
        #region General Settings
        private Vector2 generalSettingsMainScroll;
        private const float enumPopupLabelWidth = 145;
        private const float generalSettingstoggleLabelWidth = 285;
        private const float maskFieldLabelWidth = 110;
        private HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode tempLayoutMode;
        private HierarchyDesigner_Configurable_GeneralSettings.HierarchyTreeMode tempTreeMode;
        private bool tempEnableGameObjectMainIcon;
        private bool tempEnableGameObjectComponentIcons;
        private bool tempEnableHierarchyTree;
        private bool tempEnableGameObjectTag;
        private bool tempEnableGameObjectLayer;
        private bool tempEnableHierarchyRows;
        private bool tempEnableHierarchyLines;
        private bool tempEnableHierarchyButtons;
        private bool tempEnableMajorShortcuts;
        private bool tempDisableHierarchyDesignerDuringPlayMode;
        private bool tempExcludeFolderProperties;
        private bool tempExcludeTransformForGameObjectComponentIcons;
        private bool tempExcludeCanvasRendererForGameObjectComponentIcons;
        private int tempMaximumComponentIconsAmount;
        private List<string> tempExcludedTags;
        private List<string> tempExcludedLayers;
        private static bool generalSettingsHasModifiedChanges = false;
        #endregion
        #region Design Settings
        private Vector2 designSettingsMainScroll;
        private const float designSettingslabelWidth = 260;
        private float tempComponentIconsSize;
        private int tempComponentIconsOffset;
        private float tempComponentIconsSpacing;
        private Color tempHierarchyTreeColor;
        private HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType tempTreeBranchImageType_I;
        private HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType tempTreeBranchImageType_L;
        private HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType tempTreeBranchImageType_T;
        private HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType tempTreeBranchImageType_TerminalBud;
        private Color tempTagColor;
        private TextAnchor tempTagTextAnchor;
        private FontStyle tempTagFontStyle;
        private int tempTagFontSize;
        private Color tempLayerColor;
        private TextAnchor tempLayerTextAnchor;
        private FontStyle tempLayerFontStyle;
        private int tempLayerFontSize;
        private int tempTagLayerOffset;
        private int tempTagLayerSpacing;
        private Color tempHierarchyLineColor;
        private int tempHierarchyLineThickness;
        #region Folder
        private Color tempFolderDefaultTextColor;
        private int tempFolderDefaultFontSize;
        private FontStyle tempFolderDefaultFontStyle;
        private Color tempFolderDefaultImageColor;
        private HierarchyDesigner_Configurable_Folder.FolderImageType tempFolderDefaultImageType;
        #endregion
        #region Separator
        private Color tempSeparatorDefaultTextColor;
        private bool tempSeparatorDefaultIsGradientBackground;
        private Color tempSeparatorDefaultBackgroundColor;
        private Gradient tempSeparatorDefaultBackgroundGradient;
        private int tempSeparatorDefaultFontSize;
        private FontStyle tempSeparatorDefaultFontStyle;
        private TextAnchor tempSeparatorDefaultTextAnchor;
        private HierarchyDesigner_Configurable_Separator.SeparatorImageType tempSeparatorDefaultImageType;
        private int tempSeparatorLeftSideTextAnchorOffset;
        private int tempSeparatorRightSideTextAnchorOffset;
        #endregion
        #region Lock Label
        private Color tempLockColor;
        private TextAnchor tempLockTextAnchor;
        private FontStyle tempLockFontStyle;
        private int tempLockFontSize;
        #endregion
        private static bool designSettingsHasModifiedChanges = false;
        #endregion
        #region Shortcut Settings
        private Vector2 shortcutSettingsMainScroll;
        private Vector2 minorShortcutSettingsScroll;
        private const float majorShortcutEnumToggleLabelWidth = 270;
        private const float minorShortcutCommandLabelWidth = 240;
        private const float minorShortcutLabelWidth = 300;
        private List<string> minorShortcutIdentifiers = new List<string>
        {
            "Hierarchy Designer/Open Hierarchy Designer Window",
            "Hierarchy Designer/Open Folder Panel",
            "Hierarchy Designer/Open Separator Panel",
            "Hierarchy Designer/Open Tools Panel",
            "Hierarchy Designer/Open Presets Panel",
            "Hierarchy Designer/Open General Settings Panel",
            "Hierarchy Designer/Open Design Settings Panel",
            "Hierarchy Designer/Open Shortcut Settings Panel",
            "Hierarchy Designer/Open Advanced Settings Panel",
            "Hierarchy Designer/Open Rename Tool Window",
            "Hierarchy Designer/Create All Folders",
            "Hierarchy Designer/Create Default Folder",
            "Hierarchy Designer/Create Missing Folders",
            "Hierarchy Designer/Create All Separators",
            "Hierarchy Designer/Create Default Separator",
            "Hierarchy Designer/Create Missing Separators",
            "Hierarchy Designer/Refresh All GameObjects' Data",
            "Hierarchy Designer/Refresh Selected GameObject's Data",
            "Hierarchy Designer/Refresh Selected Main Icon",
            "Hierarchy Designer/Refresh Selected Component Icons",
            "Hierarchy Designer/Refresh Selected Hierarchy Tree Icon",
            "Hierarchy Designer/Refresh Selected Tag",
            "Hierarchy Designer/Refresh Selected Layer",
        };
        private KeyCode tempToggleGameObjectActiveStateKeyCode;
        private KeyCode tempToggleLockStateKeyCode;
        private KeyCode tempChangeTagLayerKeyCode;
        private KeyCode tempRenameSelectedGameObjectsKeyCode;
        private static bool shortcutSettingsHasModifiedChanges = false;
        #endregion
        #region Advanced Settings
        private Vector2 advancedSettingsMainScroll;
        private const float advancedSettingsEnumPopupLabelWidth = 205;
        private const float advancedSettingsToggleLabelWidth = 360;
        private HierarchyDesigner_Configurable_AdvancedSettings.HierarchyDesignerLocation tempHierarchyLocation;
        private HierarchyDesigner_Configurable_AdvancedSettings.UpdateMode tempMainIconUpdateMode;
        private HierarchyDesigner_Configurable_AdvancedSettings.UpdateMode tempComponentsIconsUpdateMode;
        private HierarchyDesigner_Configurable_AdvancedSettings.UpdateMode tempHierarchyTreeUpdateMode;
        private HierarchyDesigner_Configurable_AdvancedSettings.UpdateMode tempTagUpdateMode;
        private HierarchyDesigner_Configurable_AdvancedSettings.UpdateMode tempLayerUpdateMode;
        private bool tempEnableDynamicBackgroundForGameObjectMainIcon;
        private bool tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
        private bool tempEnableCustomizationForGameObjectComponentIcons;
        private bool tempEnableTooltipOnComponentIconHovered;
        private bool tempEnableActiveStateEffectForComponentIcons;
        private bool tempDisableComponentIconsForInactiveGameObjects;
        private bool tempIncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder;
        private bool tempIncludeBackgroundImageForGradientBackground;
        private bool tempExcludeFoldersFromCountSelectToolCalculations;
        private bool tempExcludeSeparatorsFromCountSelectToolCalculations;
        private static bool advancedSettingsHasModifiedChanges = false;
        #endregion
        #endregion

        #region Window
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Base_HierarchyDesigner, false, HierarchyDesigner_Shared_MenuItems.LayerTwelve)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_Main editorWindow = GetWindow<HierarchyDesigner_Window_Main>("Hierarchy Designer");
            editorWindow.minSize = new Vector2(500, 300);
        }
        #endregion

        #region Initialization
        public static void Initialize()
        {
            // Load the asset file
            isLeftPanelCollapsed = HierarchyDesigner_Manager_State.instance.isLeftPanelCollapsed;
            utilitiesFoldout = HierarchyDesigner_Manager_State.instance.utilitiesFoldout;
            configurationsFoldout = HierarchyDesigner_Manager_State.instance.configurationsFoldout;
            currentWindow = HierarchyDesigner_Manager_State.instance.currentWindow;
        }

        private void OnEnable()
        {
            InitializeFontSizeOptions();
            LoadPatchNotes();
            LoadFolderData();
            LoadSeparatorData();
            LoadTools();
            LoadPresets();
            LoadGeneralSettingsData();
            LoadDesignSettingsData();
            LoadShortcutSettingsData();
            LoadAdvancedSettingsData();
            updateBoardContent = introMessage;
            _ = FetchAndSetUpdateBoardContent();
        }

        private void InitializeFontSizeOptions()
        {
            for (int i = 0; i < fontSizeOptions.Length; i++)
            {
                fontSizeOptions[i] = 7 + i;
            }
        }
        #endregion

        private void OnGUI()
        {
            #region Header
            HierarchyDesigner_Shared_GUI.GetHierarchyDesignerGUIStyles(out leftPanelFixed, out leftPanel, out titleLabel, out versionLabel, out rightPanel, out categoryLabel, out primaryButtonStyle, out secondaryButtonStyle, out contentPanel, out contentLabel, out fieldsLabel, out unassignedLabel, out messageLabel);
            EditorGUILayout.BeginHorizontal();
            #endregion

            #region Body
            #region Left Panel
            EditorGUILayout.BeginVertical(isLeftPanelCollapsed ? leftPanel : leftPanelFixed);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetLeftPanelButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
            {
                isLeftPanelCollapsed = !isLeftPanelCollapsed;
                HierarchyDesigner_Manager_State.instance.isLeftPanelCollapsed = isLeftPanelCollapsed;
                //HierarchyDesigner_Manager_State.instance.Save();
            }
            #if UNITY_6000_0_OR_NEWER
            EditorGUILayout.LabelField(isLeftPanelCollapsed ? "" : $"Hierarchy\n<size=80%><color={(HierarchyDesigner_Manager_Editor.IsProSkin ? "#FFEB5D" : "#5E70FF")}>Designer</color></size>", titleLabel);
            #else
            EditorGUILayout.LabelField(isLeftPanelCollapsed ? "" : $"Hierarchy\n<size=20%><color={(HierarchyDesigner_Manager_Editor.IsProSkin ? "#FFEB5D" : "#5E70FF")}>Designer</color></size>", titleLabel);
            #endif
            EditorGUILayout.EndHorizontal();

            if (!isLeftPanelCollapsed)
            {
                EditorGUILayout.Space(40);
                buttonsScroll = GUILayout.BeginScrollView(buttonsScroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

                if (GUILayout.Button("Home", primaryButtonStyle, GUILayout.Height(primaryButtonsHeight)))
                {
                    currentWindow = CurrentWindow.Home;
                }

                DrawLineDivisor();

                if (GUILayout.Button("Folders", primaryButtonStyle, GUILayout.Height(primaryButtonsHeight)))
                {
                    currentWindow = CurrentWindow.Folders;
                }

                if (GUILayout.Button("Separators", primaryButtonStyle, GUILayout.Height(primaryButtonsHeight)))
                {
                    currentWindow = CurrentWindow.Separators;
                }

                DrawLineDivisor();

                if (GUILayout.Button($"{(utilitiesFoldout ? foldoutSign : collapsedSign)} Utilities", primaryButtonStyle, GUILayout.Height(primaryButtonsHeight)))
                {
                    utilitiesFoldout = !utilitiesFoldout;
                    HierarchyDesigner_Manager_State.instance.utilitiesFoldout = utilitiesFoldout;
                    //HierarchyDesigner_Manager_State.instance.Save();
                }

                #region Utilities Foldout
                if (utilitiesFoldout)
                {
                    DrawVerticalLine(secondaryButtonsHeight * 2 - 4);

                    if (GUILayout.Button("Tools", secondaryButtonStyle, GUILayout.Height(secondaryButtonsHeight)))
                    {
                        currentWindow = CurrentWindow.Tools;
                    }
                    if (GUILayout.Button("Presets", secondaryButtonStyle, GUILayout.Height(secondaryButtonsHeight)))
                    {
                        currentWindow = CurrentWindow.Presets;
                    }
                }
                #endregion

                DrawLineDivisor();

                if (GUILayout.Button($"{(configurationsFoldout ? foldoutSign : collapsedSign)} Configurations", primaryButtonStyle, GUILayout.Height(primaryButtonsHeight)))
                {
                    configurationsFoldout = !configurationsFoldout;
                    HierarchyDesigner_Manager_State.instance.configurationsFoldout = configurationsFoldout;
                    //HierarchyDesigner_Manager_State.instance.Save();
                }

                #region Configurations Foldout
                if (configurationsFoldout)
                {
                    DrawVerticalLine(secondaryButtonsHeight * 4);

                    if (GUILayout.Button("General Settings", secondaryButtonStyle, GUILayout.Height(secondaryButtonsHeight)))
                    {
                        currentWindow = CurrentWindow.GeneralSettings;
                    }
                    if (GUILayout.Button("Design Settings", secondaryButtonStyle, GUILayout.Height(secondaryButtonsHeight)))
                    {
                        currentWindow = CurrentWindow.DesignSettings;
                    }
                    if (GUILayout.Button("Shortcut Settings", secondaryButtonStyle, GUILayout.Height(secondaryButtonsHeight)))
                    {
                        currentWindow = CurrentWindow.ShortcutSettings;
                    }
                    if (GUILayout.Button("Advanced Settings", secondaryButtonStyle, GUILayout.Height(secondaryButtonsHeight)))
                    {
                        currentWindow = CurrentWindow.AdvancedSettings;
                    }
                }
                #endregion

                EditorGUILayout.EndScrollView();
                EditorGUILayout.LabelField($"Version {HierarchyDesigner_Manager_Data.CurrentVersion}", versionLabel);
            }
            else
            {
                EditorGUILayout.Space(40);
                buttonsCollapsedScroll = GUILayout.BeginScrollView(buttonsCollapsedScroll, false, false,  GUIStyle.none, GUIStyle.none);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetHomeButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.Home;
                }

                EditorGUILayout.Space(defaultMarginSpacing);
                DrawMiniLineDivisor();
                EditorGUILayout.Space(defaultMarginSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetFolderButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.Folders;
                }

                EditorGUILayout.Space(iconSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetSeparatorButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.Separators;
                }

                EditorGUILayout.Space(defaultMarginSpacing);
                DrawMiniLineDivisor();
                EditorGUILayout.Space(defaultMarginSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetToolsButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.Tools;
                }

                EditorGUILayout.Space(iconSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetPresetsButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.Presets;
                }

                EditorGUILayout.Space(defaultMarginSpacing);
                DrawMiniLineDivisor();
                EditorGUILayout.Space(defaultMarginSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetGeneralSettingsButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.GeneralSettings;
                }

                EditorGUILayout.Space(iconSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetDesignSettingsButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.DesignSettings;
                }

                EditorGUILayout.Space(iconSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetShortcutSettingsButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.ShortcutSettings;
                }

                EditorGUILayout.Space(iconSpacing);

                if (GUILayout.Button(HierarchyDesigner_Shared_GUI.GetAdvancedSettingsButtonContent(), primaryButtonStyle, GUILayout.Width(iconButtonsSize), GUILayout.Height(iconButtonsSize)))
                {
                    currentWindow = CurrentWindow.AdvancedSettings;
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Right Panel
            EditorGUILayout.BeginVertical(rightPanel);
            switch (currentWindow)
            {
                case CurrentWindow.Home:
                    DrawHomeWindow();
                    break;
                case CurrentWindow.Folders:
                    DrawFoldersWindow();
                    break;
                case CurrentWindow.Separators:
                    DrawSeparatorsWindow();
                    break;
                case CurrentWindow.Tools:
                    DrawToolsWindow();
                    break;
                case CurrentWindow.Presets:
                    DrawPresetsWindow();
                    break;
                case CurrentWindow.GeneralSettings:
                    DrawGeneralSettingsWindow();
                    break;
                case CurrentWindow.DesignSettings:
                    DrawDesignSettingsWindow();
                    break;
                case CurrentWindow.ShortcutSettings:
                    DrawShortcutSettingsWindow();
                    break;
                case CurrentWindow.AdvancedSettings:
                    DrawAdvancedSettingsWindow();
                    break;
            }
            EditorGUILayout.EndVertical();
            #endregion
            #endregion

            #region Footer
            EditorGUILayout.EndHorizontal();
            #endregion
        }

        #region Methods
        #region Main
        private void DrawHomeWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Home", categoryLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            homeMainScroll = EditorGUILayout.BeginScrollView(homeMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginHorizontal();
            DrawUpdateBoardPanel();
            GUILayout.Space(4);
            EditorGUILayout.BeginVertical();
            DrawContactPanel();
            EditorGUILayout.Space(defaultMarginSpacing);
            DrawMiscellaneousPanel();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(defaultMarginSpacing);
            DrawPatchNotesPanel();
            EditorGUILayout.EndScrollView();
            #endregion
        }

        private void DrawFoldersWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Folders", categoryLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            folderMainScroll = EditorGUILayout.BeginScrollView(folderMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawFoldersCreationFields();
            EditorGUILayout.Space(defaultSpacing);
            if (tempFolders.Count > 0)
            {
                DrawFoldersGlobalFields();
                EditorGUILayout.Space(defaultSpacing);
                DrawFoldersList();
            }
            else
            {
                EditorGUILayout.LabelField("No folders found. Please create a new folder.", unassignedLabel);
            }
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            if (GUILayout.Button("Update and Save Folders", GUILayout.Height(primaryButtonsHeight)))
            {
                UpdateAndSaveFoldersData();
            }
            #endregion
        }

        private void DrawSeparatorsWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Separators", categoryLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            separatorMainScroll = EditorGUILayout.BeginScrollView(separatorMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawSeparatorsCreationFields();
            EditorGUILayout.Space(defaultSpacing);

            if (tempSeparators.Count > 0)
            {
                DrawSeparatorsGlobalFields();
                EditorGUILayout.Space(defaultSpacing);
                DrawSeparatorsList();
            }
            else
            {
                EditorGUILayout.LabelField("No separators found. Please create a new separator.", unassignedLabel);
            }
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            if (GUILayout.Button("Update and Save Separators", GUILayout.Height(primaryButtonsHeight)))
            {
                UpdateAndSaveSeparatorsData();
            }
            #endregion
        }

        private void DrawToolsWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Tools", categoryLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            toolsMainScroll = EditorGUILayout.BeginScrollView(toolsMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(contentPanel);
            DrawToolsCategory();
            EditorGUILayout.Space(defaultSpacing);
            DrawToolsActions();
            EditorGUILayout.Space(defaultMarginSpacing);
            if (GUILayout.Button("Apply Action", GUILayout.Height(primaryButtonsHeight)))
            {
                if (availableActionMethods.Count > selectedActionIndex && selectedActionIndex >= 0)
                {
                    MethodInfo methodToInvoke = availableActionMethods[selectedActionIndex];
                    methodToInvoke?.Invoke(null, null);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            #endregion
        }

        private void DrawPresetsWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Presets", categoryLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            presetsMainScroll = EditorGUILayout.BeginScrollView(presetsMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawPresetsList();
            EditorGUILayout.Space(2);
            DrawPresetsFeaturesFields();
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable All Features", GUILayout.Height(secondaryButtonsHeight)))
            {
                EnableAllFeatures(true);
            }
            if (GUILayout.Button("Disable All Features", GUILayout.Height(secondaryButtonsHeight)))
            {
                EnableAllFeatures(false);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Confirm and Apply Preset", GUILayout.Height(primaryButtonsHeight)))
            {
                ApplySelectedPreset();
            }
            EditorGUILayout.EndVertical();
            #endregion
        }

        private void DrawGeneralSettingsWindow()
        {
            #region Header
            EditorGUILayout.LabelField("General Settings", categoryLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            generalSettingsMainScroll = EditorGUILayout.BeginScrollView(generalSettingsMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawGeneralSettingsCoreFeatures();
            EditorGUILayout.Space(defaultSpacing);
            DrawGeneralSettingsMainFeatures();
            EditorGUILayout.Space(defaultSpacing);
            DrawGeneralSettingsFilteringFeatures();
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable All Features", GUILayout.Height(secondaryButtonsHeight)))
            {
                EnableAllGeneralSettingsFeatures(true);
            }
            if (GUILayout.Button("Disable All Features", GUILayout.Height(secondaryButtonsHeight)))
            {
                EnableAllGeneralSettingsFeatures(false);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Update and Save General Settings", GUILayout.Height(primaryButtonsHeight)))
            {
                UpdateAndSaveGeneralSettingsData();
            }
            #endregion
        }

        private void DrawDesignSettingsWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Design Settings", categoryLabel);
            GUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            designSettingsMainScroll = EditorGUILayout.BeginScrollView(designSettingsMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawDesignSettingsComponentIcons();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsTag();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsLayer();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsTagAndLayer();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsHierarchyTree();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsHierarchyLines();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsFolder();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsSeparator();
            EditorGUILayout.Space(defaultSpacing);
            DrawDesignSettingsLock();
            EditorGUILayout.Space(defaultSpacing);
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            if (GUILayout.Button("Update and Save Design Settings", GUILayout.Height(primaryButtonsHeight)))
            {
                UpdateAndSaveDesignSettingsData();
            }
            #endregion
        }

        private void DrawShortcutSettingsWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Shortcut Settings", categoryLabel);
            GUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            shortcutSettingsMainScroll = EditorGUILayout.BeginScrollView(shortcutSettingsMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawMajorShortcuts();
            EditorGUILayout.Space(defaultSpacing);
            DrawMinorShortcuts();
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Open Shortcut Manager", GUILayout.Height(primaryButtonsHeight)))
            {
                EditorApplication.ExecuteMenuItem("Edit/Shortcuts...");
            }
            if (GUILayout.Button("Update and Save Shortcut Settings", GUILayout.Height(primaryButtonsHeight)))
            {
                UpdateAndSaveShortcutSettingsData();
            }
            EditorGUILayout.EndVertical();
            #endregion
        }

        private void DrawAdvancedSettingsWindow()
        {
            #region Header
            EditorGUILayout.LabelField("Advanced Settings", categoryLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            #endregion

            #region Body
            advancedSettingsMainScroll = EditorGUILayout.BeginScrollView(advancedSettingsMainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawAdvancedCoreFeatures();
            EditorGUILayout.Space(defaultSpacing);
            DrawAdvancedMainIconFeatures();
            EditorGUILayout.Space(defaultSpacing);
            DrawAdvancedComponentIconsFeatures();
            EditorGUILayout.Space(defaultSpacing);
            DrawAdvancedFolderFeatures();
            EditorGUILayout.Space(defaultSpacing);
            DrawAdvancedSeparatorFeatures();
            EditorGUILayout.Space(defaultSpacing);
            DrawAdvancedHierarchyToolsFeatures();
            EditorGUILayout.EndScrollView();
            #endregion

            #region Footer
            EditorGUILayout.Space(defaultSpacing);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable All Features", GUILayout.Height(secondaryButtonsHeight)))
            {
                EnableAllAdvancedSettingsFeatures(true);
            }
            if (GUILayout.Button("Disable All Features", GUILayout.Height(secondaryButtonsHeight)))
            {
                EnableAllAdvancedSettingsFeatures(false);
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Update and Save Advanced Settings", GUILayout.Height(primaryButtonsHeight)))
            {
                UpdateAndSaveAdvancedSettingsData();
            }
            #endregion
        }

        private void DrawLineDivisor()
        {
            EditorGUILayout.Space(lineDivisorSpace);
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect textureRect = new Rect(lastRect.x + 4, lastRect.y + (lineDivisorSpace / 2), lineDivisorWidth, lineDivisorHeight);
            GUI.color = HierarchyDesigner_Shared_ColorUtility.HexToColor("505050");
            GUI.DrawTexture(textureRect, HierarchyDesigner_Shared_Resources.DefaultTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
        }

        private void DrawVerticalLine(float height)
        {
            Rect lineRect = new Rect(6, GUILayoutUtility.GetLastRect().y + 35, 1, height);
            GUI.color = HierarchyDesigner_Shared_ColorUtility.HexToColor("505050");
            GUI.DrawTexture(lineRect, HierarchyDesigner_Shared_Resources.DefaultTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
        }

        private void DrawMiniLineDivisor()
        {
            EditorGUILayout.Space(lineDivisorSpace);
            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect textureRect = new Rect(lastRect.x + 5, lastRect.y + (lineDivisorSpace / 2), miniLineDivisorWidth, lineDivisorHeight);
            GUI.color = HierarchyDesigner_Shared_ColorUtility.HexToColor("505050");
            GUI.DrawTexture(textureRect, HierarchyDesigner_Shared_Resources.DefaultTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;
        }
        #endregion

        #region Home
        private void DrawUpdateBoardPanel()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("The Update Board", contentLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            homeUpdateBoardScroll = EditorGUILayout.BeginScrollView(homeUpdateBoardScroll,  GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(updateBoardContent, messageLabel, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawContactPanel()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Contact Info", contentLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            EditorGUILayout.LabelField("<b>Support Email:</b>\nVerphaSuporte@outlook.com\n\n<b>Discord Username:</b>\nverpha", messageLabel, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndVertical();
        }

        private void DrawMiscellaneousPanel()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Miscellaneous", contentLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            if (GUILayout.Button("Check for Latest Updates", GUILayout.Height(secondaryButtonsHeight)))
            {
                DownloadLatestUpdate();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPatchNotesPanel()
        {
            if (string.IsNullOrEmpty(patchNotesContent))
            {
                patchNotesContent = ReadPatchNotesFile(HierarchyDesigner_Manager_Data.GetPatchNotesFilePath());
            }

            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Patch Notes", contentLabel);
            EditorGUILayout.Space(defaultMarginSpacing);
            homePatchNotesScroll = EditorGUILayout.BeginScrollView(homePatchNotesScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.LabelField(patchNotesContent, messageLabel, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(defaultMarginSpacing);
        }

        private void LoadPatchNotes()
        {
            patchNotesContent = ReadPatchNotesFile(HierarchyDesigner_Manager_Data.GetPatchNotesFilePath());
        }

        private string ReadPatchNotesFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"Patch notes file not found at path: {filePath}");
                return "Patch notes file not found.";
            }

            try
            {
                StringBuilder patchNotesBuilder = new StringBuilder();
                int maxLines = 50;
                int lineCount = 0;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream && lineCount < maxLines)
                    {
                        string line = reader.ReadLine();
                        patchNotesBuilder.AppendLine(line);
                        lineCount++;
                    }
                }
                if (lineCount == maxLines)
                {
                    patchNotesBuilder.AppendLine("...more");
                }

                return patchNotesBuilder.ToString();
            }
            catch (IOException e)
            {
                Debug.LogError($"Error reading patch notes file: {e.Message}");
                return "Error reading patch notes.";
            }
        }

        private async Task FetchAndSetUpdateBoardContent()
        {
            updateBoardContent += await HierarchyDesigner_Manager_Data.FetchMessagesFromGist();
        }

        private async void DownloadLatestUpdate()
        {
            await HierarchyDesigner_Manager_Data.DownloadLatestHierarchyDesignerVersion();
        }
        #endregion

        #region Folder
        private void DrawFoldersCreationFields()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Folder Creation", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", fieldsLabel, GUILayout.Width(folderCreationLabelWidth));
            newFolderName = EditorGUILayout.TextField(newFolderName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Text Color", fieldsLabel, GUILayout.Width(folderCreationLabelWidth));
            newFolderTextColor = EditorGUILayout.ColorField(newFolderTextColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            string[] newFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int newFontSizeIndex = Array.IndexOf(fontSizeOptions, newFolderFontSize);
            EditorGUILayout.LabelField("Font Size", fieldsLabel, GUILayout.Width(folderCreationLabelWidth));
            newFolderFontSize = fontSizeOptions[EditorGUILayout.Popup(newFontSizeIndex, newFontSizeOptionsStrings)];
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Font Style", fieldsLabel, GUILayout.Width(folderCreationLabelWidth));
            newFolderFontStyle = (FontStyle)EditorGUILayout.EnumPopup(newFolderFontStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Image Color", fieldsLabel, GUILayout.Width(folderCreationLabelWidth));
            newFolderIconColor = EditorGUILayout.ColorField(newFolderIconColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Image Type", fieldsLabel, GUILayout.Width(folderCreationLabelWidth));
            if (GUILayout.Button(HierarchyDesigner_Configurable_Folder.GetFolderImageTypeDisplayName(newFolderImageType), EditorStyles.popup))
            {
                ShowFolderImageTypePopup();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);
            if (GUILayout.Button("Create Folder", GUILayout.Height(secondaryButtonsHeight)))
            {
                if (IsFolderNameValid(newFolderName))
                {
                    CreateFolder(newFolderName, newFolderTextColor, newFolderFontSize, newFolderFontStyle, newFolderIconColor, newFolderImageType);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder Name", "Folder name is either duplicate or invalid.", "OK");
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawFoldersGlobalFields()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Folders' Global Fields", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            tempFolderGlobalTextColor = EditorGUILayout.ColorField(tempFolderGlobalTextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalFolderTextColor(tempFolderGlobalTextColor); }
            EditorGUI.BeginChangeCheck();
            string[] tempFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int tempFontSizeIndex = Array.IndexOf(fontSizeOptions, tempFolderGlobalFontSize);
            tempFolderGlobalFontSize = fontSizeOptions[EditorGUILayout.Popup(tempFontSizeIndex, tempFontSizeOptionsStrings, GUILayout.Width(50))];
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalFolderFontSize(tempFolderGlobalFontSize); }
            EditorGUI.BeginChangeCheck();
            tempFolderGlobalFontStyle = (FontStyle)EditorGUILayout.EnumPopup(tempFolderGlobalFontStyle, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalFolderFontStyle(tempFolderGlobalFontStyle); }
            EditorGUI.BeginChangeCheck();
            tempFolderGlobalIconColor = EditorGUILayout.ColorField(tempFolderGlobalIconColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalFolderIconColor(tempFolderGlobalIconColor); }
            if (GUILayout.Button(HierarchyDesigner_Configurable_Folder.GetFolderImageTypeDisplayName(tempGlobalFolderImageType), EditorStyles.popup, GUILayout.MinWidth(125), GUILayout.ExpandWidth(true))) { ShowFolderImageTypePopupGlobal(); }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawFoldersList()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Folders' List", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            foldersListScroll = EditorGUILayout.BeginScrollView(foldersListScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            int index = 1;
            for (int i = 0; i < foldersOrder.Count; i++)
            {
                string key = foldersOrder[i];
                DrawFolders(index, key, tempFolders[key], i, foldersOrder.Count);
                index++;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

        }

        private void UpdateAndSaveFoldersData()
        {
            HierarchyDesigner_Configurable_Folder.ApplyChangesToFolders(tempFolders, foldersOrder);
            HierarchyDesigner_Configurable_Folder.SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearFolderCache();
            folderHasModifiedChanges = false;
        }

        private void LoadFolderData()
        {
            tempFolders = HierarchyDesigner_Configurable_Folder.GetAllFoldersData(true);
            foldersOrder = new List<string>(tempFolders.Keys);
        }

        private void LoadFolderCreationFields()
        {
            newFolderTextColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultTextColor;
            newFolderFontSize = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontSize;
            newFolderFontStyle = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontStyle;
            newFolderIconColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageColor;
            HierarchyDesigner_Configurable_Folder.FolderImageType newFolderImageType = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageType;
        }

        #region Folder Operations
        private bool IsFolderNameValid(string folderName)
        {
            return !string.IsNullOrEmpty(folderName) && !tempFolders.TryGetValue(folderName, out _);
        }

        private void CreateFolder(string folderName, Color textColor, int fontSize, FontStyle fontStyle, Color ImageColor, HierarchyDesigner_Configurable_Folder.FolderImageType imageType)
        {
            HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData newFolderData = new HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData
            {
                Name = folderName,
                TextColor = textColor,
                FontSize = fontSize,
                FontStyle = fontStyle,
                ImageColor = ImageColor,
                ImageType = imageType
            };
            tempFolders[folderName] = newFolderData;
            foldersOrder.Add(folderName);
            newFolderName = "";
            newFolderTextColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultTextColor;
            newFolderFontSize = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontSize;
            newFolderFontStyle = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontStyle;
            newFolderIconColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageColor;
            newFolderImageType = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageType;
            folderHasModifiedChanges = true;
            GUI.FocusControl(null);
        }

        private void DrawFolders(int index, string key, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folderData, int position, int totalItems)
        {
            float folderLabelWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(tempFolders.Keys);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{index}) {folderData.Name}", fieldsLabel, GUILayout.Width(folderLabelWidth));
            EditorGUI.BeginChangeCheck();
            folderData.TextColor = EditorGUILayout.ColorField(folderData.TextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            string[] fontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int fontSizeIndex = Array.IndexOf(fontSizeOptions, folderData.FontSize);
            if (fontSizeIndex == -1) { fontSizeIndex = 5; }
            folderData.FontSize = fontSizeOptions[EditorGUILayout.Popup(fontSizeIndex, fontSizeOptionsStrings, GUILayout.Width(50))];
            folderData.FontStyle = (FontStyle)EditorGUILayout.EnumPopup(folderData.FontStyle, GUILayout.Width(110));
            folderData.ImageColor = EditorGUILayout.ColorField(folderData.ImageColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (GUILayout.Button(HierarchyDesigner_Configurable_Folder.GetFolderImageTypeDisplayName(folderData.ImageType), EditorStyles.popup, GUILayout.MinWidth(125), GUILayout.ExpandWidth(true))) { ShowFolderImageTypePopupForFolder(folderData); }
            if (EditorGUI.EndChangeCheck()) { folderHasModifiedChanges = true; }

            if (GUILayout.Button("↑", GUILayout.Width(moveItemInListButtonWidth)) && position > 0)
            {
                MoveFolder(position, position - 1);
            }
            if (GUILayout.Button("↓", GUILayout.Width(moveItemInListButtonWidth)) && position < totalItems - 1)
            {
                MoveFolder(position, position + 1);
            }
            if (GUILayout.Button("Create", GUILayout.Width(createButtonWidth)))
            {
                CreateFolderGameObject(folderData);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(removeButtonWidth)))
            {
                RemoveFolder(key);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void MoveFolder(int indexA, int indexB)
        {
            string keyA = foldersOrder[indexA];
            string keyB = foldersOrder[indexB];

            foldersOrder[indexA] = keyB;
            foldersOrder[indexB] = keyA;
            folderHasModifiedChanges = true;
        }

        private void CreateFolderGameObject(HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folderData)
        {
            GameObject folder = new GameObject(folderData.Name);
            folder.AddComponent<HierarchyDesignerFolder>();
            Undo.RegisterCreatedObjectUndo(folder, $"Create {folderData.Name}");

            Texture2D inspectorIcon = HierarchyDesigner_Shared_Resources.FolderInspectorIcon;
            if (inspectorIcon != null)
            {
                EditorGUIUtility.SetIconForObject(folder, inspectorIcon);
            }
        }

        private void RemoveFolder(string folderName)
        {
            if (tempFolders.TryGetValue(folderName, out _))
            {
                tempFolders.Remove(folderName);
                foldersOrder.Remove(folderName);
                folderHasModifiedChanges = true;
                GUIUtility.ExitGUI();
            }
        }
        #endregion

        #region Folder Image Type
        private void ShowFolderImageTypePopup()
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Folder.GetFolderImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Folder.GetFolderImageTypeDisplayName(newFolderImageType), OnFolderImageTypeSelected, typeName);
                }
            }
            menu.ShowAsContext();
        }

        private void ShowFolderImageTypePopupGlobal()
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Folder.GetFolderImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Folder.GetFolderImageTypeDisplayName(tempGlobalFolderImageType), OnFolderImageTypeGlobalSelected, typeName);
                }
            }
            menu.ShowAsContext();
        }

        private void ShowFolderImageTypePopupForFolder(HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folderData)
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Folder.GetFolderImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Folder.GetFolderImageTypeDisplayName(folderData.ImageType), OnFolderImageTypeForFolderSelected, new KeyValuePair<HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData, string>(folderData, typeName));
                }
            }
            menu.ShowAsContext();
        }

        private void OnFolderImageTypeSelected(object imageTypeObj)
        {
            string typeName = (string)imageTypeObj;
            newFolderImageType = HierarchyDesigner_Configurable_Folder.ParseFolderImageType(typeName);
        }

        private void OnFolderImageTypeGlobalSelected(object imageTypeObj)
        {
            string typeName = (string)imageTypeObj;
            tempGlobalFolderImageType = HierarchyDesigner_Configurable_Folder.ParseFolderImageType(typeName);
            UpdateGlobalFolderImageType(tempGlobalFolderImageType);
        }

        private void OnFolderImageTypeForFolderSelected(object folderDataAndTypeObj)
        {
            KeyValuePair<HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData, string> folderDataAndType = (KeyValuePair<HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData, string>)folderDataAndTypeObj;
            folderDataAndType.Key.ImageType = HierarchyDesigner_Configurable_Folder.ParseFolderImageType(folderDataAndType.Value);
        }
        #endregion

        #region Folder Global Fields
        private void UpdateGlobalFolderTextColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folder in tempFolders.Values)
            {
                folder.TextColor = color;
            }
            folderHasModifiedChanges = true;
        }

        private void UpdateGlobalFolderFontSize(int size)
        {
            foreach (HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folder in tempFolders.Values)
            {
                folder.FontSize = size;
            }
            folderHasModifiedChanges = true;
        }

        private void UpdateGlobalFolderFontStyle(FontStyle style)
        {
            foreach (HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folder in tempFolders.Values)
            {
                folder.FontStyle = style;
            }
            folderHasModifiedChanges = true;
        }

        private void UpdateGlobalFolderIconColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folder in tempFolders.Values)
            {
                folder.ImageColor = color;
            }
            folderHasModifiedChanges = true;
        }

        private void UpdateGlobalFolderImageType(HierarchyDesigner_Configurable_Folder.FolderImageType imageType)
        {
            foreach (HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folder in tempFolders.Values)
            {
                folder.ImageType = imageType;
            }
            folderHasModifiedChanges = true;
        }
        #endregion
        #endregion

        #region Separator
        private void DrawSeparatorsCreationFields()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Separator Creation", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Name", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
            newSeparatorName = EditorGUILayout.TextField(newSeparatorName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Text Color", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
            newSeparatorTextColor = EditorGUILayout.ColorField(newSeparatorTextColor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is Gradient Background", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
            newSeparatorIsGradient = EditorGUILayout.Toggle(newSeparatorIsGradient);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (newSeparatorIsGradient)
            {
                EditorGUILayout.LabelField("Background Gradient", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
                newSeparatorBackgroundGradient = EditorGUILayout.GradientField(newSeparatorBackgroundGradient) != null ? newSeparatorBackgroundGradient : new Gradient();
            }
            else
            {
                EditorGUILayout.LabelField("Background Color", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
                newSeparatorBackgroundColor = EditorGUILayout.ColorField(newSeparatorBackgroundColor);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            string[] newFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int newFontSizeIndex = Array.IndexOf(fontSizeOptions, newSeparatorFontSize);
            EditorGUILayout.LabelField("Font Size", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
            newSeparatorFontSize = fontSizeOptions[EditorGUILayout.Popup(newFontSizeIndex, newFontSizeOptionsStrings)];
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Font Style", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
            newSeparatorFontStyle = (FontStyle)EditorGUILayout.EnumPopup(newSeparatorFontStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Text Anchor", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
            newSeparatorTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(newSeparatorTextAnchor);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Background Type", fieldsLabel, GUILayout.Width(separatorCreationLabelWidth));
            if (GUILayout.Button(HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(newSeparatorImageType), EditorStyles.popup))
            {
                ShowSeparatorImageTypePopup();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);
            if (GUILayout.Button("Create Separator", GUILayout.Height(secondaryButtonsHeight)))
            {
                if (IsSeparatorNameValid(newSeparatorName))
                {
                    CreateSeparator(newSeparatorName, newSeparatorTextColor, newSeparatorIsGradient, newSeparatorBackgroundColor, newSeparatorBackgroundGradient, newSeparatorFontSize, newSeparatorFontStyle, newSeparatorTextAnchor, newSeparatorImageType);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Separator Name", "Separator name is either duplicate or invalid.", "OK");
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSeparatorsGlobalFields()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Separators' Global Fields", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            tempSeparatorGlobalTextColor = EditorGUILayout.ColorField(tempSeparatorGlobalTextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorTextColor(tempSeparatorGlobalTextColor); }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space(defaultMarginSpacing);
            tempSeparatorGlobalIsGradient = EditorGUILayout.Toggle(tempSeparatorGlobalIsGradient, GUILayout.Width(18));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorIsGradientBackground(tempSeparatorGlobalIsGradient); }
            EditorGUI.BeginChangeCheck();
            tempSeparatorGlobalBackgroundColor = EditorGUILayout.ColorField(tempSeparatorGlobalBackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorBackgroundColor(tempSeparatorGlobalBackgroundColor); }
            EditorGUI.BeginChangeCheck();
            tempSeparatorGlobalBackgroundGradient = EditorGUILayout.GradientField(tempSeparatorGlobalBackgroundGradient, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorGradientBackground(tempSeparatorGlobalBackgroundGradient); }
            EditorGUI.BeginChangeCheck();
            string[] tempFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int tempFontSizeIndex = Array.IndexOf(fontSizeOptions, tempSeparatorGlobalFontSize);
            tempSeparatorGlobalFontSize = fontSizeOptions[EditorGUILayout.Popup(tempFontSizeIndex, tempFontSizeOptionsStrings, GUILayout.Width(50))];
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorFontSize(tempSeparatorGlobalFontSize); }
            EditorGUI.BeginChangeCheck();
            tempSeparatorGlobalFontStyle = (FontStyle)EditorGUILayout.EnumPopup(tempSeparatorGlobalFontStyle, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorFontStyle(tempSeparatorGlobalFontStyle); }
            EditorGUI.BeginChangeCheck();
            tempSeparatorGlobalTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(tempSeparatorGlobalTextAnchor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck()) { UpdateGlobalSeparatorTextAnchor(tempSeparatorGlobalTextAnchor); }
            if (GUILayout.Button(HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(tempSeparatorGlobalImageType), EditorStyles.popup, GUILayout.MinWidth(150), GUILayout.ExpandWidth(true))) { ShowSeparatorImageTypePopupGlobal(); }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawSeparatorsList()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Separators' List", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);

            separatorsListScroll = EditorGUILayout.BeginScrollView(separatorsListScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            int index = 1;
            for (int i = 0; i < separatorsOrder.Count; i++)
            {
                string key = separatorsOrder[i];
                DrawSeparators(index, key, tempSeparators[key], i, separatorsOrder.Count);
                index++;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void UpdateAndSaveSeparatorsData()
        {
            HierarchyDesigner_Configurable_Separator.ApplyChangesToSeparators(tempSeparators, separatorsOrder);
            HierarchyDesigner_Configurable_Separator.SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearSeparatorCache();
            separatorHasModifiedChanges = false;
        }

        private void LoadSeparatorData()
        {
            tempSeparators = HierarchyDesigner_Configurable_Separator.GetAllSeparatorsData(true);
            separatorsOrder = new List<string>(tempSeparators.Keys);
        }

        private void LoadSeparatorsCreationFields()
        {
            newSeparatorTextColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextColor;
            newSeparatorIsGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultIsGradientBackground;
            newSeparatorBackgroundColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundColor;
            newSeparatorBackgroundGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundGradient;
            newSeparatorFontSize = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontSize;
            newSeparatorFontStyle = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontStyle;
            newSeparatorTextAnchor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextAnchor;
            HierarchyDesigner_Configurable_Separator.SeparatorImageType newSeparatorImageType = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultImageType;
        }

        #region Separator Operations
        private bool IsSeparatorNameValid(string separatorName)
        {
            return !string.IsNullOrEmpty(separatorName) && !tempSeparators.TryGetValue(separatorName, out _);
        }

        private void CreateSeparator(string separatorName, Color textColor, bool isGradient, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, HierarchyDesigner_Configurable_Separator.SeparatorImageType imageType)
        {
            HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData newSeparatorData = new HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData
            {
                Name = separatorName,
                TextColor = textColor,
                IsGradientBackground = isGradient,
                BackgroundColor = backgroundColor,
                BackgroundGradient = backgroundGradient,
                FontSize = fontSize,
                FontStyle = fontStyle,
                TextAnchor = textAnchor,
                ImageType = imageType,

            };
            tempSeparators[separatorName] = newSeparatorData;
            separatorsOrder.Add(separatorName);
            newSeparatorName = "";
            newSeparatorTextColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextColor;
            newSeparatorIsGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultIsGradientBackground;
            newSeparatorBackgroundColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundColor;
            newSeparatorBackgroundGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundGradient;
            newSeparatorFontSize = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontSize;
            newSeparatorFontStyle = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontStyle;
            newSeparatorTextAnchor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextAnchor;
            newSeparatorImageType = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultImageType;
            separatorHasModifiedChanges = true;
            GUI.FocusControl(null);
        }

        private void DrawSeparators(int index, string key, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData, int position, int totalItems)
        {
            float separatorLabelWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(tempSeparators.Keys);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"{index}) {separatorData.Name}", fieldsLabel, GUILayout.Width(separatorLabelWidth));
            EditorGUI.BeginChangeCheck();
            separatorData.TextColor = EditorGUILayout.ColorField(separatorData.TextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            GUILayout.Space(defaultMarginSpacing);
            separatorData.IsGradientBackground = EditorGUILayout.Toggle(separatorData.IsGradientBackground, GUILayout.Width(18));
            if (separatorData.IsGradientBackground) { separatorData.BackgroundGradient = EditorGUILayout.GradientField(separatorData.BackgroundGradient, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true)) ?? new Gradient(); }
            else { separatorData.BackgroundColor = EditorGUILayout.ColorField(separatorData.BackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true)); }
            string[] fontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int fontSizeIndex = Array.IndexOf(fontSizeOptions, separatorData.FontSize);
            if (fontSizeIndex == -1) { fontSizeIndex = 5; }
            separatorData.FontSize = fontSizeOptions[EditorGUILayout.Popup(fontSizeIndex, fontSizeOptionsStrings, GUILayout.Width(50))];
            separatorData.FontStyle = (FontStyle)EditorGUILayout.EnumPopup(separatorData.FontStyle, GUILayout.Width(100));
            separatorData.TextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(separatorData.TextAnchor, GUILayout.Width(125));
            if (GUILayout.Button(HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(separatorData.ImageType), EditorStyles.popup, GUILayout.Width(150))) { ShowSeparatorImageTypePopupForSeparator(separatorData); }
            if (EditorGUI.EndChangeCheck()) { separatorHasModifiedChanges = true; }

            if (GUILayout.Button("↑", GUILayout.Width(moveItemInListButtonWidth)) && position > 0)
            {
                MoveSeparator(position, position - 1);
            }
            if (GUILayout.Button("↓", GUILayout.Width(moveItemInListButtonWidth)) && position < totalItems - 1)
            {
                MoveSeparator(position, position + 1);
            }
            if (GUILayout.Button("Create", GUILayout.Width(createButtonWidth)))
            {
                CreateSeparatorGameObject(separatorData);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(removeButtonWidth)))
            {
                RemoveSeparator(key);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void MoveSeparator(int indexA, int indexB)
        {
            string keyA = separatorsOrder[indexA];
            string keyB = separatorsOrder[indexB];

            separatorsOrder[indexA] = keyB;
            separatorsOrder[indexB] = keyA;
            separatorHasModifiedChanges = true;
        }

        private void CreateSeparatorGameObject(HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData)
        {
            GameObject separator = new GameObject($"//{separatorData.Name}");
            separator.tag = "EditorOnly";
            HierarchyDesigner_Utility_Separator.SetSeparatorState(separator, false);
            separator.SetActive(false);
            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorData.Name}");

            Texture2D inspectorIcon = HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon;
            if (inspectorIcon != null)
            {
                EditorGUIUtility.SetIconForObject(separator, inspectorIcon);
            }
        }

        private void RemoveSeparator(string separatorName)
        {
            if (tempSeparators.TryGetValue(separatorName, out _))
            {
                tempSeparators.Remove(separatorName);
                separatorsOrder.Remove(separatorName);
                separatorHasModifiedChanges = true;
                GUIUtility.ExitGUI();
            }
        }
        #endregion

        #region Separator Image Type
        private void ShowSeparatorImageTypePopup()
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(newSeparatorImageType), OnSeparatorImageTypeSelected, typeName);
                }
            }
            menu.ShowAsContext();
        }

        private void ShowSeparatorImageTypePopupGlobal()
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(tempSeparatorGlobalImageType), OnSeparatorImageTypeGlobalSelected, typeName);
                }
            }
            menu.ShowAsContext();
        }

        private void ShowSeparatorImageTypePopupForSeparator(HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData)
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedTypes = HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypesGrouped();
            foreach (KeyValuePair<string, List<string>> group in groupedTypes)
            {
                foreach (string typeName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{typeName}"), typeName == HierarchyDesigner_Configurable_Separator.GetSeparatorImageTypeDisplayName(separatorData.ImageType), OnSeparatorImageTypeForSeparatorSelected, new KeyValuePair<HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData, string>(separatorData, typeName));
                }
            }
            menu.ShowAsContext();
        }

        private void OnSeparatorImageTypeSelected(object imageTypeObj)
        {
            string typeName = (string)imageTypeObj;
            newSeparatorImageType = HierarchyDesigner_Configurable_Separator.ParseSeparatorImageType(typeName);
        }

        private void OnSeparatorImageTypeGlobalSelected(object imageTypeObj)
        {
            string typeName = (string)imageTypeObj;
            tempSeparatorGlobalImageType = HierarchyDesigner_Configurable_Separator.ParseSeparatorImageType(typeName);
            UpdateGlobalSeparatorImageType(tempSeparatorGlobalImageType);
        }

        private void OnSeparatorImageTypeForSeparatorSelected(object separatorDataAndTypeObj)
        {
            KeyValuePair<HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData, string> separatorDataAndType = (KeyValuePair<HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData, string>)separatorDataAndTypeObj;
            separatorDataAndType.Key.ImageType = HierarchyDesigner_Configurable_Separator.ParseSeparatorImageType(separatorDataAndType.Value);
        }
        #endregion

        #region Separator Global Fields
        private void UpdateGlobalSeparatorTextColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.TextColor = color;
            }
            separatorHasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorIsGradientBackground(bool isGradientBackground)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.IsGradientBackground = isGradientBackground;
            }
            separatorHasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorBackgroundColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.BackgroundColor = color;
            }
            separatorHasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorGradientBackground(Gradient gradientBackground)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.BackgroundGradient = gradientBackground;
            }
            separatorHasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorFontSize(int size)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.FontSize = size;
            }
            separatorHasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorFontStyle(FontStyle style)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.FontStyle = style;
            }
            separatorHasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorTextAnchor(TextAnchor anchor)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.TextAnchor = anchor;
            }
            separatorHasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorImageType(HierarchyDesigner_Configurable_Separator.SeparatorImageType imageType)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.ImageType = imageType;
            }
            separatorHasModifiedChanges = true;
        }
        #endregion
        #endregion

        #region Utilities
        #region Tools
        private void DrawToolsCategory()
        {
            HierarchyDesigner_Attribute_Tools previousCategory = selectedCategory;
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Category:", contentLabel, GUILayout.Width(labelWidth));
            selectedCategory = (HierarchyDesigner_Attribute_Tools)EditorGUILayout.EnumPopup(selectedCategory);
            if (previousCategory != selectedCategory) { UpdateAvailableActions(); }
            EditorGUILayout.EndVertical();
        }

        private void DrawToolsActions()
        {
            if (availableActionNames.Count == 0) { GUILayout.Label("No tools available for this category.", unassignedLabel); }
            else
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Action:", contentLabel, GUILayout.Width(labelWidth));
                selectedActionIndex = EditorGUILayout.Popup(selectedActionIndex, availableActionNames.ToArray());
                EditorGUILayout.EndVertical();
            }
        }

        private void LoadTools()
        {
            if (!cacheInitialized)
            {
                InitializeActionCache();
                cacheInitialized = true;
            }
            UpdateAvailableActions();
        }

        private void InitializeActionCache()
        {
            MethodInfo[] methods = typeof(HierarchyDesigner_Utility_Tools).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                object[] toolAttributes = method.GetCustomAttributes(typeof(HierarchyDesigner_Shared_Attributes), false);
                for (int i = 0; i < toolAttributes.Length; i++)
                {
                    HierarchyDesigner_Shared_Attributes toolAttribute = toolAttributes[i] as HierarchyDesigner_Shared_Attributes;
                    if (toolAttribute != null)
                    {
                        object[] menuItemAttributes = method.GetCustomAttributes(typeof(MenuItem), true);
                        for (int j = 0; j < menuItemAttributes.Length; j++)
                        {
                            MenuItem menuItemAttribute = menuItemAttributes[j] as MenuItem;
                            if (menuItemAttribute != null)
                            {
                                string rawActionName = menuItemAttribute.menuItem;
                                string actionName = ExtractActionsFromCategories(rawActionName, toolAttribute.Category);
                                if (!string.IsNullOrEmpty(actionName))
                                {
                                    if (!cachedActions.TryGetValue(toolAttribute.Category, out List<(string Name, MethodInfo Method)> actionsList))
                                    {
                                        actionsList = new List<(string Name, MethodInfo Method)>();
                                        cachedActions[toolAttribute.Category] = actionsList;
                                    }
                                    actionsList.Add((actionName, method));
                                }
                            }
                        }
                    }
                }
            }
        }

        private string ExtractActionsFromCategories(string menuItemPath, HierarchyDesigner_Attribute_Tools category)
        {
            string[] parts = menuItemPath.Split('/');
            if (parts.Length < 2) return null;

            switch (category)
            {
                case HierarchyDesigner_Attribute_Tools.Rename:
                case HierarchyDesigner_Attribute_Tools.Sort:
                    return parts[parts.Length - 1];
                case HierarchyDesigner_Attribute_Tools.Activate:
                case HierarchyDesigner_Attribute_Tools.Count:
                case HierarchyDesigner_Attribute_Tools.Lock:
                case HierarchyDesigner_Attribute_Tools.Select:
                    return string.Join("/", parts, 3, parts.Length - 3);
                default:
                    return parts[parts.Length - 2] + "/" + parts[parts.Length - 1];
            }
        }

        private void UpdateAvailableActions()
        {
            availableActionNames.Clear();
            availableActionMethods.Clear();
            if (cachedActions.TryGetValue(selectedCategory, out List<(string Name, MethodInfo Method)> actions))
            {
                foreach ((string Name, MethodInfo Method) action in actions)
                {
                    availableActionNames.Add(action.Name);
                    availableActionMethods.Add(action.Method);
                }
            }
            selectedActionIndex = 0;
        }
        #endregion

        #region Presets
        private void DrawPresetsList()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Select a Preset:", contentLabel, GUILayout.Width(presetslabelWidth));
            GUILayout.Space(contentLabelSpacing);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Selected Preset:", fieldsLabel, GUILayout.Width(160));
            if (GUILayout.Button(presetNames[selectedPresetIndex], EditorStyles.popup))
            {
                ShowPresetPopup();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        private void DrawPresetsFeaturesFields()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Apply Preset To:", contentLabel, GUILayout.Width(presetslabelWidth));
            GUILayout.Space(contentLabelSpacing);

            applyToFolders = HierarchyDesigner_Shared_GUI.DrawToggle("Folders", presetsToggleLabelWidth, applyToFolders);
            applyToSeparators = HierarchyDesigner_Shared_GUI.DrawToggle("Separators", presetsToggleLabelWidth, applyToSeparators);
            applyToTag = HierarchyDesigner_Shared_GUI.DrawToggle("GameObject's Tag", presetsToggleLabelWidth, applyToTag);
            applyToLayer = HierarchyDesigner_Shared_GUI.DrawToggle("GameObject's Layer", presetsToggleLabelWidth, applyToLayer);
            applyToTree = HierarchyDesigner_Shared_GUI.DrawToggle("Hierarchy Tree", presetsToggleLabelWidth, applyToTree);
            applyToLines = HierarchyDesigner_Shared_GUI.DrawToggle("Hierarchy Lines", presetsToggleLabelWidth, applyToLines);
            applyToFolderDefaultValues = HierarchyDesigner_Shared_GUI.DrawToggle("Folder Default Values", presetsToggleLabelWidth, applyToFolderDefaultValues);
            applyToSeparatorDefaultValues = HierarchyDesigner_Shared_GUI.DrawToggle("Separator Default Values", presetsToggleLabelWidth, applyToSeparatorDefaultValues);
            applyToLock = HierarchyDesigner_Shared_GUI.DrawToggle("Lock Label", presetsToggleLabelWidth, applyToLock);
            EditorGUILayout.EndVertical();
        }

        private void LoadPresets()
        {
            presetNames = HierarchyDesigner_Configurable_Presets.GetPresetNames();
        }

        #region Presets Operations
        private void ShowPresetPopup()
        {
            GenericMenu menu = new GenericMenu();
            Dictionary<string, List<string>> groupedPresets = HierarchyDesigner_Configurable_Presets.GetPresetNamesGrouped();

            foreach (KeyValuePair<string, List<string>> group in groupedPresets)
            {
                foreach (string presetName in group.Value)
                {
                    menu.AddItem(new GUIContent($"{group.Key}/{presetName}"), presetName == presetNames[selectedPresetIndex], OnPresetSelected, presetName);
                }
            }

            menu.ShowAsContext();
        }

        private void OnPresetSelected(object presetNameObj)
        {
            string presetName = (string)presetNameObj;
            selectedPresetIndex = Array.IndexOf(presetNames, presetName);
        }

        private void ApplySelectedPreset()
        {
            if (selectedPresetIndex < 0 || selectedPresetIndex >= presetNames.Length) return;

            HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset selectedPreset = HierarchyDesigner_Configurable_Presets.Presets[selectedPresetIndex];

            string message = "Are you sure you want to override your current values for: ";
            List<string> changesList = new List<string>();
            if (applyToFolders) changesList.Add("Folders");
            if (applyToSeparators) changesList.Add("Separators");
            if (applyToTag) changesList.Add("GameObject's Tag");
            if (applyToLayer) changesList.Add("GameObject's Layer");
            if (applyToTree) changesList.Add("Hierarchy Tree");
            if (applyToLines) changesList.Add("Hierarchy Lines");
            if (applyToFolderDefaultValues) changesList.Add("Folder Default Values");
            if (applyToSeparatorDefaultValues) changesList.Add("Separator Default Values");
            if (applyToLock) changesList.Add("Lock Label");
            message += string.Join(", ", changesList) + "?\n\n*If you select 'confirm' all values will be overridden and saved.*";

            if (EditorUtility.DisplayDialog("Confirm Preset Application", message, "Confirm", "Cancel"))
            {
                if (applyToFolders)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToFolders(selectedPreset);
                }
                if (applyToSeparators)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToSeparators(selectedPreset);
                }
                bool shouldSaveDesignSettings = false;
                if (applyToTag)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToTag(selectedPreset);
                    shouldSaveDesignSettings = true;
                }
                if (applyToLayer)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToLayer(selectedPreset);
                    shouldSaveDesignSettings = true;
                }
                if (applyToTree)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToTree(selectedPreset);
                    shouldSaveDesignSettings = true;
                }
                if (applyToFolderDefaultValues)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToDefaultFolderValues(selectedPreset);
                    shouldSaveDesignSettings = true;
                }
                if (applyToSeparatorDefaultValues)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToDefaultSeparatorValues(selectedPreset);
                    shouldSaveDesignSettings = true;
                }
                if (applyToLines)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToLines(selectedPreset);
                    shouldSaveDesignSettings = true;
                }
                if (applyToLock)
                {
                    HierarchyDesigner_Utility_Presets.ApplyPresetToLockLabel(selectedPreset);
                    shouldSaveDesignSettings = true;
                }
                if (shouldSaveDesignSettings)
                {
                    HierarchyDesigner_Configurable_DesignSettings.SaveSettings();
                    LoadDesignSettingsData();
                    LoadFolderCreationFields();
                    LoadSeparatorsCreationFields();
                }
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        private void EnableAllFeatures(bool enable)
        {
            applyToFolders = enable;
            applyToSeparators = enable;
            applyToTag = enable;
            applyToLayer = enable;
            applyToTree = enable;
            applyToLines = enable;
            applyToFolderDefaultValues = enable;
            applyToSeparatorDefaultValues = enable;
            applyToLock = enable;
        }
        #endregion
        #endregion
        #endregion

        #region Configurations
        #region General Settings
        private void DrawGeneralSettingsCoreFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Core Features", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempLayoutMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Layout Mode", enumPopupLabelWidth, tempLayoutMode);
            tempTreeMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Tree Mode", enumPopupLabelWidth, tempTreeMode);
            if (EditorGUI.EndChangeCheck()) { generalSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawGeneralSettingsMainFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Main Features", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempEnableGameObjectMainIcon = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Main Icon", generalSettingstoggleLabelWidth, tempEnableGameObjectMainIcon);
            tempEnableGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Component Icons", generalSettingstoggleLabelWidth, tempEnableGameObjectComponentIcons);
            tempEnableGameObjectTag = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Tag", generalSettingstoggleLabelWidth, tempEnableGameObjectTag);
            tempEnableGameObjectLayer = HierarchyDesigner_Shared_GUI.DrawToggle("Enable GameObject's Layer", generalSettingstoggleLabelWidth, tempEnableGameObjectLayer);
            tempEnableHierarchyTree = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Hierarchy Tree", generalSettingstoggleLabelWidth, tempEnableHierarchyTree);
            tempEnableHierarchyRows = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Hierarchy Rows", generalSettingstoggleLabelWidth, tempEnableHierarchyRows);
            tempEnableHierarchyLines = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Hierarchy Lines", generalSettingstoggleLabelWidth, tempEnableHierarchyLines);
            tempEnableHierarchyButtons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Hierarchy Buttons", generalSettingstoggleLabelWidth, tempEnableHierarchyButtons);
            tempEnableMajorShortcuts = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Major Shortcuts", generalSettingstoggleLabelWidth, tempEnableMajorShortcuts);
            tempDisableHierarchyDesignerDuringPlayMode = HierarchyDesigner_Shared_GUI.DrawToggle("Disable Hierarchy Designer During PlayMode", generalSettingstoggleLabelWidth, tempDisableHierarchyDesignerDuringPlayMode);
            if (EditorGUI.EndChangeCheck()) { generalSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawGeneralSettingsFilteringFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Filtering Features", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempExcludeFolderProperties = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Folder Properties", generalSettingstoggleLabelWidth, tempExcludeFolderProperties);
            tempExcludeTransformForGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Transform Component", generalSettingstoggleLabelWidth, tempExcludeTransformForGameObjectComponentIcons);
            tempExcludeCanvasRendererForGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Canvas Renderer Component", generalSettingstoggleLabelWidth, tempExcludeCanvasRendererForGameObjectComponentIcons);
            tempMaximumComponentIconsAmount = HierarchyDesigner_Shared_GUI.DrawIntSlider("Maximum Component Icons Amount", generalSettingstoggleLabelWidth, tempMaximumComponentIconsAmount, 1, 25);
            if (EditorGUI.EndChangeCheck()) { generalSettingsHasModifiedChanges = true; }
            GUILayout.Space(defaultMarginSpacing);

            #region Tag
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            int tagMask = GetMaskFromList(tempExcludedTags, tags);
            EditorGUI.BeginChangeCheck();
            tagMask = HierarchyDesigner_Shared_GUI.DrawMaskField("Excluded Tags", maskFieldLabelWidth, tagMask, tags);
            if (EditorGUI.EndChangeCheck()) { generalSettingsHasModifiedChanges = true; }
            tempExcludedTags = GetListFromMask(tagMask, tags);
            #endregion

            #region Layer
            string[] layers = UnityEditorInternal.InternalEditorUtility.layers;
            int layerMask = GetMaskFromList(tempExcludedLayers, layers);
            layerMask = HierarchyDesigner_Shared_GUI.DrawMaskField("Excluded Layers", maskFieldLabelWidth, layerMask, layers);
            EditorGUI.BeginChangeCheck();
            tempExcludedLayers = GetListFromMask(layerMask, layers);
            if (EditorGUI.EndChangeCheck()) { generalSettingsHasModifiedChanges = true; }
            #endregion
            EditorGUILayout.EndVertical();
        }

        private void UpdateAndSaveGeneralSettingsData()
        {
            HierarchyDesigner_Configurable_GeneralSettings.LayoutMode = tempLayoutMode;
            HierarchyDesigner_Configurable_GeneralSettings.TreeMode = tempTreeMode;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectMainIcon = tempEnableGameObjectMainIcon;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectComponentIcons = tempEnableGameObjectComponentIcons;
            HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyTree = tempEnableHierarchyTree;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectTag = tempEnableGameObjectTag;
            HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectLayer = tempEnableGameObjectLayer;
            HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyRows = tempEnableHierarchyRows;
            HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyLines = tempEnableHierarchyLines;
            HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyButtons = tempEnableHierarchyButtons;
            HierarchyDesigner_Configurable_GeneralSettings.EnableMajorShortcuts = tempEnableMajorShortcuts;
            HierarchyDesigner_Configurable_GeneralSettings.DisableHierarchyDesignerDuringPlayMode = tempDisableHierarchyDesignerDuringPlayMode;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludeFolderProperties = tempExcludeFolderProperties;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludeTransformForGameObjectComponentIcons = tempExcludeTransformForGameObjectComponentIcons;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludeCanvasRendererForGameObjectComponentIcons = tempExcludeCanvasRendererForGameObjectComponentIcons;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludedTags = tempExcludedTags;
            HierarchyDesigner_Configurable_GeneralSettings.ExcludedLayers = tempExcludedLayers;
            HierarchyDesigner_Configurable_GeneralSettings.MaximumComponentIconsAmount = tempMaximumComponentIconsAmount;
            HierarchyDesigner_Configurable_GeneralSettings.SaveSettings();
            generalSettingsHasModifiedChanges = false;
        }

        private void LoadGeneralSettingsData()
        {
            tempLayoutMode = HierarchyDesigner_Configurable_GeneralSettings.LayoutMode;
            tempTreeMode = HierarchyDesigner_Configurable_GeneralSettings.TreeMode;
            tempEnableGameObjectMainIcon = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectMainIcon;
            tempEnableGameObjectComponentIcons = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectComponentIcons;
            tempEnableGameObjectTag = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectTag;
            tempEnableGameObjectLayer = HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectLayer;
            tempEnableHierarchyTree = HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyTree;
            tempEnableHierarchyRows = HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyRows;
            tempEnableHierarchyLines = HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyLines;
            tempEnableHierarchyButtons = HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyButtons;
            tempEnableMajorShortcuts = HierarchyDesigner_Configurable_GeneralSettings.EnableMajorShortcuts;
            tempDisableHierarchyDesignerDuringPlayMode = HierarchyDesigner_Configurable_GeneralSettings.DisableHierarchyDesignerDuringPlayMode;
            tempExcludeFolderProperties = HierarchyDesigner_Configurable_GeneralSettings.ExcludeFolderProperties;
            tempExcludeTransformForGameObjectComponentIcons = HierarchyDesigner_Configurable_GeneralSettings.ExcludeTransformForGameObjectComponentIcons;
            tempExcludeCanvasRendererForGameObjectComponentIcons = HierarchyDesigner_Configurable_GeneralSettings.ExcludeCanvasRendererForGameObjectComponentIcons;
            tempExcludedTags = HierarchyDesigner_Configurable_GeneralSettings.ExcludedTags;
            tempExcludedLayers = HierarchyDesigner_Configurable_GeneralSettings.ExcludedLayers;
            tempMaximumComponentIconsAmount = HierarchyDesigner_Configurable_GeneralSettings.MaximumComponentIconsAmount;
        }

        private void EnableAllGeneralSettingsFeatures(bool enable)
        {
            tempEnableGameObjectMainIcon = enable;
            tempEnableGameObjectComponentIcons = enable;
            tempEnableGameObjectTag = enable;
            tempEnableGameObjectLayer = enable;
            tempEnableHierarchyTree = enable;
            tempEnableHierarchyRows = enable;
            tempEnableHierarchyLines = enable;
            tempEnableHierarchyButtons = enable;
            tempEnableMajorShortcuts = enable;
            tempDisableHierarchyDesignerDuringPlayMode = enable;
            tempExcludeFolderProperties = enable;
            tempExcludeTransformForGameObjectComponentIcons = enable;
            tempExcludeCanvasRendererForGameObjectComponentIcons = enable;
            generalSettingsHasModifiedChanges = true;
        }

        #region General Settings Operations
        private int GetMaskFromList(List<string> list, string[] allItems)
        {
            int mask = 0;
            for (int i = 0; i < allItems.Length; i++)
            {
                if (list.Contains(allItems[i]))
                {
                    mask |= 1 << i;
                }
            }
            return mask;
        }

        private List<string> GetListFromMask(int mask, string[] allItems)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < allItems.Length; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    list.Add(allItems[i]);
                }
            }
            return list;
        }
        #endregion
        #endregion

        #region Design Settings
        private void DrawDesignSettingsComponentIcons()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("GameObject's Component Icons", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempComponentIconsSize = HierarchyDesigner_Shared_GUI.DrawSlider("GameObject's Component Icons Size", designSettingslabelWidth, tempComponentIconsSize, 0.5f, 1.0f);
            tempComponentIconsOffset = HierarchyDesigner_Shared_GUI.DrawIntSlider("GameObject's Component Icons Offset", designSettingslabelWidth, tempComponentIconsOffset, 15, 30);
            tempComponentIconsSpacing = HierarchyDesigner_Shared_GUI.DrawSlider("GameObject's Component Icons Spacing", designSettingslabelWidth, tempComponentIconsSpacing, 0.0f, 10.0f);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsTag()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("GameObject's Tag", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempTagColor = HierarchyDesigner_Shared_GUI.DrawColorField("GameObject's Tag Color", designSettingslabelWidth, tempTagColor);
            tempTagFontSize = HierarchyDesigner_Shared_GUI.DrawIntSlider("GameObject's Tag Font Size", designSettingslabelWidth, tempTagFontSize, 7, 21);
            tempTagFontStyle = HierarchyDesigner_Shared_GUI.DrawEnumPopup("GameObject's Tag Font Style", designSettingslabelWidth, tempTagFontStyle);
            tempTagTextAnchor = HierarchyDesigner_Shared_GUI.DrawEnumPopup("GameObject's Tag Text Anchor", designSettingslabelWidth, tempTagTextAnchor);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsLayer()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("GameObject's Layer", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempLayerColor = HierarchyDesigner_Shared_GUI.DrawColorField("GameObject's Layer Color", designSettingslabelWidth, tempLayerColor);
            tempLayerFontSize = HierarchyDesigner_Shared_GUI.DrawIntSlider("GameObject's Layer Font Size", designSettingslabelWidth, tempLayerFontSize, 7, 21);
            tempLayerFontStyle = HierarchyDesigner_Shared_GUI.DrawEnumPopup("GameObject's Layer Font Style", designSettingslabelWidth, tempLayerFontStyle);
            tempLayerTextAnchor = HierarchyDesigner_Shared_GUI.DrawEnumPopup("GameObject's Layer Text Anchor", designSettingslabelWidth, tempLayerTextAnchor);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsTagAndLayer()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("GameObject's Tag-Layer", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempTagLayerOffset = HierarchyDesigner_Shared_GUI.DrawIntSlider("GameObject's Tag-Layer Offset", designSettingslabelWidth, tempTagLayerOffset, 0, 20);
            tempTagLayerSpacing = HierarchyDesigner_Shared_GUI.DrawIntSlider("GameObject's Tag-Layer Spacing", designSettingslabelWidth, tempTagLayerSpacing, 0, 20);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsHierarchyTree()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Hierarchy Tree", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempHierarchyTreeColor = HierarchyDesigner_Shared_GUI.DrawColorField("Hierarchy Tree Color", designSettingslabelWidth, tempHierarchyTreeColor);
            tempTreeBranchImageType_I = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Tree Branch Image Type I", designSettingslabelWidth, tempTreeBranchImageType_I);
            tempTreeBranchImageType_L = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Tree Branch Image Type L", designSettingslabelWidth, tempTreeBranchImageType_L);
            tempTreeBranchImageType_T = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Tree Branch Image Type T", designSettingslabelWidth, tempTreeBranchImageType_T);
            tempTreeBranchImageType_TerminalBud = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Tree Branch Image Type T-Bud", designSettingslabelWidth, tempTreeBranchImageType_TerminalBud);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsHierarchyLines()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Hierarchy Lines", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempHierarchyLineColor = HierarchyDesigner_Shared_GUI.DrawColorField("Hierarchy Lines Color", designSettingslabelWidth, tempHierarchyLineColor);
            tempHierarchyLineThickness = HierarchyDesigner_Shared_GUI.DrawIntSlider("Hierarchy Lines Thickness", designSettingslabelWidth, tempHierarchyLineThickness, 1, 3);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsFolder()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Folder", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempFolderDefaultTextColor = HierarchyDesigner_Shared_GUI.DrawColorField("Folder Default Text Color", designSettingslabelWidth, tempFolderDefaultTextColor);
            tempFolderDefaultFontSize = HierarchyDesigner_Shared_GUI.DrawIntSlider("Folder Default Font Size", designSettingslabelWidth, tempFolderDefaultFontSize, 7, 21);
            tempFolderDefaultFontStyle = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Folder Default Font Style", designSettingslabelWidth, tempFolderDefaultFontStyle);
            tempFolderDefaultImageColor = HierarchyDesigner_Shared_GUI.DrawColorField("Folder Default Image Color", designSettingslabelWidth, tempFolderDefaultImageColor);
            tempFolderDefaultImageType = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Folder Default Image Type", designSettingslabelWidth, tempFolderDefaultImageType);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsSeparator()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Separator", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempSeparatorDefaultTextColor = HierarchyDesigner_Shared_GUI.DrawColorField("Separator Default Text Color", designSettingslabelWidth, tempSeparatorDefaultTextColor);
            tempSeparatorDefaultIsGradientBackground = HierarchyDesigner_Shared_GUI.DrawToggle("Separator Default Is Gradient Background", designSettingslabelWidth, tempSeparatorDefaultIsGradientBackground);
            tempSeparatorDefaultBackgroundColor = HierarchyDesigner_Shared_GUI.DrawColorField("Separator Default Background Color", designSettingslabelWidth, tempSeparatorDefaultBackgroundColor);
            tempSeparatorDefaultBackgroundGradient = HierarchyDesigner_Shared_GUI.DrawGradientField("Separator Default Background Gradient", designSettingslabelWidth, tempSeparatorDefaultBackgroundGradient != null ? tempSeparatorDefaultBackgroundGradient : new Gradient());
            tempSeparatorDefaultFontSize = HierarchyDesigner_Shared_GUI.DrawIntSlider("Separator Default Font Size", designSettingslabelWidth, tempSeparatorDefaultFontSize, 7, 21);
            tempSeparatorDefaultFontStyle = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Separator Default Font Style", designSettingslabelWidth, tempSeparatorDefaultFontStyle);
            tempSeparatorDefaultTextAnchor = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Separator Default Text Anchor", designSettingslabelWidth, tempSeparatorDefaultTextAnchor);
            tempSeparatorDefaultImageType = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Separator Default Image Type", designSettingslabelWidth, tempSeparatorDefaultImageType);
            tempSeparatorLeftSideTextAnchorOffset = HierarchyDesigner_Shared_GUI.DrawIntSlider("Separator Left Side Text Anchor Offset", designSettingslabelWidth, tempSeparatorLeftSideTextAnchorOffset, 0, 33);
            tempSeparatorRightSideTextAnchorOffset = HierarchyDesigner_Shared_GUI.DrawIntSlider("Separator Right Side Text Anchor Offset", designSettingslabelWidth, tempSeparatorRightSideTextAnchorOffset, 33, 66);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawDesignSettingsLock()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Lock Label", contentLabel);
            EditorGUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempLockColor = HierarchyDesigner_Shared_GUI.DrawColorField("Lock Label Color", designSettingslabelWidth, tempLockColor);
            tempLockFontSize = HierarchyDesigner_Shared_GUI.DrawIntSlider("Lock Label Font Size", designSettingslabelWidth, tempLockFontSize, 7, 21);
            tempLockFontStyle = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Lock Label Font Style", designSettingslabelWidth, tempLockFontStyle);
            tempLockTextAnchor = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Lock Label Text Anchor", designSettingslabelWidth, tempLockTextAnchor);
            if (EditorGUI.EndChangeCheck()) { designSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void UpdateAndSaveDesignSettingsData()
        {
            HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSize = tempComponentIconsSize;
            HierarchyDesigner_Configurable_DesignSettings.ComponentIconsOffset = tempComponentIconsOffset;
            HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSpacing = tempComponentIconsSpacing;
            HierarchyDesigner_Configurable_DesignSettings.HierarchyTreeColor = tempHierarchyTreeColor;
            HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_I = tempTreeBranchImageType_I;
            HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_L = tempTreeBranchImageType_L;
            HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_T = tempTreeBranchImageType_T;
            HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_TerminalBud = tempTreeBranchImageType_TerminalBud;
            HierarchyDesigner_Configurable_DesignSettings.TagColor = tempTagColor;
            HierarchyDesigner_Configurable_DesignSettings.TagTextAnchor = tempTagTextAnchor;
            HierarchyDesigner_Configurable_DesignSettings.TagFontStyle = tempTagFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.TagFontSize = tempTagFontSize;
            HierarchyDesigner_Configurable_DesignSettings.LayerColor = tempLayerColor;
            HierarchyDesigner_Configurable_DesignSettings.LayerTextAnchor = tempLayerTextAnchor;
            HierarchyDesigner_Configurable_DesignSettings.LayerFontStyle = tempLayerFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.LayerFontSize = tempLayerFontSize;
            HierarchyDesigner_Configurable_DesignSettings.TagLayerOffset = tempTagLayerOffset;
            HierarchyDesigner_Configurable_DesignSettings.TagLayerSpacing = tempTagLayerSpacing;
            HierarchyDesigner_Configurable_DesignSettings.HierarchyLineColor = tempHierarchyLineColor;
            HierarchyDesigner_Configurable_DesignSettings.HierarchyLineThickness = tempHierarchyLineThickness;
            HierarchyDesigner_Configurable_DesignSettings.FolderDefaultTextColor = tempFolderDefaultTextColor;
            HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontSize = tempFolderDefaultFontSize;
            HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontStyle = tempFolderDefaultFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageColor = tempFolderDefaultImageColor;
            HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageType = tempFolderDefaultImageType;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextColor = tempSeparatorDefaultTextColor;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultIsGradientBackground = tempSeparatorDefaultIsGradientBackground;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundColor = tempSeparatorDefaultBackgroundColor;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundGradient = tempSeparatorDefaultBackgroundGradient;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontSize = tempSeparatorDefaultFontSize;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontStyle = tempSeparatorDefaultFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextAnchor = tempSeparatorDefaultTextAnchor;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultImageType = tempSeparatorDefaultImageType;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorLeftSideTextAnchorOffset = tempSeparatorLeftSideTextAnchorOffset;
            HierarchyDesigner_Configurable_DesignSettings.SeparatorRightSideTextAnchorOffset = tempSeparatorRightSideTextAnchorOffset;
            HierarchyDesigner_Configurable_DesignSettings.LockColor = tempLockColor;
            HierarchyDesigner_Configurable_DesignSettings.LockTextAnchor = tempLockTextAnchor;
            HierarchyDesigner_Configurable_DesignSettings.LockFontStyle = tempLockFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.LockFontSize = tempLockFontSize;
            HierarchyDesigner_Configurable_DesignSettings.SaveSettings();
            designSettingsHasModifiedChanges = false;
        }

        private void LoadDesignSettingsData()
        {
            tempComponentIconsSize = HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSize;
            tempComponentIconsOffset = HierarchyDesigner_Configurable_DesignSettings.ComponentIconsOffset;
            tempComponentIconsSpacing = HierarchyDesigner_Configurable_DesignSettings.ComponentIconsSpacing;
            tempHierarchyTreeColor = HierarchyDesigner_Configurable_DesignSettings.HierarchyTreeColor;
            tempTreeBranchImageType_I = HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_I;
            tempTreeBranchImageType_L = HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_L;
            tempTreeBranchImageType_T = HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_T;
            tempTreeBranchImageType_TerminalBud = HierarchyDesigner_Configurable_DesignSettings.TreeBranchImageType_TerminalBud;
            tempTagColor = HierarchyDesigner_Configurable_DesignSettings.TagColor;
            tempTagTextAnchor = HierarchyDesigner_Configurable_DesignSettings.TagTextAnchor;
            tempTagFontStyle = HierarchyDesigner_Configurable_DesignSettings.TagFontStyle;
            tempTagFontSize = HierarchyDesigner_Configurable_DesignSettings.TagFontSize;
            tempLayerColor = HierarchyDesigner_Configurable_DesignSettings.LayerColor;
            tempLayerTextAnchor = HierarchyDesigner_Configurable_DesignSettings.LayerTextAnchor;
            tempLayerFontStyle = HierarchyDesigner_Configurable_DesignSettings.LayerFontStyle;
            tempLayerFontSize = HierarchyDesigner_Configurable_DesignSettings.LayerFontSize;
            tempTagLayerOffset = HierarchyDesigner_Configurable_DesignSettings.TagLayerOffset;
            tempTagLayerSpacing = HierarchyDesigner_Configurable_DesignSettings.TagLayerSpacing;
            tempHierarchyLineColor = HierarchyDesigner_Configurable_DesignSettings.HierarchyLineColor;
            tempHierarchyLineThickness = HierarchyDesigner_Configurable_DesignSettings.HierarchyLineThickness;
            tempFolderDefaultTextColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultTextColor;
            tempFolderDefaultFontSize = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontSize;
            tempFolderDefaultFontStyle = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontStyle;
            tempFolderDefaultImageColor = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageColor;
            tempFolderDefaultImageType = HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageType;
            tempSeparatorDefaultTextColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextColor;
            tempSeparatorDefaultIsGradientBackground = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultIsGradientBackground;
            tempSeparatorDefaultBackgroundColor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundColor;
            tempSeparatorDefaultBackgroundGradient = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundGradient;
            tempSeparatorDefaultFontSize = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontSize;
            tempSeparatorDefaultFontStyle = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontStyle;
            tempSeparatorDefaultTextAnchor = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextAnchor;
            tempSeparatorDefaultImageType = HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultImageType;
            tempSeparatorLeftSideTextAnchorOffset = HierarchyDesigner_Configurable_DesignSettings.SeparatorLeftSideTextAnchorOffset;
            tempSeparatorRightSideTextAnchorOffset = HierarchyDesigner_Configurable_DesignSettings.SeparatorRightSideTextAnchorOffset;
            tempLockColor = HierarchyDesigner_Configurable_DesignSettings.LockColor;
            tempLockTextAnchor = HierarchyDesigner_Configurable_DesignSettings.LockTextAnchor;
            tempLockFontStyle = HierarchyDesigner_Configurable_DesignSettings.LockFontStyle;
            tempLockFontSize = HierarchyDesigner_Configurable_DesignSettings.LockFontSize;
        }
        #endregion

        #region Shortcut Settings
        private void DrawMajorShortcuts()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Major Shortcuts", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempToggleGameObjectActiveStateKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Toggle GameObject Active State Key Code", majorShortcutEnumToggleLabelWidth, tempToggleGameObjectActiveStateKeyCode);
            tempToggleLockStateKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Toggle GameObject Lock State Key Code", majorShortcutEnumToggleLabelWidth, tempToggleLockStateKeyCode);
            tempChangeTagLayerKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Change Selected Tag, Layer Key Code", majorShortcutEnumToggleLabelWidth, tempChangeTagLayerKeyCode);
            tempRenameSelectedGameObjectsKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Rename Selected GameObjects Key Code", majorShortcutEnumToggleLabelWidth, tempRenameSelectedGameObjectsKeyCode);
            if (EditorGUI.EndChangeCheck()) { shortcutSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawMinorShortcuts()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Minor Shortcuts", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            minorShortcutSettingsScroll = EditorGUILayout.BeginScrollView(minorShortcutSettingsScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            foreach (string shortcutId in minorShortcutIdentifiers)
            {
                ShortcutBinding currentBinding = ShortcutManager.instance.GetShortcutBinding(shortcutId);
                string[] parts = shortcutId.Split('/');
                string commandName = parts[parts.Length - 1];

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(commandName + ":", fieldsLabel, GUILayout.Width(minorShortcutCommandLabelWidth));

                bool hasKeyCombination = false;
                foreach (KeyCombination kc in currentBinding.keyCombinationSequence)
                {
                    if (!hasKeyCombination)
                    {
                        hasKeyCombination = true;
                        GUILayout.Label(kc.ToString(), fieldsLabel, GUILayout.MinWidth(minorShortcutLabelWidth));
                    }
                    else
                    {
                        GUILayout.Label(" + " + kc.ToString(), fieldsLabel, GUILayout.MinWidth(minorShortcutLabelWidth));
                    }
                }
                if (!hasKeyCombination)
                {
                    GUILayout.Label("unassigned shortcut", unassignedLabel, GUILayout.MinWidth(minorShortcutLabelWidth));
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            GUILayout.Space(defaultMarginSpacing);
            EditorGUILayout.HelpBox("To modify minor shortcuts, please go to: Edit/Shortcuts.../Hierarchy Designer.\nYou can click the button below for quick access, then in the category section, search for Hierarchy Designer.", MessageType.Info);
        }

        private void UpdateAndSaveShortcutSettingsData()
        {
            HierarchyDesigner_Configurable_ShortcutsSettings.ToggleGameObjectActiveStateKeyCode = tempToggleGameObjectActiveStateKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.ToggleLockStateKeyCode = tempToggleLockStateKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.ChangeTagLayerKeyCode = tempChangeTagLayerKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.RenameSelectedGameObjectsKeyCode = tempRenameSelectedGameObjectsKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.SaveSettings();
            shortcutSettingsHasModifiedChanges = false;
        }

        private void LoadShortcutSettingsData()
        {
            tempToggleGameObjectActiveStateKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.ToggleGameObjectActiveStateKeyCode;
            tempToggleLockStateKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.ToggleLockStateKeyCode;
            tempChangeTagLayerKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.ChangeTagLayerKeyCode;
            tempRenameSelectedGameObjectsKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.RenameSelectedGameObjectsKeyCode;
        }
        #endregion

        #region Advanced Settings
        private void DrawAdvancedCoreFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Core Features", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempHierarchyLocation = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Designer Location", advancedSettingsEnumPopupLabelWidth, tempHierarchyLocation);
            tempMainIconUpdateMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Main Icon Update Mode", advancedSettingsEnumPopupLabelWidth, tempMainIconUpdateMode);
            tempComponentsIconsUpdateMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Component Icons Update Mode", advancedSettingsEnumPopupLabelWidth, tempComponentsIconsUpdateMode);
            tempHierarchyTreeUpdateMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Hierarchy Tree Update Mode", advancedSettingsEnumPopupLabelWidth, tempHierarchyTreeUpdateMode);
            tempTagUpdateMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Tag Update Mode", advancedSettingsEnumPopupLabelWidth, tempTagUpdateMode);
            tempLayerUpdateMode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Layer Update Mode", advancedSettingsEnumPopupLabelWidth, tempLayerUpdateMode);
            if (EditorGUI.EndChangeCheck()) { advancedSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawAdvancedMainIconFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("GameObject's Main Icon", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempEnableDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Dynamic Background", advancedSettingsToggleLabelWidth, tempEnableDynamicBackgroundForGameObjectMainIcon);
            tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Precise Rect For Dynamic Background", advancedSettingsToggleLabelWidth, tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon);
            if (EditorGUI.EndChangeCheck()) { advancedSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawAdvancedComponentIconsFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("GameObject's Component Icons", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempEnableCustomizationForGameObjectComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Design Customization For Component Icons", advancedSettingsToggleLabelWidth, tempEnableCustomizationForGameObjectComponentIcons);
            tempEnableTooltipOnComponentIconHovered = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Tooltip For Component Icons", advancedSettingsToggleLabelWidth, tempEnableTooltipOnComponentIconHovered);
            tempEnableActiveStateEffectForComponentIcons = HierarchyDesigner_Shared_GUI.DrawToggle("Enable Active State Effect For Component Icons", advancedSettingsToggleLabelWidth, tempEnableActiveStateEffectForComponentIcons);
            tempDisableComponentIconsForInactiveGameObjects = HierarchyDesigner_Shared_GUI.DrawToggle("Disable Component Icons For Inactive GameObjects", advancedSettingsToggleLabelWidth, tempDisableComponentIconsForInactiveGameObjects);
            if (EditorGUI.EndChangeCheck()) { advancedSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawAdvancedFolderFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Folders", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempIncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder = HierarchyDesigner_Shared_GUI.DrawToggle("Include Editor Utilities For Hierarchy Designer's Folder", advancedSettingsToggleLabelWidth, tempIncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder);
            if (EditorGUI.EndChangeCheck()) { advancedSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawAdvancedSeparatorFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Separators", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempIncludeBackgroundImageForGradientBackground = HierarchyDesigner_Shared_GUI.DrawToggle("Include Background Image For Gradient Background", advancedSettingsToggleLabelWidth, tempIncludeBackgroundImageForGradientBackground);
            if (EditorGUI.EndChangeCheck()) { advancedSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void DrawAdvancedHierarchyToolsFeatures()
        {
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Hierarchy Tools", contentLabel);
            GUILayout.Space(contentLabelSpacing);
            EditorGUI.BeginChangeCheck();
            tempExcludeFoldersFromCountSelectToolCalculations = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Folders From Count-Select Tool Calculations", advancedSettingsToggleLabelWidth, tempExcludeFoldersFromCountSelectToolCalculations);
            tempExcludeSeparatorsFromCountSelectToolCalculations = HierarchyDesigner_Shared_GUI.DrawToggle("Exclude Separators From Count-Select Tool Calculations", advancedSettingsToggleLabelWidth, tempExcludeSeparatorsFromCountSelectToolCalculations);
            if (EditorGUI.EndChangeCheck()) { advancedSettingsHasModifiedChanges = true; }
            EditorGUILayout.EndVertical();
        }

        private void UpdateAndSaveAdvancedSettingsData()
        {
            bool hierarchyLocationChanged = tempHierarchyLocation != HierarchyDesigner_Configurable_AdvancedSettings.HierarchyLocation;

            HierarchyDesigner_Configurable_AdvancedSettings.HierarchyLocation = tempHierarchyLocation;
            HierarchyDesigner_Configurable_AdvancedSettings.MainIconUpdateMode = tempMainIconUpdateMode;
            HierarchyDesigner_Configurable_AdvancedSettings.ComponentsIconsUpdateMode = tempComponentsIconsUpdateMode;
            HierarchyDesigner_Configurable_AdvancedSettings.HierarchyTreeUpdateMode = tempHierarchyTreeUpdateMode;
            HierarchyDesigner_Configurable_AdvancedSettings.TagUpdateMode = tempTagUpdateMode;
            HierarchyDesigner_Configurable_AdvancedSettings.LayerUpdateMode = tempLayerUpdateMode;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableDynamicBackgroundForGameObjectMainIcon = tempEnableDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Configurable_AdvancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableCustomizationForGameObjectComponentIcons = tempEnableCustomizationForGameObjectComponentIcons;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableTooltipOnComponentIconHovered = tempEnableTooltipOnComponentIconHovered;
            HierarchyDesigner_Configurable_AdvancedSettings.EnableActiveStateEffectForComponentIcons = tempEnableActiveStateEffectForComponentIcons;
            HierarchyDesigner_Configurable_AdvancedSettings.DisableComponentIconsForInactiveGameObjects = tempDisableComponentIconsForInactiveGameObjects;
            HierarchyDesigner_Configurable_AdvancedSettings.IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder = tempIncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder;
            HierarchyDesigner_Configurable_AdvancedSettings.IncludeBackgroundImageForGradientBackground = tempIncludeBackgroundImageForGradientBackground;
            HierarchyDesigner_Configurable_AdvancedSettings.ExcludeFoldersFromCountSelectToolCalculations = tempExcludeFoldersFromCountSelectToolCalculations;
            HierarchyDesigner_Configurable_AdvancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations = tempExcludeSeparatorsFromCountSelectToolCalculations;
            HierarchyDesigner_Configurable_AdvancedSettings.SaveSettings();
            advancedSettingsHasModifiedChanges = false;

            if (hierarchyLocationChanged)
            {
                HierarchyDesigner_Configurable_AdvancedSettings.GenerateConstantsFile(tempHierarchyLocation);
            }
        }

        private void LoadAdvancedSettingsData()
        {
            tempHierarchyLocation = HierarchyDesigner_Configurable_AdvancedSettings.HierarchyLocation;
            tempMainIconUpdateMode = HierarchyDesigner_Configurable_AdvancedSettings.MainIconUpdateMode;
            tempComponentsIconsUpdateMode = HierarchyDesigner_Configurable_AdvancedSettings.ComponentsIconsUpdateMode;
            tempHierarchyTreeUpdateMode = HierarchyDesigner_Configurable_AdvancedSettings.HierarchyTreeUpdateMode;
            tempTagUpdateMode = HierarchyDesigner_Configurable_AdvancedSettings.TagUpdateMode;
            tempLayerUpdateMode = HierarchyDesigner_Configurable_AdvancedSettings.LayerUpdateMode;
            tempEnableDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Configurable_AdvancedSettings.EnableDynamicBackgroundForGameObjectMainIcon;
            tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = HierarchyDesigner_Configurable_AdvancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            tempEnableCustomizationForGameObjectComponentIcons = HierarchyDesigner_Configurable_AdvancedSettings.EnableCustomizationForGameObjectComponentIcons;
            tempEnableTooltipOnComponentIconHovered = HierarchyDesigner_Configurable_AdvancedSettings.EnableTooltipOnComponentIconHovered;
            tempEnableActiveStateEffectForComponentIcons = HierarchyDesigner_Configurable_AdvancedSettings.EnableActiveStateEffectForComponentIcons;
            tempDisableComponentIconsForInactiveGameObjects = HierarchyDesigner_Configurable_AdvancedSettings.DisableComponentIconsForInactiveGameObjects;
            tempIncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder = HierarchyDesigner_Configurable_AdvancedSettings.IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder;
            tempIncludeBackgroundImageForGradientBackground = HierarchyDesigner_Configurable_AdvancedSettings.IncludeBackgroundImageForGradientBackground;
            tempExcludeFoldersFromCountSelectToolCalculations = HierarchyDesigner_Configurable_AdvancedSettings.ExcludeFoldersFromCountSelectToolCalculations;
            tempExcludeSeparatorsFromCountSelectToolCalculations = HierarchyDesigner_Configurable_AdvancedSettings.ExcludeSeparatorsFromCountSelectToolCalculations;
        }

        private void EnableAllAdvancedSettingsFeatures(bool enable)
        {
            tempEnableDynamicBackgroundForGameObjectMainIcon = enable;
            tempEnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = enable;
            tempEnableCustomizationForGameObjectComponentIcons = enable;
            tempEnableTooltipOnComponentIconHovered = enable;
            tempEnableActiveStateEffectForComponentIcons = enable;
            tempDisableComponentIconsForInactiveGameObjects = enable;
            tempIncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder = enable;
            tempIncludeBackgroundImageForGradientBackground = enable;
            tempExcludeFoldersFromCountSelectToolCalculations = enable;
            tempExcludeSeparatorsFromCountSelectToolCalculations = enable;
            advancedSettingsHasModifiedChanges = true;
        }
        #endregion
        #endregion
        #endregion

        private void OnDestroy()
        {
            string message = "The following settings have been modified: ";
            List<string> modifiedSettingsList = new List<string>();

            if (folderHasModifiedChanges) modifiedSettingsList.Add("Folders");
            if (separatorHasModifiedChanges) modifiedSettingsList.Add("Separators");
            if (generalSettingsHasModifiedChanges) modifiedSettingsList.Add("General Settings");
            if (designSettingsHasModifiedChanges) modifiedSettingsList.Add("Design Settings");
            if (shortcutSettingsHasModifiedChanges) modifiedSettingsList.Add("Shortcut Settings");
            if (advancedSettingsHasModifiedChanges) modifiedSettingsList.Add("Advanced Settings");

            if (modifiedSettingsList.Count > 0)
            {
                message += string.Join(", ", modifiedSettingsList) + ".\n\nWould you like to save the changes?";
                bool shouldSave = EditorUtility.DisplayDialog("Data Has Been Modified!", message, "Save", "Don't Save");

                if (shouldSave)
                {
                    if (folderHasModifiedChanges) UpdateAndSaveFoldersData();
                    if (separatorHasModifiedChanges) UpdateAndSaveSeparatorsData();
                    if (generalSettingsHasModifiedChanges) UpdateAndSaveGeneralSettingsData();
                    if (designSettingsHasModifiedChanges) UpdateAndSaveDesignSettingsData();
                    if (shortcutSettingsHasModifiedChanges) UpdateAndSaveShortcutSettingsData();
                    if (advancedSettingsHasModifiedChanges) UpdateAndSaveAdvancedSettingsData();
                }
            }

            folderHasModifiedChanges = false;
            separatorHasModifiedChanges = false;
            generalSettingsHasModifiedChanges = false;
            designSettingsHasModifiedChanges = false;
            shortcutSettingsHasModifiedChanges = false;
            advancedSettingsHasModifiedChanges = false;

            HierarchyDesigner_Manager_State.instance.currentWindow = currentWindow;
            //HierarchyDesigner_Manager_State.instance.Save();
        }
    }
}
#endif