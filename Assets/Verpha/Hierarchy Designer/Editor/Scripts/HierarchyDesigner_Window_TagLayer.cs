#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Window_TagLayer : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 mainScroll;
        private GUIStyle rightPanel;
        private GUIStyle categoryLabel;
        private GUIStyle contentPanel;
        private GUIStyle fieldsLabel;
        #endregion
        #region Const
        private const float minimumSpaceMargin = 2f;
        #endregion
        #region Tag and Layer Values
        private GameObject gameObject;
        private bool isTag;
        private string windowLabel;
        #endregion
        #endregion

        #region Window
        public static void OpenWindow(GameObject gameObject, bool isTag, Vector2 position)
        {
            HierarchyDesigner_Window_TagLayer window = GetWindow<HierarchyDesigner_Window_TagLayer>("Tag and Layer");
            window.minSize = new Vector2(250, 105);
            window.maxSize = new Vector2(300, window.minSize.y);
            Vector2 offset = new Vector2(-12, 25);
            Vector2 adjustedPosition = position - offset;
            window.position = new Rect(adjustedPosition, window.minSize);
            window.gameObject = gameObject;
            window.isTag = isTag;
            window.windowLabel = isTag ? "Tag" : "Layer";
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.GetHierarchyDesignerGUIStyles(out GUIStyle _, out GUIStyle _, out GUIStyle _, out GUIStyle _, out rightPanel, out categoryLabel, out GUIStyle _, out GUIStyle _, out contentPanel, out _, out fieldsLabel, out GUIStyle _, out GUIStyle _);

            #region Header
            bool cancelLayout = false;
            EditorGUILayout.BeginVertical(rightPanel);
            EditorGUILayout.LabelField(windowLabel, categoryLabel);
            GUILayout.Space(minimumSpaceMargin);
            #endregion

            mainScroll = EditorGUILayout.BeginScrollView(mainScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(contentPanel);

            #region Body
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"<color={(HierarchyDesigner_Manager_Editor.IsProSkin ? "#FFEB5D" : "#5E70FF")}>GameObject:</color> {gameObject.name}", fieldsLabel, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            GUILayout.Space(minimumSpaceMargin);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            if (isTag)
            {
                string newTag = EditorGUILayout.TagField(gameObject.tag);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(gameObject, "Change Tag");
                    gameObject.tag = newTag;
                    Close();
                }
            }
            else
            {
                int newLayer = EditorGUILayout.LayerField(gameObject.layer);
                if (EditorGUI.EndChangeCheck())
                {
                    bool shouldChangeLayer = true;
                    if (gameObject.transform.childCount > 0)
                    {
                        int result = AskToChangeChildrenLayers(gameObject, newLayer);
                        if (result == 2)
                        {
                            shouldChangeLayer = false;
                            cancelLayout = true;
                        }
                    }
                    if (shouldChangeLayer)
                    {
                        Undo.RecordObject(gameObject, "Change Layer");
                        gameObject.layer = newLayer;
                        Close();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            #region Footer
            if (cancelLayout)
            {
                return;
            }
            #endregion
        }
        #endregion

        #region Operations
        private static int AskToChangeChildrenLayers(GameObject obj, int newLayer)
        {
            int option = EditorUtility.DisplayDialogComplex(
                           "Change Layer",
                           $"Do you want to set the layer to '{LayerMask.LayerToName(newLayer)}' for all child objects as well?",
                           "Yes, change children",
                           "No, this object only",
                           "Cancel"
                       );

            if (option == 0)
            {
                SetLayerRecursively(obj, newLayer);
            }

            return option;
        }

        private static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            foreach (Transform child in obj.transform)
            {
                Undo.RecordObject(child.gameObject, "Change Layer");
                child.gameObject.layer = newLayer;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        #endregion
    }
}
#endif