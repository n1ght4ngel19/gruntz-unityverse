#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_TagLayer : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 outerScroll;
        private GUIStyle headerGUIStyle;
        private GUIStyle contentGUIStyle;
        private GUIStyle messageGUIStyle;
        private GUIStyle outerBackgroundGUIStyle;
        private GUIStyle innerBackgroundGUIStyle;
        private GUIStyle contentBackgroundGUIStyle;
        #endregion
        #region Const
        private const float tagLabelWidth = 80;
        private const float layerLabelWidth = 90;
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
            window.minSize = new Vector2(250, 120);
            window.position = new Rect(position, new Vector2(300, 120));
            window.gameObject = gameObject;
            window.isTag = isTag;
            window.windowLabel = isTag ? "Tag" : "Layer";
        }
        #endregion

        #region Main
        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out messageGUIStyle, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            bool cancelLayout = false;
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField(windowLabel, headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("GameObject:", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField(gameObject.name, EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            GUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            if (isTag)
            {
                EditorGUILayout.LabelField("Current Tag:", EditorStyles.boldLabel, GUILayout.Width(tagLabelWidth));
                string newTag = EditorGUILayout.TagField(gameObject.tag, GUILayout.Width(180));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(gameObject, "Change Tag");
                    gameObject.tag = newTag;
                    Close();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Current Layer:", EditorStyles.boldLabel, GUILayout.Width(layerLabelWidth));
                int newLayer = EditorGUILayout.LayerField(gameObject.layer, GUILayout.Width(180));
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

            if (cancelLayout) 
            { 
                return; 
            }
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