#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Window_RenameTool : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 mainScroll;
        private GUIStyle rightPanel;
        private GUIStyle categoryLabel;
        private GUIStyle contentPanel;
        private GUIStyle contentLabel;
        #endregion
        #region Const
        private const float minimumSpaceMargin = 2f;
        private const float fieldsWidth = 95;
        #endregion
        #region Rename Tool Values
        private static string newName = "";
        private static bool automaticIndexing = true;
        private static int startingIndex = 0;
        [SerializeField] private List<GameObject> selectedGameObjects = new();
        private ReorderableList reorderableList;
        #endregion
        #endregion

        #region Window
        public static void OpenWindow(List<GameObject> gameObjects, bool autoIndex = true, int startIndex = 0)
        {
            #region Window Initial Values
            HierarchyDesigner_Window_RenameTool window = GetWindow<HierarchyDesigner_Window_RenameTool>("Rename Tool");
            Vector2 size = new Vector2(400, 200);
            window.minSize = size;
            #endregion

            #region Rename Tool Initial Values
            newName = "";
            automaticIndexing = autoIndex;
            startingIndex = startIndex;
            window.selectedGameObjects = gameObjects ?? new List<GameObject>();
            window.InitializeReorderableList();
            #endregion
        }
        #endregion

        #region Initialization
        private void InitializeReorderableList()
        {
            reorderableList = new ReorderableList(selectedGameObjects, typeof(GameObject), true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Selected GameObjects");
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    selectedGameObjects[index] = (GameObject)EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), selectedGameObjects[index], typeof(GameObject), true);
                },
                onAddCallback = (ReorderableList list) =>
                {
                    selectedGameObjects.Add(null);
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    selectedGameObjects.RemoveAt(list.index);
                }
            };
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.GetHierarchyDesignerGUIStyles(out GUIStyle _, out GUIStyle _, out GUIStyle _, out GUIStyle _, out rightPanel, out categoryLabel, out GUIStyle _, out GUIStyle _, out contentPanel, out contentLabel, out GUIStyle _, out GUIStyle _, out GUIStyle _);

            #region Header
            EditorGUILayout.BeginVertical(rightPanel);
            EditorGUILayout.LabelField("Rename Tool", categoryLabel);
            GUILayout.Space(minimumSpaceMargin);
            #endregion

            mainScroll = EditorGUILayout.BeginScrollView(mainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            #region Body
            #region New Values
            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.LabelField("Parameters", contentLabel, GUILayout.ExpandWidth(true));
            GUILayout.Space(minimumSpaceMargin);

            newName = HierarchyDesigner_Shared_GUI.DrawTextField("New Name", fieldsWidth, newName);
            automaticIndexing = HierarchyDesigner_Shared_GUI.DrawToggle("Auto-Index", fieldsWidth, automaticIndexing);
            if (automaticIndexing) { startingIndex = HierarchyDesigner_Shared_GUI.DrawDelayedIntField("Starting Index", fieldsWidth, startingIndex); }

            EditorGUILayout.EndVertical();
            GUILayout.Space(minimumSpaceMargin);
            #endregion

            #region Selected GameObjects List
            EditorGUILayout.BeginVertical(contentPanel);
            if (reorderableList != null) { reorderableList.DoLayoutList(); }
            EditorGUILayout.EndVertical();
            #endregion
            #endregion
            EditorGUILayout.EndScrollView();

            #region Footer
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reassign Selected GameObjects", GUILayout.Height(25)))
            {
                ReassignSelectedGameObjects();
            }
            if (GUILayout.Button("Clear Selected GameObjects", GUILayout.Height(25)))
            {
                ClearSelectedGameObjects();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Rename Selected GameObjects", GUILayout.Height(30)))
            {
                RenameSelectedGameObjects();
                Close();
            }
            #endregion

            EditorGUILayout.EndVertical();
        }

        private void OnDestroy()
        {
            newName = null;
            selectedGameObjects = null;
            reorderableList = null;
        }
        #endregion

        #region Operations
        private void ReassignSelectedGameObjects()
        {
            selectedGameObjects = new List<GameObject>(Selection.gameObjects);
            InitializeReorderableList();
        }

        private void ClearSelectedGameObjects()
        {
            selectedGameObjects.Clear();
            InitializeReorderableList();
        }

        private void RenameSelectedGameObjects()
        {
            if (selectedGameObjects == null) return;

            for (int i = 0; i < selectedGameObjects.Count; i++)
            {
                if (selectedGameObjects[i] != null)
                {
                    Undo.RecordObject(selectedGameObjects[i], "Rename GameObject");
                    string objectName = automaticIndexing ? $"{newName} ({startingIndex + i})" : newName;
                    selectedGameObjects[i].name = objectName;
                    EditorUtility.SetDirty(selectedGameObjects[i]);
                }
            }
        }
        #endregion
    }
}
#endif