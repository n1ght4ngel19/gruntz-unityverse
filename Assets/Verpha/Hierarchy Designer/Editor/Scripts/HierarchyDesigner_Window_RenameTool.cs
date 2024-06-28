#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_RenameTool : EditorWindow
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
        private const float fieldsWidth = 100;
        #endregion
        #region Rename Tool Values
        private static string newName = "";
        private static bool automaticIndexing = true;
        private static int startingIndex = 0;
        [SerializeField] private List<GameObject> selectedGameObjects = new List<GameObject>();
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
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out GUIStyle _, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Rename Tool", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region New Values
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Parameters:", contentGUIStyle, GUILayout.ExpandWidth(true));
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(fieldsWidth))
            {
                newName = EditorGUILayout.TextField("New Name", newName);
                GUILayout.Space(2);
                automaticIndexing = HierarchyDesigner_Shared_GUI.DrawToggle("Auto-Index", fieldsWidth, automaticIndexing);
                if (automaticIndexing) { startingIndex = EditorGUILayout.DelayedIntField("Starting Index", startingIndex); }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Selected GameObjects List
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Selected GameObjects:", contentGUIStyle, GUILayout.ExpandWidth(true));
            GUILayout.Space(4);
            if (reorderableList != null) { reorderableList.DoLayoutList(); }
            EditorGUILayout.EndVertical();
            #endregion
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

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

        #region Operations Methods
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