#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Utility_Separator
    {
        #region Menu Items
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separators + "/Create All Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateAllSeparators()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separator in HierarchyDesigner_Configurable_Separator.GetAllSeparatorsData(false))
            {
                CreateSeparator(separator.Key);
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separators + "/Create Default Separator", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateDefaultSeparator()
        {
            CreateSeparator("Separator");
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separators + "/Create Missing Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateMissingSeparators()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separator in HierarchyDesigner_Configurable_Separator.GetAllSeparatorsData(false))
            {
                if (!SeparatorExists(separator.Key))
                {
                    CreateSeparator(separator.Key);
                }
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separators + "/Transform GameObject into a Separator", false, HierarchyDesigner_Shared_MenuItems.LayerOne)]
        public static void TransformGameObjectIntoASeparator()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogWarning("No GameObject selected.");
                return;
            }
            if (selectedObject.GetComponents<Component>().Length > 1)
            {
                Debug.LogWarning("Separators cannot have components because separators are marked as editorOnly, meaning they will not be present in your game's build.");
                return;
            }

            string separatorName = HierarchyDesigner_Configurable_Separator.StripPrefix(selectedObject.name);
            HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData = HierarchyDesigner_Configurable_Separator.GetSeparatorData(separatorName);
            if (separatorData == null)
            {
                HierarchyDesigner_Configurable_Separator.SetSeparatorData(
                    separatorName,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextColor,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultIsGradientBackground,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundColor,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultBackgroundGradient,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontSize,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultFontStyle,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultTextAnchor,
                    HierarchyDesigner_Configurable_DesignSettings.SeparatorDefaultImageType
                );
                if (!selectedObject.name.StartsWith("//"))
                {
                    selectedObject.name = $"//{selectedObject.name}";
                }
                selectedObject.tag = "EditorOnly";
                selectedObject.SetActive(false);
                EditorGUIUtility.SetIconForObject(selectedObject, HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon);
                Debug.Log($"GameObject <color=#73FF7A>'{separatorName}'</color> was transformed into a Separator and added to the Separators dictionary.");
            }
            else
            {
                Debug.LogWarning($"GameObject <color=#FF7674>'{separatorName}'</color> already exists in the Separators dictionary.");
                return;
            }
            SetSeparatorState(selectedObject, false);
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separators + "/Transform Separator into a GameObject", false, HierarchyDesigner_Shared_MenuItems.LayerOne + 1)]
        public static void TransformSeparatorIntoAGameObject()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogWarning("No GameObject selected.");
                return;
            }

            if (!selectedObject.name.StartsWith("//") && selectedObject.tag != "EditorOnly")
            {
                Debug.LogWarning($"GameObject <color=#FF7674>'{selectedObject.name}'</color> is not a Separator.");
                return;
            }

            string separatorName = HierarchyDesigner_Configurable_Separator.StripPrefix(selectedObject.name);
            if (HierarchyDesigner_Configurable_Separator.RemoveSeparatorData(separatorName))
            {
                selectedObject.name = separatorName;
                selectedObject.tag = "Untagged";
                selectedObject.SetActive(true);
                HierarchyDesigner_Utility_Tools.SetLockState(selectedObject, true);
                EditorGUIUtility.SetIconForObject(selectedObject, null);
                Debug.Log($"GameObject <color=#73FF7A>'{separatorName}'</color> was transformed back into a GameObject and removed from the Separators dictionary.");
            }
            else
            {
                Debug.LogWarning($"Separator data for GameObject <color=#FF7674>'{separatorName}'</color> does not exist in the Separators dictionary.");
            }
        }
        #endregion

        #region Operations
        private static void CreateSeparator(string separatorName)
        {
            GameObject separator = new GameObject($"//{separatorName}");
            separator.tag = "EditorOnly";
            SetSeparatorState(separator, false);
            separator.SetActive(false);
            EditorGUIUtility.SetIconForObject(separator, HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon);
            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorName}");
        }

        public static void SetSeparatorState(GameObject gameObject, bool editable)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component) 
                {
                    component.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable; 
                }
            }
            gameObject.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            gameObject.transform.hideFlags = HideFlags.HideInInspector;
            EditorUtility.SetDirty(gameObject);
        }

        private static bool SeparatorExists(string separatorName)
        {
            string fullSeparatorName = "//" + separatorName;
            #if UNITY_6000_0_OR_NEWER
            Transform[] allTransforms = GameObject.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            #else
            Transform[] allTransforms = Object.FindObjectsOfType<Transform>(true);
            #endif
            foreach (Transform t in allTransforms)
            {
                if (t.gameObject.CompareTag("EditorOnly") && t.gameObject.name.Equals(fullSeparatorName))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
#endif