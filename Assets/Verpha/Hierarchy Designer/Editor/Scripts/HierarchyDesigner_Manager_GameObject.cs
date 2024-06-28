#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Manager_GameObject
    {
        #region Properties
        #region Const Values
        private const string inspectorWindow = "Inspector";
        private const string hierarchyWindow = "Hierarchy";
        private const string warningIconTexture = "console.warnicon";
        private const string missingComponentMessage = "Missing Component";
        private const int hierarchyWindowOffsetLeft = 32;
        private const int hierarchyWindowOffsetRight = -53;
        private const float alphaValueForInactiveGameObjects = 0.5f;
        private const int mainIconSelectionHeight = 16;
        private const float defaultComponentIconsSize = 1f;
        private const float defaultComponentIconsSpacing = 2f;
        private const int defaultComponentIconsOffset = 20;
        private const int hierarchyTreeOffset = 22;
        private const int hierarchyTreeFillLinesOffset = 14;
        private const int lockButtonWidth = 20;
        private const int activeButtonWidth = 30;
        private const float lockIconOffset = 15f;
        private const int additionalLockLabelWidth = 5;
        private const int dockedPropertiesBaseOffset = 14;
        private const int splitModeOffset = 12;
        private const string lockedLabel = "(Locked)";
        private const string separatorMessage = "Separators are EditorOnly, meaning they will not be present in your game's build. If you want a GameObject parent to organize your GameObjects, use a folder instead.";
        private const string lockedGameObjectMessage = "This gameObject is locked, components are not available for editing.";
        private const string separatorTag = "EditorOnly";
        private const string separatorPrefix = "//";
        #endregion
        #region Resources
        private static readonly Texture2D defaultTexture = HierarchyDesigner_Shared_Resources.DefaultTexture;
        private static readonly Texture2D lockIcon = HierarchyDesigner_Shared_Resources.LockIcon;
        private static readonly Texture2D treeBranchIconI = HierarchyDesigner_Shared_Resources.TreeBranchIcon_I;
        private static readonly Texture2D treeBranchIconL = HierarchyDesigner_Shared_Resources.TreeBranchIcon_L;
        private static readonly Texture2D treeBranchIconT = HierarchyDesigner_Shared_Resources.TreeBranchIcon_T;
        private static readonly Texture2D treeBranchIconTerminalBud = HierarchyDesigner_Shared_Resources.TreeBranchIcon_TerminalBud;
        #endregion
        #region Component Types
        private static readonly Type typeTransform = typeof(Transform);
        #endregion
        #region Cache
        #region GUI
        private static readonly Color activeColor = new Color(1f, 1f, 1f, 1f);
        private static readonly Color inactiveColor = new Color(1f, 1f, 1f, alphaValueForInactiveGameObjects);
        private static GUIStyle tagStyle;
        private static GUIStyle TagStyle
        {
            get
            {
                if (tagStyle == null)
                {
                    tagStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = tagTextAnchor,
                        fontStyle = tagFontStyle,
                        fontSize = tagFontSize,
                        normal = { textColor = tagColor }
                    };
                }
                else
                {
                    tagStyle.alignment = tagTextAnchor;
                    tagStyle.fontStyle = tagFontStyle;
                    tagStyle.fontSize = tagFontSize;
                    tagStyle.normal.textColor = tagColor;
                }
                return tagStyle;
            }
        }
        private static GUIStyle layerStyle;
        private static GUIStyle LayerStyle
        {
            get
            {
                if (layerStyle == null)
                {
                    layerStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = layerTextAnchor,
                        fontStyle = layerFontStyle,
                        fontSize = layerFontSize,
                        normal = { textColor = layerColor }
                    };
                }
                else
                {
                    layerStyle.alignment = layerTextAnchor;
                    layerStyle.fontStyle = layerFontStyle;
                    layerStyle.fontSize = layerFontSize;
                    layerStyle.normal.textColor = layerColor;
                }
                return layerStyle;
            }
        }
        private static GUIStyle lockStyle;
        private static GUIStyle LockStyle
        {
            get
            {
                if (lockStyle == null)
                {
                    lockStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = lockTextAnchor,
                        fontStyle = lockFontStyle,
                        fontSize = lockFontSize,
                        normal = { textColor = lockColor }
                    };
                }
                else
                {
                    lockStyle.alignment = lockTextAnchor;
                    lockStyle.fontStyle = lockFontStyle;
                    lockStyle.fontSize = lockFontSize;
                    lockStyle.normal.textColor = lockColor;
                }
                return lockStyle;
            }
        }
        #endregion
        #region Data
        private static HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode layoutMode;
        private static bool enableGameObjectMainIcon;
        private static bool enableGameObjectComponentIcons;
        private static bool enableHierarchyTree;
        private static bool enableGameObjectTag;
        private static bool enableGameObjectLayer;
        private static bool enableHierarchyRows;
        private static bool enableHierarchyButtons;
        private static bool enableMajorShortcuts;
        private static bool disableEditorDesignerMajorOperationsDuringPlayMode;
        private static bool enableDynamicChangesCheckForGameObjectMainIcon;
        private static bool enableDynamicBackgroundForGameObjectMainIcon;
        private static bool enablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
        private static bool enableCustomizationForGameObjectComponentIcons;
        private static bool enableTooltipOnComponentIconHovered;
        private static bool enableActiveStateEffectForComponentIcons;
        private static bool disableComponentIconsForInactiveGameObjects;
        private static bool includeBackgroundImageForGradientBackgroundCache;
        private static bool excludeFolderProperties;
        private static bool excludeTransformComponent;
        private static bool excludeCanvasRendererComponent;
        private static float componentIconsSize;
        private static float componentIconsSpacing;
        private static int componentIconsOffset;
        private static Color hierarchyTreeColor;
        private static Color tagColor;
        private static TextAnchor tagTextAnchor;
        private static FontStyle tagFontStyle;
        private static int tagFontSize;
        private static Color layerColor;
        private static TextAnchor layerTextAnchor;
        private static FontStyle layerFontStyle;
        private static int layerFontSize;
        private static int tagLayerOffset;
        private static int tagLayerSpacing;
        private static List<string> excludedTags;
        private static List<string> excludedLayers;
        private static Color lockColor;
        private static TextAnchor lockTextAnchor;
        private static FontStyle lockFontStyle;
        private static int lockFontSize;
        private static KeyCode toggleGameObjectActiveStateKeyCode;
        private static KeyCode toggleLockStateKeyCode;
        private static KeyCode changeTagLayerKeyCode;
        private static KeyCode renameSelectedGameObjectsKeyCode;
        #endregion
        #region Dictionaries
        private static Dictionary<int, Texture2D> gameObjectMainIconCache = new Dictionary<int, Texture2D>();
        private static Dictionary<int, bool> gameObjectMainIconDirtyFlagCache = new Dictionary<int, bool>();
        private static Dictionary<int, List<(Component component, Texture2D icon)>> gameObjectComponentIconsCache = new Dictionary<int, List<(Component, Texture2D)>>();
        private static Dictionary<int, Texture2D> hierarchyTreeCache = new Dictionary<int, Texture2D>();
        private static Dictionary<int, string> gameObjectTagCache = new Dictionary<int, string>();
        private static Dictionary<int, string> gameObjectLayerCache = new Dictionary<int, string>();
        private static Dictionary<int, (Color folderColor, HierarchyDesigner_Configurable_Folder.FolderImageType folderImageType)> folderCache = new Dictionary<int, (Color, HierarchyDesigner_Configurable_Folder.FolderImageType)>();
        private static Dictionary<int, (Color textColor, bool isGradientBackground, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, HierarchyDesigner_Configurable_Separator.SeparatorImageType separatorImageType)> separatorCache = new Dictionary<int, (Color textColor, bool isGradientBackground, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, HierarchyDesigner_Configurable_Separator.SeparatorImageType separatorImageType)>();
        private static Dictionary<int, Texture2D> gradientTextureCache = new Dictionary<int, Texture2D>();
        #endregion
        #endregion
        #endregion

        #region Initialization
        public static void Initialize()
        {
            SubscribeToEvents();
        }

        private static void SubscribeToEvents()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            Editor.finishedDefaultHeaderGUI -= OnPostHeaderGUI;
            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }
        #endregion

        #region Events
        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            #region Header
            if (HierarchyDesigner_Manager_Editor.IsPlaying && disableEditorDesignerMajorOperationsDuringPlayMode) { return; }
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject == null) { return; }
            #endregion

            #region Events
            Event currentEvent = Event.current;
            if (enableDynamicChangesCheckForGameObjectMainIcon && currentEvent.type == EventType.Layout) { gameObjectMainIconDirtyFlagCache[instanceID] = true; }
            if (currentEvent.type != EventType.Repaint && !(gameObject.tag == separatorTag && gameObject.name.StartsWith(separatorPrefix)))
            {
                if (enableHierarchyButtons) { ProcessHierarchyButtons(gameObject, selectionRect); }
                if (enableMajorShortcuts)
                {
                    if (IsShortcutPressed(toggleGameObjectActiveStateKeyCode))
                    {
                        GameObject[] selectedGameObjects = Selection.gameObjects;
                        if (selectedGameObjects != null && selectedGameObjects.Length > 1)
                        {
                            ProcessToggleGameObjectActiveStateMajorShortcut(selectedGameObjects);
                        }
                        else
                        {
                            if (selectionRect.Contains(currentEvent.mousePosition)) { ProcessToggleGameObjectActiveStateMajorShortcut(new GameObject[] { gameObject }); }
                        }
                    }
                    if (IsShortcutPressed(toggleLockStateKeyCode))
                    {
                        GameObject[] selectedGameObjects = Selection.gameObjects;
                        if (selectedGameObjects != null && selectedGameObjects.Length > 1)
                        {
                            ProcessToggleGameObjectLockStateMajorShortcut(selectedGameObjects);
                        }
                        else
                        {
                            if (selectionRect.Contains(currentEvent.mousePosition)) { ProcessToggleGameObjectLockStateMajorShortcut(new GameObject[] { gameObject }); }
                        }
                    }
                    if (IsShortcutPressed(changeTagLayerKeyCode)) { ProcessTagLayerMajorShortcut(gameObject, selectionRect, instanceID); };
                    if (IsShortcutPressed(renameSelectedGameObjectsKeyCode)) { ProcessRenameMajorShortcut(); }
                }
                return;
            }
            #endregion

            #region Features
            if (separatorCache.ContainsKey(gameObject.GetInstanceID()) || (gameObject.tag == separatorTag && gameObject.name.StartsWith(separatorPrefix))) { DrawSeparator(gameObject, selectionRect, instanceID); return; }
            if (enableHierarchyRows) { DrawHierarchyRows(selectionRect); }
            if (enableGameObjectMainIcon) { DrawGameObjectMainIcon(gameObject, selectionRect, instanceID); }
            if (enableHierarchyTree) { DrawHierarchyTree(gameObject, selectionRect, instanceID); }
            if (enableHierarchyButtons) { DrawHierarchyButtons(gameObject, selectionRect); }
            if (folderCache.ContainsKey(instanceID) || gameObject.GetComponent<HierarchyDesignerFolder>()) { DrawFolder(gameObject, selectionRect, instanceID); }
            if ((gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable) { DrawGameObjectLock(gameObject, selectionRect); return; }
            if ((folderCache.ContainsKey(instanceID) || gameObject.GetComponent<HierarchyDesignerFolder>()) && excludeFolderProperties) { return; }
            if (enableGameObjectComponentIcons) { DrawGameObjectComponentIcons(gameObject, selectionRect, instanceID); }
            if (enableGameObjectTag) { DrawGameObjectTag(gameObject, selectionRect, instanceID); }
            if (enableGameObjectLayer) { DrawGameObjectLayer(gameObject, selectionRect, instanceID); }
            #endregion
        }

        private static void OnHierarchyChanged()
        {
            #region Header
            if (HierarchyDesigner_Manager_Editor.IsPlaying && disableEditorDesignerMajorOperationsDuringPlayMode) { return; }
            if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == inspectorWindow) { return; }
            #endregion

            #region Features
            if (enableGameObjectMainIcon) { UpdateGameObjectMainIconCacheIfNeeded(); }
            if (enableGameObjectComponentIcons) { UpdateGameObjectComponentIconsCacheIfNeeded(); }
            if (enableHierarchyTree) { UpdateHierarchyTreeCacheIfNeeded(); }
            if (enableGameObjectTag) { UpdateTagCacheIfNeeded(); }
            if (enableGameObjectLayer) { UpdateLayerCacheIfNeeded(); }
            #endregion
        }

        private static void OnPostHeaderGUI(Editor editor)
        {
            #region Header
            if (HierarchyDesigner_Manager_Editor.IsPlaying && disableEditorDesignerMajorOperationsDuringPlayMode) { return; }
            if (editor.target is not GameObject gameObject) { return; }
            #endregion

            #region Features
            if ((gameObject.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable) return;
            if (separatorCache.ContainsKey(gameObject.GetInstanceID()) || (gameObject.tag == separatorTag && gameObject.name.StartsWith(separatorPrefix))) { EditorGUILayout.HelpBox(separatorMessage, MessageType.Info, true); }
            else { EditorGUILayout.HelpBox(lockedGameObjectMessage, MessageType.Info, true); }
            #endregion
        }
        #endregion

        #region Methods
        #region Main Icon
        private static void DrawGameObjectMainIcon(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            DrawBackground(selectionRect, instanceID);
            Texture2D mainIcon = DecideGameObjectMainIcon(gameObject, instanceID);
            GUI.color = gameObject.activeInHierarchy ? activeColor : inactiveColor;
            GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.height, mainIconSelectionHeight), mainIcon);
            GUI.color = activeColor;
        }

        private static void DrawBackground(Rect selectionRect, int instanceID)
        {
            GUI.color = SetBackgroundColorBasedOnState(selectionRect, instanceID);
            GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.height, mainIconSelectionHeight), defaultTexture);
            GUI.color = activeColor;
        }

        private static Color SetBackgroundColorBasedOnState(Rect selectionRect, int instanceID)
        {
            bool isRow = false;
            if (enableHierarchyRows) { isRow = (int)(selectionRect.y / selectionRect.height) % 2 != 0; }
            if (enableDynamicBackgroundForGameObjectMainIcon)
            {
                Rect finalSelectionRect = selectionRect;
                if (enablePreciseRectForDynamicBackgroundForGameObjectMainIcon)
                {
                    const float leftOffset = 0f;
                    const float rightOffset = 15.25f;
                    finalSelectionRect = new Rect(leftOffset, selectionRect.yMin, selectionRect.width + rightOffset + selectionRect.xMin, selectionRect.height);
                }

                bool isHovering = finalSelectionRect.Contains(Event.current.mousePosition);
                bool isSelected = Array.IndexOf(Selection.instanceIDs, instanceID) >= 0;
                bool isHierarchyFocused = EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == hierarchyWindow;

                if (isSelected && !isHierarchyFocused)
                {
                    return isRow ? HierarchyDesigner_Shared_ColorUtility.GetHighlightedFocusedEditorRowColor() : HierarchyDesigner_Shared_ColorUtility.GetHighlightedFocusedEditorColor();
                }
                else if (isSelected)
                {
                    return isRow ? HierarchyDesigner_Shared_ColorUtility.GetSelectedEditorRowColor() : HierarchyDesigner_Shared_ColorUtility.GetSelectedEditorColor();
                }
                else if (isHovering)
                {
                    return isRow ? HierarchyDesigner_Shared_ColorUtility.GetHighlightedEditorRowColor() : HierarchyDesigner_Shared_ColorUtility.GetHighlightedEditorColor();
                }
                return isRow ? HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorRowBackgroundColor() : HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorBackgroundColor();
            }
            return isRow ? HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorRowBackgroundColor() : HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorBackgroundColor();
        }

        private static Texture2D DecideGameObjectMainIcon(GameObject gameObject, int instanceID)
        {
            if (gameObjectMainIconCache.TryGetValue(instanceID, out Texture2D cachedIcon) && !gameObjectMainIconDirtyFlagCache[instanceID])
            {
                return cachedIcon;
            }
            else
            {
                Texture2D icon;
                Component[] components = gameObject.GetComponents<Component>();
                if (components.Length == 1)
                {
                    icon = EditorGUIUtility.ObjectContent(components[0], typeTransform).image as Texture2D;
                }
                else
                {
                    if (components[0] is not RectTransform)
                    {
                        Component component = components[1];
                        if (component != null)
                        {
                            icon = EditorGUIUtility.ObjectContent(component, component.GetType()).image as Texture2D;
                        }
                        else
                        {
                            icon = EditorGUIUtility.FindTexture(warningIconTexture);
                        }
                    }
                    else
                    {
                        icon = DetermineUIIcon(components);
                    }
                }
                icon = icon ?? defaultTexture;
                gameObjectMainIconCache[instanceID] = icon;
                gameObjectMainIconDirtyFlagCache[instanceID] = false;
                return icon;
            }
        }

        private static Texture2D DetermineUIIcon(Component[] components)
        {
            if (components.Length < 2) return null;

            int mainIconIndex = -1;
            if (components.Length >= 4 && components[1] is CanvasRenderer && components[2] is UnityEngine.UI.Image)
            {
                mainIconIndex = 3;
            }
            else if (components.Length >= 3 && components[1] is CanvasRenderer)
            {
                mainIconIndex = 2;
            }
            else if (components.Length >= 2)
            {
                mainIconIndex = 1;
            }

            if (mainIconIndex != -1 && mainIconIndex < components.Length)
            {
                Component mainComponent = components[mainIconIndex];
                return EditorGUIUtility.ObjectContent(mainComponent, mainComponent.GetType()).image as Texture2D;
            }
            return null;
        }

        private static void UpdateGameObjectMainIconCacheIfNeeded()
        {
            List<int> instanceIDsToRemove = new List<int>();
            foreach (int key in gameObjectMainIconCache.Keys)
            {
                if (EditorUtility.InstanceIDToObject(key) as GameObject == null)
                {
                    instanceIDsToRemove.Add(key);
                }
            }
            foreach (int id in instanceIDsToRemove)
            {
                gameObjectMainIconCache.Remove(id);
                gameObjectMainIconDirtyFlagCache.Remove(id);
            }
        }
        #endregion

        #region Component Icons
        private static void DrawGameObjectComponentIcons(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            if (disableComponentIconsForInactiveGameObjects && !gameObject.activeInHierarchy) { return; }
            if (!gameObjectComponentIconsCache.TryGetValue(instanceID, out List<(Component component, Texture2D icon)> cachedIcons) || CheckComponentsChanged(gameObject, cachedIcons))
            {
                cachedIcons = CacheComponentIcons(gameObject);
                gameObjectComponentIconsCache[instanceID] = cachedIcons;
            }

            float iconSizeMultiplier = defaultComponentIconsSize;
            float iconsSpacing = defaultComponentIconsSpacing;
            int iconAdditionalOffset = defaultComponentIconsOffset;
            if (enableCustomizationForGameObjectComponentIcons)
            {
                iconSizeMultiplier = componentIconsSize;
                iconsSpacing = componentIconsSpacing;
                iconAdditionalOffset = componentIconsOffset;
            }
            float iconSize = selectionRect.height * iconSizeMultiplier;

            float iconOffset = 0f;
            switch (layoutMode)
            {
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Consecutive:
                    iconOffset = selectionRect.x + (GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x) + iconAdditionalOffset;
                    break;

                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Split:
                    if (enableHierarchyButtons) { iconOffset += lockButtonWidth + activeButtonWidth + 5f; }
                    iconOffset += cachedIcons.Count * (iconSize + iconsSpacing);
                    selectionRect.x = EditorGUIUtility.currentViewWidth - dockedPropertiesBaseOffset - iconOffset;
                    iconOffset = selectionRect.x - componentIconsOffset + 20;
                    break;
                
                default:
                    if (enableHierarchyButtons) { iconOffset += lockButtonWidth + activeButtonWidth + 5f; }
                    if (enableGameObjectLayer)
                    {
                        string layer;
                        if (!gameObjectLayerCache.TryGetValue(instanceID, out layer) || layer != LayerMask.LayerToName(gameObject.layer))
                        {
                            layer = LayerMask.LayerToName(gameObject.layer);
                            gameObjectLayerCache[instanceID] = layer;
                        }
                        if (!excludedLayers.Contains(layer))
                        {
                            GUIStyle layerStyle = LayerStyle;
                            float layerLabelWidth = layerStyle.CalcSize(new GUIContent(layer)).x;
                            iconOffset += layerLabelWidth;
                        }
                    }
                    if (enableGameObjectTag)
                    {
                        string tag;
                        if (!gameObjectTagCache.TryGetValue(instanceID, out tag) || tag != gameObject.tag)
                        {
                            tag = gameObject.tag;
                            gameObjectTagCache[instanceID] = tag;
                        }
                        if (!excludedTags.Contains(tag))
                        {
                            GUIStyle tagStyle = TagStyle;
                            float tagLabelWidth = tagStyle.CalcSize(new GUIContent(tag)).x;
                            iconOffset += tagLabelWidth;
                        }
                    }
                    if (enableGameObjectLayer || enableGameObjectTag) { iconOffset += tagLayerOffset + tagLayerSpacing; }
                    iconOffset += cachedIcons.Count * (iconSize + iconsSpacing);
                    selectionRect.x = EditorGUIUtility.currentViewWidth - dockedPropertiesBaseOffset - iconOffset;
                    iconOffset = selectionRect.x - componentIconsOffset + 20;
                    break;
            }

            foreach ((Component component, Texture2D icon) in cachedIcons)
            {
                bool isComponentDisabled = false;
                if (enableActiveStateEffectForComponentIcons && component != null)
                {
                    try
                    {
                        dynamic dynamicComponent = component;
                        isComponentDisabled = !dynamicComponent.enabled;
                    }
                    catch { }
                }

                GUI.color = (!gameObject.activeInHierarchy || isComponentDisabled) ? inactiveColor : activeColor;
                Rect iconRect = new Rect(iconOffset, selectionRect.y + (selectionRect.height - iconSize) / 2, iconSize, iconSize);
                GUI.DrawTexture(iconRect, icon);
                if (enableTooltipOnComponentIconHovered)
                {
                    string tooltip = component != null ? component.GetType().Name : missingComponentMessage;
                    GUIContent iconContent = new GUIContent(string.Empty, tooltip);
                    DrawComponentIconTooltip(iconOffset, iconRect, iconSize, iconContent);
                }
                iconOffset += iconSize + iconsSpacing;
            }
            GUI.color = activeColor;
        }

        private static bool CheckComponentsChanged(GameObject gameObject, List<(Component component, Texture2D icon)> cachedIcons)
        {
            Component[] currentComponents = gameObject.GetComponents<Component>();
            if (currentComponents.Length != cachedIcons.Count) { return true; }

            for (int i = 0; i < currentComponents.Length; i++)
            {
                if (currentComponents[i] != cachedIcons[i].component)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<(Component component, Texture2D icon)> CacheComponentIcons(GameObject gameObject)
        {
            List<(Component component, Texture2D icon)> icons = new List<(Component component, Texture2D icon)>();
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component == null)
                {
                    Texture2D warningIcon = EditorGUIUtility.FindTexture(warningIconTexture);
                    icons.Add((null, warningIcon));
                    continue;
                }

                if (excludeTransformComponent) { if (component is Transform || component is RectTransform) continue; }
                if (excludeCanvasRendererComponent) { if (component is CanvasRenderer) continue; }

                Texture2D icon = EditorGUIUtility.ObjectContent(component, component.GetType()).image as Texture2D ?? defaultTexture;
                icons.Add((component, icon));
            }
            return icons;
        }

        private static void DrawComponentIconTooltip(float iconOffset, Rect selectionRect, float iconSize, GUIContent content)
        {
            Rect tooltipRect = new Rect(iconOffset, selectionRect.y, iconSize, iconSize);
            GUI.Box(tooltipRect, content, GUIStyle.none);
        }

        private static void UpdateGameObjectComponentIconsCacheIfNeeded()
        {
            List<int> keysToRemove = new List<int>();
            foreach (KeyValuePair<int, List<(Component component, Texture2D icon)>> entry in gameObjectComponentIconsCache)
            {
                GameObject go = EditorUtility.InstanceIDToObject(entry.Key) as GameObject;
                if (go == null)
                {
                    keysToRemove.Add(entry.Key);
                }
            }
            foreach (int key in keysToRemove)
            {
                gameObjectComponentIconsCache.Remove(key);
            }
        }

        public static void ClearComponentIconsCache()
        {
            gameObjectComponentIconsCache.Clear();
        }
        #endregion

        #region Hierarchy Tree
        private static void DrawHierarchyTree(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            if (gameObject.transform.parent == null) return;
            Transform transform = gameObject.transform;
            Texture2D branchIcon = GetOrCreateBranchIcon(transform, instanceID);
            float selectionRectX = selectionRect.x - hierarchyTreeOffset;

            GUI.color = gameObject.activeInHierarchy ? hierarchyTreeColor : inactiveColor;
            GUI.DrawTexture(new Rect(selectionRectX, selectionRect.y, selectionRect.height, selectionRect.height), branchIcon, ScaleMode.ScaleToFit);
            DrawParentTreeFillLines(transform, selectionRectX, hierarchyTreeFillLinesOffset, selectionRect.height, selectionRect.y);
            GUI.color = activeColor;
        }

        private static Texture2D GetOrCreateBranchIcon(Transform transform, int instanceID)
        {
            bool isCacheValid = hierarchyTreeCache.TryGetValue(instanceID, out Texture2D cachedIcon);
            if (!isCacheValid || HasHierarchyChanged(transform, cachedIcon))
            {
                int siblingsCount = transform.parent.childCount;
                bool isLastChild = transform.GetSiblingIndex() == siblingsCount - 1;

                if (isLastChild)
                {
                    cachedIcon = transform.childCount > 0 ? treeBranchIconL : treeBranchIconTerminalBud;
                }
                else
                {
                    cachedIcon = treeBranchIconT;
                }
                hierarchyTreeCache[instanceID] = cachedIcon;
            }
            return cachedIcon;
        }

        private static void DrawParentTreeFillLines(Transform transform, float rectX, float offsetX, float rectHeight, float rectY)
        {
            Transform parentTransform = transform.parent;
            while (parentTransform != null)
            {
                if (parentTransform.parent == null) break;
                rectX -= offsetX;
                bool isLastSibling = parentTransform.GetSiblingIndex() == parentTransform.parent.childCount - 1;
                if (!isLastSibling) { GUI.DrawTexture(new Rect(rectX, rectY, rectHeight, rectHeight), treeBranchIconI, ScaleMode.ScaleToFit); }
                parentTransform = parentTransform.parent;
            }
        }

        private static void UpdateHierarchyTreeCacheIfNeeded()
        {
            List<int> instanceIDsToRemove = new List<int>();
            foreach (int key in hierarchyTreeCache.Keys)
            {
                if (EditorUtility.InstanceIDToObject(key) as Transform == null)
                {
                    instanceIDsToRemove.Add(key);
                }
            }
            foreach (int id in instanceIDsToRemove)
            {
                hierarchyTreeCache.Remove(id);
            }
        }

        private static bool HasHierarchyChanged(Transform transform, Texture2D currentIcon)
        {
            int siblingsCount = transform.parent.childCount;
            bool isLastChild = transform.GetSiblingIndex() == siblingsCount - 1;
            bool shouldHaveTerminalBud = isLastChild && transform.childCount == 0;

            if (shouldHaveTerminalBud && currentIcon != treeBranchIconTerminalBud) return true;
            if (isLastChild && currentIcon != treeBranchIconL) return true;
            if (!isLastChild && currentIcon != treeBranchIconT) return true;
            return false;
        }
        #endregion

        #region Tag and Layer
        private static void DrawGameObjectTag(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            string tag;
            if (!gameObjectTagCache.TryGetValue(instanceID, out tag) || tag != gameObject.tag)
            {
                tag = gameObject.tag;
                gameObjectTagCache[instanceID] = tag;
            }
            if (excludedTags.Contains(tag)) return;

            float iconOffset = 0f;
            GUIStyle tagStyle = TagStyle;
            float tagLabelWidth = tagStyle.CalcSize(new GUIContent(tag)).x;

            switch (layoutMode)
            {
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Docked:
                    if (enableHierarchyButtons) { iconOffset += lockButtonWidth + activeButtonWidth; }
                    if (enableGameObjectLayer)
                    {
                        string layer;
                        if (!gameObjectLayerCache.TryGetValue(instanceID, out layer) || layer != LayerMask.LayerToName(gameObject.layer))
                        {
                            layer = LayerMask.LayerToName(gameObject.layer);
                            gameObjectLayerCache[instanceID] = layer;
                        }
                        if (!excludedLayers.Contains(layer))
                        {
                            GUIStyle layerStyle = LayerStyle;
                            float layerLabelWidth = layerStyle.CalcSize(new GUIContent(layer)).x;
                            iconOffset += layerLabelWidth + tagLayerOffset + tagLayerSpacing;
                        }
                        else iconOffset += 5f;
                    }
                    selectionRect.x = EditorGUIUtility.currentViewWidth - tagLabelWidth - dockedPropertiesBaseOffset - iconOffset;
                    iconOffset = selectionRect.x;
                    break;

                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Split:
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + tagLayerOffset + splitModeOffset;
                    break;

                default:
                    float iconSizeMultiplier = enableCustomizationForGameObjectComponentIcons ? componentIconsSize : defaultComponentIconsSize;
                    float iconsSpacing = enableCustomizationForGameObjectComponentIcons ? componentIconsSpacing : defaultComponentIconsSpacing;
                    int iconAdditionalOffset = enableCustomizationForGameObjectComponentIcons ? componentIconsOffset : defaultComponentIconsOffset;
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + iconAdditionalOffset + tagLayerOffset;
                    if (enableGameObjectComponentIcons && gameObjectComponentIconsCache.TryGetValue(instanceID, out List<(Component component, Texture2D icon)> cachedIcons))
                    {
                        if (!(disableComponentIconsForInactiveGameObjects && !gameObject.activeInHierarchy))
                        {
                            float iconSize = selectionRect.height * iconSizeMultiplier;
                            iconOffset += cachedIcons.Count * (iconSize + iconsSpacing);
                        }
                    }
                    break;
            }

            Rect tagRect = new Rect(iconOffset, selectionRect.y, tagLabelWidth, selectionRect.height);
            GUI.color = gameObject.activeInHierarchy ? activeColor : inactiveColor;
            GUI.Label(tagRect, tag, tagStyle);
            GUI.color = activeColor;
        }

        private static void DrawGameObjectLayer(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            string layer;
            if (!gameObjectLayerCache.TryGetValue(instanceID, out layer) || layer != LayerMask.LayerToName(gameObject.layer))
            {
                layer = LayerMask.LayerToName(gameObject.layer);
                gameObjectLayerCache[instanceID] = layer;
            }
            if (excludedLayers.Contains(layer)) return;

            float iconOffset;
            GUIStyle layerStyle = LayerStyle;
            float layerLabelWidth = layerStyle.CalcSize(new GUIContent(layer)).x;

            switch (layoutMode)
            {
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Docked:
                    if (enableHierarchyButtons) { selectionRect.x = EditorGUIUtility.currentViewWidth - lockButtonWidth - activeButtonWidth - layerLabelWidth - additionalLockLabelWidth; }
                    else { selectionRect.x = EditorGUIUtility.currentViewWidth - layerLabelWidth - 5; }
                    iconOffset = selectionRect.x - tagLayerOffset - dockedPropertiesBaseOffset;
                    break;

                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Split:
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + tagLayerOffset + splitModeOffset;
                    if (enableGameObjectTag)
                    {
                        string tag;
                        if (gameObjectTagCache.TryGetValue(instanceID, out tag))
                        {
                            if (!excludedTags.Contains(tag))
                            {
                                GUIStyle tagStyle = TagStyle;
                                float tagLabelWidth = tagStyle.CalcSize(new GUIContent(tag)).x + 2;
                                iconOffset += tagLabelWidth + tagLayerSpacing;
                            }
                        }
                    }
                    break;

                default:
                    float iconSizeMultiplier = enableCustomizationForGameObjectComponentIcons ? componentIconsSize : defaultComponentIconsSize;
                    float iconsSpacing = enableCustomizationForGameObjectComponentIcons ? componentIconsSpacing : defaultComponentIconsSpacing;
                    int iconAdditionalOffset = enableCustomizationForGameObjectComponentIcons ? componentIconsOffset : defaultComponentIconsOffset;
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + iconAdditionalOffset + tagLayerOffset;
                    if (enableGameObjectComponentIcons && gameObjectComponentIconsCache.TryGetValue(instanceID, out List<(Component component, Texture2D icon)> cachedIcons))
                    {
                        if (!(disableComponentIconsForInactiveGameObjects && !gameObject.activeInHierarchy))
                        {
                            float iconSize = selectionRect.height * iconSizeMultiplier;
                            iconOffset += cachedIcons.Count * (iconSize + iconsSpacing);
                        }
                    }
                    if (enableGameObjectTag)
                    {
                        string tag;
                        if (gameObjectTagCache.TryGetValue(instanceID, out tag))
                        {
                            if (!excludedTags.Contains(tag))
                            {
                                GUIStyle tagStyle = TagStyle;
                                float tagLabelWidth = tagStyle.CalcSize(new GUIContent(tag)).x + 2;
                                iconOffset += tagLabelWidth + tagLayerSpacing;
                            }
                        }
                    }
                    break;
            }

            Rect layerRect = new Rect(iconOffset, selectionRect.y, layerLabelWidth, selectionRect.height);
            GUI.color = gameObject.activeInHierarchy ? activeColor : inactiveColor;
            GUI.Label(layerRect, layer, layerStyle);
            GUI.color = activeColor;
        }

        private static Rect CalculateTagRect(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            string tag;
            if (!gameObjectTagCache.TryGetValue(instanceID, out tag) || tag != gameObject.tag)
            {
                tag = gameObject.tag;
                gameObjectTagCache[instanceID] = tag;
            }
            if (excludedTags.Contains(tag)) return Rect.zero;

            float iconOffset = 0f;
            GUIStyle tagStyle = TagStyle;
            float tagLabelWidth = tagStyle.CalcSize(new GUIContent(tag)).x;

            switch (layoutMode)
            {
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Docked:
                    if (enableHierarchyButtons) { iconOffset += lockButtonWidth + activeButtonWidth; }
                    if (enableGameObjectLayer)
                    {
                        string layer;
                        if (!gameObjectLayerCache.TryGetValue(instanceID, out layer) || layer != LayerMask.LayerToName(gameObject.layer))
                        {
                            layer = LayerMask.LayerToName(gameObject.layer);
                            gameObjectLayerCache[instanceID] = layer;
                        }
                        if (!excludedLayers.Contains(layer))
                        {
                            GUIStyle layerStyle = LayerStyle;
                            float layerLabelWidth = layerStyle.CalcSize(new GUIContent(layer)).x;
                            iconOffset += layerLabelWidth + tagLayerOffset + tagLayerSpacing;
                        }
                        else iconOffset += 5f;
                    }
                    selectionRect.x = EditorGUIUtility.currentViewWidth - tagLabelWidth - dockedPropertiesBaseOffset - iconOffset;
                    iconOffset = selectionRect.x;
                    break;

                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Split:
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + tagLayerOffset + splitModeOffset;
                    break;

                default:
                    float iconSizeMultiplier = enableCustomizationForGameObjectComponentIcons ? componentIconsSize : defaultComponentIconsSize;
                    float iconsSpacing = enableCustomizationForGameObjectComponentIcons ? componentIconsSpacing : defaultComponentIconsSpacing;
                    int iconAdditionalOffset = enableCustomizationForGameObjectComponentIcons ? componentIconsOffset : defaultComponentIconsOffset;
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + iconAdditionalOffset + tagLayerOffset;
                    if (enableGameObjectComponentIcons && gameObjectComponentIconsCache.TryGetValue(instanceID, out List<(Component component, Texture2D icon)> cachedIcons))
                    {
                        if (!(disableComponentIconsForInactiveGameObjects && !gameObject.activeInHierarchy))
                        {
                            float iconSize = selectionRect.height * iconSizeMultiplier;
                            iconOffset += cachedIcons.Count * (iconSize + iconsSpacing);
                        }
                    }
                    break;
            }
            
            return new Rect(iconOffset, selectionRect.y, tagLabelWidth, selectionRect.height);
        }

        private static Rect CalculateLayerRect(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            string layer;
            if (!gameObjectLayerCache.TryGetValue(instanceID, out layer) || layer != LayerMask.LayerToName(gameObject.layer))
            {
                layer = LayerMask.LayerToName(gameObject.layer);
                gameObjectLayerCache[instanceID] = layer;
            }
            if (excludedLayers.Contains(layer)) return Rect.zero;

            float iconOffset;
            GUIStyle layerStyle = LayerStyle;
            float layerLabelWidth = layerStyle.CalcSize(new GUIContent(layer)).x;

            switch (layoutMode)
            {
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Docked:
                    if (enableHierarchyButtons) { selectionRect.x = EditorGUIUtility.currentViewWidth - lockButtonWidth - activeButtonWidth - layerLabelWidth - additionalLockLabelWidth; }
                    else { selectionRect.x = EditorGUIUtility.currentViewWidth - layerLabelWidth - 5; }
                    iconOffset = selectionRect.x - tagLayerOffset - dockedPropertiesBaseOffset;
                    break;
                
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Split:
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + tagLayerOffset + splitModeOffset;
                    if (enableGameObjectTag)
                    {
                        string tag;
                        if (gameObjectTagCache.TryGetValue(instanceID, out tag))
                        {
                            if (!excludedTags.Contains(tag))
                            {
                                GUIStyle tagStyle = TagStyle;
                                float tagLabelWidth = tagStyle.CalcSize(new GUIContent(tag)).x + 2;
                                iconOffset += tagLabelWidth + tagLayerSpacing;
                            }
                        }
                    }
                    break;

                default:
                    float iconSizeMultiplier = enableCustomizationForGameObjectComponentIcons ? componentIconsSize : defaultComponentIconsSize;
                    float iconsSpacing = enableCustomizationForGameObjectComponentIcons ? componentIconsSpacing : defaultComponentIconsSpacing;
                    int iconAdditionalOffset = enableCustomizationForGameObjectComponentIcons ? componentIconsOffset : defaultComponentIconsOffset;
                    iconOffset = selectionRect.x + GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x + iconAdditionalOffset + tagLayerOffset;
                    if (enableGameObjectComponentIcons && gameObjectComponentIconsCache.TryGetValue(instanceID, out List<(Component component, Texture2D icon)> cachedIcons))
                    {
                        if (!(disableComponentIconsForInactiveGameObjects && !gameObject.activeInHierarchy))
                        {
                            float iconSize = selectionRect.height * iconSizeMultiplier;
                            iconOffset += cachedIcons.Count * (iconSize + iconsSpacing);
                        }
                    }
                    if (enableGameObjectTag)
                    {
                        string tag;
                        if (gameObjectTagCache.TryGetValue(instanceID, out tag))
                        {
                            if (!excludedTags.Contains(tag))
                            {
                                GUIStyle tagStyle = TagStyle;
                                float tagLabelWidth = tagStyle.CalcSize(new GUIContent(tag)).x + 2;
                                iconOffset += tagLabelWidth + tagLayerSpacing;
                            }
                        }
                    }
                    break;
            }

            return new Rect(iconOffset, selectionRect.y, layerLabelWidth, selectionRect.height);
        }

        private static void UpdateTagCacheIfNeeded()
        {
            List<int> instanceIDsToRemove = new List<int>();
            foreach (int key in gameObjectTagCache.Keys)
            {
                if (EditorUtility.InstanceIDToObject(key) as GameObject == null)
                {
                    instanceIDsToRemove.Add(key);
                }
            }
            foreach (int id in instanceIDsToRemove)
            {
                gameObjectTagCache.Remove(id);
            }
        }

        private static void UpdateLayerCacheIfNeeded()
        {
            List<int> instanceIDsToRemove = new List<int>();
            foreach (int key in gameObjectLayerCache.Keys)
            {
                if (EditorUtility.InstanceIDToObject(key) as GameObject == null)
                {
                    instanceIDsToRemove.Add(key);
                }
            }
            foreach (int id in instanceIDsToRemove)
            {
                gameObjectLayerCache.Remove(id);
            }
        }
        #endregion

        #region Hierarchy Rows
        private static void DrawHierarchyRows(Rect selectionRect)
        {
            selectionRect.x = hierarchyWindowOffsetLeft;
            selectionRect.width = EditorGUIUtility.currentViewWidth;
            if ((int)(selectionRect.y / selectionRect.height) % 2 == 0) return;
            EditorGUI.DrawRect(selectionRect, HierarchyDesigner_Shared_ColorUtility.GetRowColor());
        }
        #endregion

        #region Hierarchy Buttons
        private static void DrawHierarchyButtons(GameObject gameObject, Rect selectionRect)
        {
            float iconStartX = CalculateButtonsRect(gameObject, selectionRect);
            Rect lockIconRect = new Rect(iconStartX, selectionRect.y, 20, selectionRect.height);
            Rect activeToggleRect = new Rect(iconStartX + 20, selectionRect.y, 30, selectionRect.height);
            DrawLockButton(gameObject, lockIconRect);
            DrawActiveToggleButton(gameObject, activeToggleRect);
        }

        private static float CalculateButtonsRect(GameObject gameObject, Rect selectionRect)
        {
            switch (layoutMode)
            {
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Consecutive:
                    float offsetX = selectionRect.x + 8f;
                    GUIContent gameObjectNameContent = new GUIContent(gameObject.name);
                    offsetX += GUI.skin.label.CalcSize(gameObjectNameContent).x;

                    if ((gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable)
                    {
                        GUIStyle lockStyle = LockStyle;
                        float lockLabelWidth = lockStyle.CalcSize(new GUIContent(lockedLabel)).x;
                        return offsetX + lockIconOffset + lockLabelWidth + additionalLockLabelWidth;
                    }
                    if ((folderCache.ContainsKey(gameObject.GetInstanceID()) || gameObject.GetComponent<HierarchyDesignerFolder>()) && excludeFolderProperties) return offsetX += 15f;

                    if (enableGameObjectComponentIcons)
                    {
                        float iconSizeMultiplier = defaultComponentIconsSize;
                        float iconsSpacing = defaultComponentIconsSpacing;
                        int iconAdditionalOffset = defaultComponentIconsOffset;
                        if (enableCustomizationForGameObjectComponentIcons)
                        {
                            iconSizeMultiplier = componentIconsSize;
                            iconsSpacing = componentIconsSpacing;
                            iconAdditionalOffset = componentIconsOffset;
                        }
                        if (!(disableComponentIconsForInactiveGameObjects && !gameObject.activeInHierarchy))
                        {
                            if (gameObjectComponentIconsCache.TryGetValue(gameObject.GetInstanceID(), out List<(Component, Texture2D)> components))
                            {
                                foreach (var component in components)
                                {
                                    offsetX += selectionRect.height * iconSizeMultiplier + iconsSpacing;
                                }
                                offsetX += iconAdditionalOffset;
                            }
                        }
                        else offsetX += 20f;
                    } else offsetX += 18f;
                    if (enableGameObjectTag && gameObjectTagCache.TryGetValue(gameObject.GetInstanceID(), out string tag))
                    {
                        if (!excludedTags.Contains(tag))
                        {
                            offsetX += TagStyle.CalcSize(new GUIContent(tag)).x + tagLayerOffset;
                        }
                    }
                    if (enableGameObjectLayer && gameObjectLayerCache.TryGetValue(gameObject.GetInstanceID(), out string layer))
                    {
                        if (!excludedLayers.Contains(layer))
                        {
                            offsetX += LayerStyle.CalcSize(new GUIContent(layer)).x + tagLayerSpacing;
                        }
                    }
                    return offsetX;

                default:
                    selectionRect.x = hierarchyWindowOffsetRight - dockedPropertiesBaseOffset;
                    selectionRect.width = EditorGUIUtility.currentViewWidth;
                    return selectionRect.x + selectionRect.width;
            }
        }

        private static void DrawLockButton(GameObject gameObject, Rect rect)
        {
            GUI.color = gameObject.activeInHierarchy ? activeColor : inactiveColor;
            if (GUI.Button(rect, lockIcon))
            {
                bool isLocked = (gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable;
                ToggleLockState(gameObject, !isLocked);
            }
            GUI.color = activeColor;
        }

        private static void DrawActiveToggleButton(GameObject gameObject, Rect rect)
        {
            bool isActive = gameObject.activeSelf;
            GUI.color = gameObject.activeInHierarchy ? activeColor : inactiveColor;
            if (GUI.Button(rect, new GUIContent(isActive ? "On" : "Off")))
            {
                ToggleActiveState(gameObject, !isActive);
            }
            GUI.color = activeColor;
        }

        private static void ToggleLockState(GameObject gameObject, bool newState)
        {
            HierarchyDesigner_Utility_Tools.LockGameObject(gameObject, newState);
        }

        private static void ToggleActiveState(GameObject gameObject, bool newState)
        {
            Undo.RecordObject(gameObject, "Toggle Active State");
            gameObject.SetActive(newState);
        }

        private static void ProcessHierarchyButtons(GameObject gameObject, Rect selectionRect)
        {
            Rect lockIconRect, activeToggleRect;
            GetButtonRects(gameObject, selectionRect, out lockIconRect, out activeToggleRect);

            bool isMouseDown = Event.current.type == EventType.MouseDown;
            if (isMouseDown && lockIconRect.Contains(Event.current.mousePosition))
            {
                ToggleLockState(gameObject, !HierarchyDesigner_Utility_Tools.IsGameObjectLocked(gameObject));
                Event.current.Use();
            }
            else if (isMouseDown && activeToggleRect.Contains(Event.current.mousePosition))
            {
                ToggleActiveState(gameObject, !gameObject.activeSelf);
                Event.current.Use();
            }
        }

        private static void GetButtonRects(GameObject gameObject, Rect selectionRect, out Rect lockIconRect, out Rect activeToggleRect)
        {
            float iconStartX = CalculateButtonsRect(gameObject, selectionRect);
            lockIconRect = new Rect(iconStartX, selectionRect.y, lockButtonWidth, selectionRect.height);
            activeToggleRect = new Rect(iconStartX + lockButtonWidth, selectionRect.y, activeButtonWidth, selectionRect.height);
        }
        #endregion

        #region Major Shortcuts
        private static bool IsShortcutPressed(KeyCode shortcutKey)
        {
            Event currentEvent = Event.current;
            if (shortcutKey >= KeyCode.Alpha0 && shortcutKey <= KeyCode.Menu) { return currentEvent.type == EventType.KeyDown && currentEvent.keyCode == shortcutKey; }
            int mouseButton = GetMouseButtonFromKeyCode(shortcutKey);
            if (mouseButton != -1) { return currentEvent.type == EventType.MouseDown && currentEvent.button == mouseButton; }
            return false;
        }

        private static int GetMouseButtonFromKeyCode(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Mouse0: return 0;
                case KeyCode.Mouse1: return 1;
                case KeyCode.Mouse2: return 2;
                case KeyCode.Mouse3: return 3;
                case KeyCode.Mouse4: return 4;
                case KeyCode.Mouse5: return 5;
                case KeyCode.Mouse6: return 6;
                default: return -1;
            }
        }

        private static void ProcessToggleGameObjectActiveStateMajorShortcut(GameObject[] gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Undo.RecordObject(gameObject, "Toggle Active State");
                gameObject.SetActive(!gameObject.activeSelf);
            }
            Event.current.Use();
        }

        private static void ProcessToggleGameObjectLockStateMajorShortcut(GameObject[] gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                bool isLocked = (gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable;
                HierarchyDesigner_Utility_Tools.SetLockState(gameObject, isLocked);
            }
            Event.current.Use();
        }

        private static void ProcessTagLayerMajorShortcut(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            if (!enableGameObjectTag && !enableGameObjectLayer) return;

            Vector2 mousePosition = Event.current.mousePosition;
            if (enableGameObjectTag) 
            {
                Rect tagRect = CalculateTagRect(gameObject, selectionRect, instanceID);
                if (tagRect.Contains(mousePosition))
                {
                    HierarchyDesigner_Window_TagLayer.OpenWindow(gameObject, true, Event.current.mousePosition);
                    Event.current.Use();
                }
            }
            if (enableGameObjectLayer)
            {
                Rect layerRect = CalculateLayerRect(gameObject, selectionRect, instanceID);
                if (layerRect.Contains(mousePosition))
                {
                    HierarchyDesigner_Window_TagLayer.OpenWindow(gameObject, false, Event.current.mousePosition);
                    Event.current.Use();
                }
            }
        }

        private static void ProcessRenameMajorShortcut()
        {
            List<GameObject> selectedGameObjects = new List<GameObject>(Selection.gameObjects);
            if (selectedGameObjects.Count < 1) { return; }
            HierarchyDesigner_Window_RenameTool.OpenWindow(selectedGameObjects, true, 0);
        }
        #endregion

        #region Lock State
        private static void DrawGameObjectLock(GameObject gameObject, Rect selectionRect)
        {
            GUIStyle lockStyle = LockStyle;
            float lockLabelWidth = lockStyle.CalcSize(new GUIContent(lockedLabel)).x;
            float offset = 0;

            switch (layoutMode)
            {
                case HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode.Docked:
                    if (enableHierarchyButtons) { selectionRect.x = EditorGUIUtility.currentViewWidth - lockButtonWidth - activeButtonWidth - lockLabelWidth - additionalLockLabelWidth - dockedPropertiesBaseOffset; }
                    else { selectionRect.x = EditorGUIUtility.currentViewWidth - lockLabelWidth - dockedPropertiesBaseOffset; }
                    break;

                default:
                    GUIContent nameContent = new GUIContent(gameObject.name);
                    float nameWidth = GUI.skin.label.CalcSize(nameContent).x;
                    offset += nameWidth + lockIconOffset;
                    break;
            }

            Rect lockRect = new Rect(selectionRect.x + offset, selectionRect.y, lockLabelWidth + additionalLockLabelWidth, selectionRect.height);
            GUI.color = gameObject.activeInHierarchy ? activeColor : inactiveColor;
            GUI.Label(lockRect, lockedLabel, lockStyle);
            GUI.color = activeColor;
        }
        #endregion

        #region Folder
        private static void DrawFolder(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            if (!folderCache.TryGetValue(instanceID, out (Color folderColor, HierarchyDesigner_Configurable_Folder.FolderImageType folderImageType) folderInfo))
            {
                HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folderData = HierarchyDesigner_Configurable_Folder.GetFolderData(gameObject.name) ?? new HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData();
                folderInfo = (folderData.Color, folderData.ImageType);
                folderCache[instanceID] = folderInfo;
            }

            DrawBackground(selectionRect, instanceID);
            Texture2D folderIcon = HierarchyDesigner_Shared_Resources.FolderImageType(folderInfo.folderImageType);
            GUI.color = gameObject.activeInHierarchy ? folderInfo.folderColor : ConvertActiveColorToInactive(folderInfo.folderColor, 0.5f);
            GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height), folderIcon);
            GUI.color = activeColor;
        }

        private static Color ConvertActiveColorToInactive(Color color, float alphaFactor)
        {
            alphaFactor = Mathf.Clamp01(alphaFactor);
            return new Color(color.r, color.g, color.b, color.a * alphaFactor);
        }

        public static void ClearFolderCache()
        {
            folderCache.Clear();
            EditorApplication.RepaintHierarchyWindow();
        }
        #endregion

        #region Separator
        private static void DrawSeparator(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            string separatorKey = gameObject.name.Replace("//", "").Trim();
            if (!separatorCache.TryGetValue(instanceID, out var separatorInfo))
            {
                HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData = HierarchyDesigner_Configurable_Separator.GetSeparatorData(separatorKey) ?? new HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData();
                separatorInfo = (separatorData.TextColor, separatorData.IsGradientBackground, separatorData.BackgroundColor, separatorData.BackgroundGradient, separatorData.FontSize, separatorData.FontStyle, separatorData.TextAnchor, separatorData.ImageType);
                separatorCache[instanceID] = separatorInfo;
            }

            GUI.color = SetBackgroundColorBasedOnState(selectionRect, instanceID);
            GUI.DrawTexture(new Rect(32, selectionRect.y, EditorGUIUtility.currentViewWidth, selectionRect.height), EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;

            selectionRect.x = 32;
            selectionRect.width = EditorGUIUtility.currentViewWidth;

            GUIStyle textStyle = new GUIStyle
            {
                alignment = separatorInfo.textAnchor,
                fontSize = separatorInfo.fontSize,
                fontStyle = separatorInfo.fontStyle,
                normal = { textColor = separatorInfo.textColor }
            };

            Texture2D backgroundTexture = HierarchyDesigner_Shared_Resources.SeparatorImageType(separatorInfo.separatorImageType);

            if (separatorInfo.isGradientBackground)
            {
                if (!gradientTextureCache.TryGetValue(instanceID, out Texture2D gradientTexture))
                {
                    gradientTexture = CreateGradientTexture(separatorInfo.backgroundGradient, Mathf.FloorToInt(selectionRect.width));
                    gradientTextureCache[instanceID] = gradientTexture;
                }
                if (includeBackgroundImageForGradientBackgroundCache) { GUI.DrawTexture(selectionRect, backgroundTexture); }
                GUI.DrawTexture(selectionRect, gradientTexture);
            }
            else
            {
                GUI.color = separatorInfo.backgroundColor;
                GUI.DrawTexture(selectionRect, backgroundTexture);
            }
            GUI.color = Color.white;

            Rect textRect = AdjustRect(selectionRect, separatorInfo.textAnchor);
            GUI.Label(textRect, separatorKey, textStyle);
        }

        private static Texture2D CreateGradientTexture(Gradient gradient, int width)
        {
            Texture2D gradientTexture = new Texture2D(width, 1, TextureFormat.ARGB32, false);
            for (int x = 0; x < width; x++)
            {
                float t = (float)x / (width - 1);
                Color color = gradient.Evaluate(t);
                gradientTexture.SetPixel(x, 0, color);
            }
            gradientTexture.Apply();
            return gradientTexture;
        }

        private static Rect AdjustRect(Rect rect, TextAnchor textAlignment)
        {
            switch (textAlignment)
            {
                case TextAnchor.MiddleLeft:
                case TextAnchor.UpperLeft:
                case TextAnchor.LowerLeft:
                    rect.x += 3;
                    break;
                case TextAnchor.MiddleRight:
                case TextAnchor.UpperRight:
                case TextAnchor.LowerRight:
                    rect.x -= 36;
                    break;
            }
            return rect;
        }

        public static void ClearSeparatorCache()
        {
            separatorCache.Clear();
            ClearGradientTextureCache();
            EditorApplication.RepaintHierarchyWindow();
        }

        private static void ClearGradientTextureCache()
        {
            foreach (Texture2D texture in gradientTextureCache.Values)
            {
                GameObject.DestroyImmediate(texture);
            }
            gradientTextureCache.Clear();
        }
        #endregion
        #endregion

        #region Cache Setters
        public static HierarchyDesigner_Configurable_GeneralSettings.HierarchyLayoutMode LayoutModeCache
        {
            set
            {
                layoutMode = value;
            }
        }

        public static bool EnableGameObjectMainIconCache
        {
            set
            {
                enableGameObjectMainIcon = value;
            }
        }

        public static bool EnableGameObjectComponentIconsCache
        {
            set
            {
                enableGameObjectComponentIcons = value;
            }
        }

        public static bool EnableHierarchyTreeCache
        {
            set
            {
                enableHierarchyTree = value;
            }
        }

        public static bool EnableGameObjectTagCache
        {
            set
            {
                enableGameObjectTag = value;
            }
        }

        public static bool EnableGameObjectLayerCache
        {
            set
            {
                enableGameObjectLayer = value;
            }
        }

        public static bool EnableHierarchyRowsCache
        {
            set
            {
                enableHierarchyRows = value;
            }
        }

        public static bool EnableHierarchyButtonsCache
        {
            set
            {
                enableHierarchyButtons = value;
            }
        }

        public static bool EnableMajorShortcutsCache
        {
            set
            {
                enableMajorShortcuts = value;
            }
        }

        public static bool DisableHierarchyDesignerDuringPlayModeCache
        {
            set
            {
                disableEditorDesignerMajorOperationsDuringPlayMode = value;
            }
        }

        public static bool ExcludeFolderProperties
        {
            set
            {
                excludeFolderProperties = value;
            }
        }

        public static bool ExcludeTransformComponentCache
        {
            set
            {
                excludeTransformComponent = value;
            }
        }

        public static bool ExcludeCanvasRendererComponentCache
        {
            set
            {
                excludeCanvasRendererComponent = value;
            }
        }

        public static bool EnableDynamicChangesCheckForGameObjectMainIconCache
        {
            set
            {
                enableDynamicChangesCheckForGameObjectMainIcon = value;
            }
        }

        public static bool EnableDynamicBackgroundForGameObjectMainIconCache
        {
            set
            {
                enableDynamicBackgroundForGameObjectMainIcon = value;
            }
        }

        public static bool EnablePreciseRectForDynamicBackgroundForGameObjectMainIconCache
        {
            set
            {
                enablePreciseRectForDynamicBackgroundForGameObjectMainIcon = value;
            }
        }

        public static bool EnableCustomizationForGameObjectComponentIconsCache
        {
            set
            {
                enableCustomizationForGameObjectComponentIcons = value;
            }
        }

        public static bool EnableTooltipOnComponentIconHoveredCache
        {
            set
            {
                enableTooltipOnComponentIconHovered = value;
            }
        }

        public static bool EnableActiveStateEffectForComponentIconsCache
        {
            set
            {
                enableActiveStateEffectForComponentIcons = value;
            }
        }

        public static bool DisableComponentIconsForInactiveGameObjectsCache
        {
            set
            {
                disableComponentIconsForInactiveGameObjects = value;
            }
        }

        public static bool IncludeBackgroundImageForGradientBackgroundCache
        {
            set
            {
                includeBackgroundImageForGradientBackgroundCache = value;
            }
        }

        public static float ComponentIconsSizeCache
        {
            set
            {
                componentIconsSize = value;
            }
        }

        public static float ComponentIconsSpacingCache
        {
            set
            {
                componentIconsSpacing = value;
            }
        }

        public static int ComponentIconsOffsetCache
        {
            set
            {
                componentIconsOffset = value;
            }
        }

        public static Color HierarchyTreeColorCache
        {
            set
            {
                hierarchyTreeColor = value;
            }
        }

        public static Color TagColorCache
        {
            set
            {
                tagColor = value;
            }
        }

        public static TextAnchor TagTextAnchorCache
        {
            set
            {
                tagTextAnchor = value;
            }
        }

        public static FontStyle TagFontStyleCache
        {
            set
            {
                tagFontStyle = value;
            }
        }

        public static int TagFontSizeCache
        {
            set
            {
                tagFontSize = value;
            }
        }

        public static Color LayerColorCache
        {
            set
            {
                layerColor = value;
            }
        }

        public static TextAnchor LayerTextAnchorCache
        {
            set
            {
                layerTextAnchor = value;
            }
        }

        public static FontStyle LayerFontStyleCache
        {
            set
            {
                layerFontStyle = value;
            }
        }

        public static int LayerFontSizeCache
        {
            set
            {
                layerFontSize = value;
            }
        }

        public static int TagLayerOffsetCache
        {
            set
            {
                tagLayerOffset = value;
            }
        }

        public static int TagLayerSpacingCache
        {
            set
            {
                tagLayerSpacing = value;
            }
        }

        public static List<string> ExcludedTagsCache
        {
            set
            {
                excludedTags = value;
            }
        }

        public static List<string> ExcludedLayersCache
        {
            set
            {
                excludedLayers = value;
            }
        }

        public static Color LockColorCache
        {
            set
            {
                lockColor = value;
            }
        }

        public static TextAnchor LockTextAnchorCache
        {
            set
            {
                lockTextAnchor = value;
            }
        }

        public static FontStyle LockFontStyleCache
        {
            set
            {
                lockFontStyle = value;
            }
        }

        public static int LockFontSizeCache
        {
            set
            {
                lockFontSize = value;
            }
        }

        public static KeyCode ToggleGameObjectActiveStateKeyCodeCache
        {
            set
            {
                toggleGameObjectActiveStateKeyCode = value;
            }
        }

        public static KeyCode ToggleLockStateKeyCodeCache
        {
            set
            {
                toggleLockStateKeyCode = value;
            }
        }

        public static KeyCode ChangeTagLayerKeyCodeCache
        {
            set
            {
                changeTagLayerKeyCode = value;
            }
        }

        public static KeyCode RenameSelectedGameObjectsKeyCodeCache
        {
            set
            {
                renameSelectedGameObjectsKeyCode = value;
            }
        }

        public static Dictionary<int, (Color folderColor, HierarchyDesigner_Configurable_Folder.FolderImageType folderImageType)> FolderCache
        {
            set
            {
                folderCache = value;
            }
        }

        public static Dictionary<int, (Color textColor, bool isGradientBackground, Color backgroundColor, Gradient backgroundGradient, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, HierarchyDesigner_Configurable_Separator.SeparatorImageType separatorImageType)> SeparatorCache
        {
            set
            {
                separatorCache = value;
            }
        }
        #endregion
    }
}
#endif