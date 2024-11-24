#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [CustomEditor(typeof(HierarchyDesignerFolder))]
    internal class HierarchyDesigner_Inspector_Folder : Editor
    {
        #region Properties
        #region Serialized Properties
        private SerializedProperty flattenFolderProp;
        private SerializedProperty flattenEventProp;
        private SerializedProperty onFlattenEventProp;
        private SerializedProperty onFolderDestroyProp;
        #endregion
        #region GUI
        private GUIStyle rightPanel;
        private GUIStyle categoryLabel;
        private GUIStyle contentPanel;
        private GUIStyle contentLabel;
        private float maxLabelWidth = 100;
        private List<GUILayoutOption[]> cachedGUIOptions;
        #endregion
        #region Consts
        private const int defaultGUISpace = 2;
        private const int minButtonWidth = 55;
        private const int maxButtonWidth = 100;
        private const string toggle = "Toggle";
        private const string select = "Select";
        private const string viewInScene = "View in Scene";
        private const string delete = "Delete";
        private const int maxAllowedChildren = 500;
        #endregion
        #region Cache and Conditions
        private bool doOnce = false;
        private bool showChildren = true;
        private bool childrenCached = false;
        private HierarchyDesignerFolder folder;
        private List<Transform> cachedChildren = new List<Transform>();
        private int totalChildCount = 0;
        #endregion
        #endregion

        #region Initialization
        private void OnEnable()
        {
            folder = (HierarchyDesignerFolder)target;

            flattenFolderProp = serializedObject.FindProperty("flattenFolder");
            flattenEventProp = serializedObject.FindProperty("flattenEvent");
            onFlattenEventProp = serializedObject.FindProperty("OnFlattenEvent");
            onFolderDestroyProp = serializedObject.FindProperty("OnFolderDestroy");

            CacheChildren();
            ProcessChildren();
        }
        #endregion

        #region Main
        public override void OnInspectorGUI()
        {
            HierarchyDesigner_Shared_GUI.GetHierarchyDesignerGUIStyles(out GUIStyle _, out GUIStyle _, out GUIStyle _, out GUIStyle _, out rightPanel, out categoryLabel, out GUIStyle _, out GUIStyle _, out contentPanel, out contentLabel, out GUIStyle _, out GUIStyle _, out GUIStyle _);
            serializedObject.Update();

            #region Runtime
            float originalLabelWidth = EditorGUIUtility.labelWidth;
            float originalFieldWidth = EditorGUIUtility.fieldWidth;
            EditorGUIUtility.labelWidth = 90;
            EditorGUIUtility.fieldWidth = maxButtonWidth;

            EditorGUILayout.BeginVertical(rightPanel);
            EditorGUILayout.LabelField("Hierarchy Designer's Folder", categoryLabel);
            EditorGUILayout.Space(defaultGUISpace);

            EditorGUILayout.BeginVertical(contentPanel);
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Settings", contentLabel);
            EditorGUILayout.Space(defaultGUISpace);

            EditorGUILayout.PropertyField(flattenFolderProp);
            if (flattenFolderProp.boolValue)
            {
                EditorGUILayout.PropertyField(flattenEventProp);
                EditorGUILayout.Space(6);

                EditorGUILayout.LabelField("Events", contentLabel);
                EditorGUILayout.Space(defaultGUISpace);

                EditorGUILayout.PropertyField(onFlattenEventProp);
                EditorGUILayout.Space(defaultGUISpace);

                EditorGUILayout.PropertyField(onFolderDestroyProp);
            }
            EditorGUILayout.EndVertical();
            EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUIUtility.fieldWidth = originalFieldWidth;

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            #endregion

            #region Editor
            if (!HierarchyDesigner_Configurable_AdvancedSettings.IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder) return;
            if(totalChildCount > maxAllowedChildren)
            {
                EditorGUILayout.HelpBox($"This folder contains {totalChildCount} gameObject children, which exceeds the maximum allowed limit of {maxAllowedChildren} children. The editor utility is disabled for this folder.", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginVertical(contentPanel);
            if (!childrenCached)
            {
                CacheChildren();
            }
            if (!doOnce)
            {
                maxLabelWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(folder.transform);
                for (int i = 0; i < cachedGUIOptions.Count; i++)
                {
                    cachedGUIOptions[i][0] = GUILayout.Width(maxLabelWidth);
                }
                doOnce = true;
            }

            EditorGUILayout.BeginVertical(rightPanel);
            EditorGUILayout.LabelField("(Editor-Only)", contentLabel);
            EditorGUILayout.Space(defaultGUISpace);
            if (GUILayout.Button("Refresh Children List", GUILayout.Height(20)))
            {
                childrenCached = false;
                doOnce = false;
                CacheChildren();
                ProcessChildren();
                maxLabelWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(folder.transform);
            }
            EditorGUILayout.Space(defaultGUISpace);
            EditorGUILayout.LabelField("Folder Stats:", HierarchyDesigner_Shared_GUI.InspectorContentGUIStyle);
            EditorGUILayout.Space(defaultGUISpace);
            EditorGUILayout.LabelField("This folder contains:", HierarchyDesigner_Shared_GUI.InspectorMessageBoldGUIStyle);
            EditorGUILayout.LabelField($"- '{totalChildCount}' gameObject children.", HierarchyDesigner_Shared_GUI.InspectorMessageItalicGUIStyle);
            EditorGUILayout.Space(defaultGUISpace);

            if (totalChildCount > 0)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(11);
                showChildren = EditorGUILayout.Foldout(showChildren, "GameObject's Children List");
                EditorGUILayout.EndHorizontal();
            }
            if (showChildren)
            {
                DisplayCachedChildren();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            #endregion
        }

        private void OnDisable()
        {
            cachedChildren.Clear();
            cachedGUIOptions.Clear();
            childrenCached = false;
        }
        #endregion

        #region Operations (Editor)
        private void CacheChildren()
        {
            cachedChildren.Clear();
            GetChildTransforms(folder.transform, cachedChildren);
            totalChildCount = cachedChildren.Count;
            childrenCached = true;
        }

        private void GetChildTransforms(Transform parent, List<Transform> children)
        {
            foreach (Transform child in parent)
            {
                children.Add(child);
                GetChildTransforms(child, children);
            }
        }

        private void ProcessChildren()
        {
            cachedGUIOptions = new List<GUILayoutOption[]>(totalChildCount);

            for (int i = 0; i < totalChildCount; i++)
            {
                GUILayoutOption[] options = new GUILayoutOption[4];
                options[0] = GUILayout.Width(maxLabelWidth);
                options[1] = GUILayout.MinWidth(minButtonWidth);
                options[2] = GUILayout.ExpandWidth(true);
                options[3] = GUILayout.MinWidth(maxButtonWidth);
                cachedGUIOptions.Add(options);
            }
        }

        private void DisplayCachedChildren()
        {
            for (int i = 0; i < cachedChildren.Count; i++)
            {
                Transform child = cachedChildren[i];
                GUILayoutOption[] options = cachedGUIOptions[i];

                EditorGUILayout.BeginHorizontal();
                GUIStyle currentStyle = child.gameObject.activeSelf ? EditorStyles.label : HierarchyDesigner_Shared_GUI.InactiveLabelGUIStyle;
                EditorGUILayout.LabelField(child.name, currentStyle, options[0]);

                if (GUILayout.Button(toggle, options[1], options[2]))
                {
                    Undo.RecordObject(child.gameObject, "Toggle Active State");
                    child.gameObject.SetActive(!child.gameObject.activeSelf);
                }
                if (GUILayout.Button(select, options[1], options[2]))
                {
                    EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
                    Selection.activeGameObject = child.gameObject;
                }
                if (GUILayout.Button(viewInScene, options[3], options[2]))
                {
                    GameObject originalSelection = Selection.activeGameObject;
                    Selection.activeGameObject = child.gameObject;
                    SceneView.FrameLastActiveSceneView();
                    Selection.activeGameObject = originalSelection;
                }
                if (GUILayout.Button(delete, options[1], options[2]))
                {
                    Undo.DestroyObjectImmediate(child.gameObject);
                    cachedChildren.Remove(child);
                    cachedGUIOptions.RemoveAt(i);
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        #endregion
    }
}
#endif