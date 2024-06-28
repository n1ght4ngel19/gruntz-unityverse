#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_ToolsMaster : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 outerScroll;
        private GUIStyle headerGUIStyle;
        private GUIStyle contentGUIStyle;
        private GUIStyle outerBackgroundGUIStyle;
        private GUIStyle innerBackgroundGUIStyle;
        private GUIStyle contentBackgroundGUIStyle;
        #endregion
        #region Const
        private const float labelWidth = 85;
        #endregion
        #region Tools Values
        private HierarchyDesigner_Attribute_Tools selectedCategory = HierarchyDesigner_Attribute_Tools.Activate;
        private int selectedActionIndex = 0;
        private List<string> availableActionNames = new List<string>();
        private List<MethodInfo> availableActionMethods = new List<MethodInfo>();
        private static Dictionary<HierarchyDesigner_Attribute_Tools, List<(string Name, MethodInfo Method)>> cachedActions = new Dictionary<HierarchyDesigner_Attribute_Tools, List<(string Name, MethodInfo Method)>>();
        private static bool cacheInitialized = false;
        #endregion
        #endregion

        #region Window
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Helpers + "/Tools Master", false, HierarchyDesigner_Shared_MenuItems.LayerSeven + 1)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_ToolsMaster window = GetWindow<HierarchyDesigner_Window_ToolsMaster>("Tools Master", true);
            window.minSize = new Vector2(300, 150);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            if (!cacheInitialized)
            {
                InitializeActionCache();
                cacheInitialized = true;
            }
            UpdateAvailableActions();
        }

        private static void InitializeActionCache()
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
                                    if (!cachedActions.ContainsKey(toolAttribute.Category))
                                    {
                                        cachedActions[toolAttribute.Category] = new List<(string Name, MethodInfo Method)>();
                                    }
                                    cachedActions[toolAttribute.Category].Add((actionName, method));
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string ExtractActionsFromCategories(string menuItemPath, HierarchyDesigner_Attribute_Tools category)
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
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out GUIStyle _, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Tools Master", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region Category
            HierarchyDesigner_Attribute_Tools previousCategory = selectedCategory;
            EditorGUILayout.BeginHorizontal(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Category:", contentGUIStyle, GUILayout.Width(labelWidth));
            selectedCategory = (HierarchyDesigner_Attribute_Tools)EditorGUILayout.EnumPopup(selectedCategory, GUILayout.ExpandWidth(true));
            if (previousCategory != selectedCategory) { UpdateAvailableActions(); }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4);
            #endregion

            #region Actions
            if (availableActionNames.Count == 0) { GUILayout.Label("No tools available for this category."); }
            else
            {
                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("Action:", contentGUIStyle, GUILayout.ExpandWidth(true));
                GUILayout.Space(4);
                selectedActionIndex = EditorGUILayout.Popup(selectedActionIndex, availableActionNames.ToArray());
                EditorGUILayout.EndVertical();
            }

            #endregion
            #endregion
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            #region Footer
            if (GUILayout.Button("Apply Action", GUILayout.Height(30)))
            {
                ApplyAction();
            }
            EditorGUILayout.EndVertical();
            #endregion
        }

        private void ApplyAction()
        {
            if (availableActionMethods.Count > selectedActionIndex && selectedActionIndex >= 0)
            {
                MethodInfo methodToInvoke = availableActionMethods[selectedActionIndex];
                methodToInvoke?.Invoke(null, null);
            }
        }
        #endregion

        #region Operations
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
    }
}
#endif