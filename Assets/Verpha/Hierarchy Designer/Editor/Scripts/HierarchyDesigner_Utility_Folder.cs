#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Utility_Folder
    {
        #region Menu Items
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folders + "/Create All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateAllFolders()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData> folder in HierarchyDesigner_Configurable_Folder.GetAllFoldersData(false))
            {
                CreateFolder(folder.Key, false);
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folders + "/Create Default Folder", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateDefaultFolder()
        {
            CreateFolder("New Folder", true);
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folders + "/Create Missing Folders", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateMissingFolders()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData> folder in HierarchyDesigner_Configurable_Folder.GetAllFoldersData(false))
            {
                if (!FolderExists(folder.Key))
                {
                    CreateFolder(folder.Key, false);
                }
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folders + "/Transform GameObject into a Folder", false, HierarchyDesigner_Shared_MenuItems.LayerOne)]
        public static void TransformGameObjectIntoAFolder()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogWarning("No GameObject selected.");
                return;
            }

            string folderName = selectedObject.name;
            HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folderData = HierarchyDesigner_Configurable_Folder.GetFolderData(folderName);
            if (folderData == null)
            {
                HierarchyDesigner_Configurable_Folder.SetFolderData(
                    folderName,
                    HierarchyDesigner_Configurable_DesignSettings.FolderDefaultTextColor,
                    HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontSize,
                    HierarchyDesigner_Configurable_DesignSettings.FolderDefaultFontStyle,
                    HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageColor,
                    HierarchyDesigner_Configurable_DesignSettings.FolderDefaultImageType
                );
                if (selectedObject.GetComponent<HierarchyDesignerFolder>() == null)
                {
                    selectedObject.AddComponent<HierarchyDesignerFolder>();
                }
                EditorGUIUtility.SetIconForObject(selectedObject, HierarchyDesigner_Shared_Resources.FolderInspectorIcon);
                Debug.Log($"GameObject <color=#73FF7A>'{folderName}'</color> was transformed into a Folder and added to the Folders dictionary.");
            }
            else
            {
                Debug.LogWarning($"GameObject <color=#FF7674>'{folderName}'</color> already exists in the Folders dictionary.");
                return;
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folders + "/Transform Folder into a GameObject", false, HierarchyDesigner_Shared_MenuItems.LayerOne + 1)]
        public static void TransformFolderIntoAGameObject()
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject == null)
            {
                Debug.LogWarning("No GameObject selected.");
                return;
            }

            HierarchyDesignerFolder folderComponent = selectedObject.GetComponent<HierarchyDesignerFolder>();
            if (folderComponent == null)
            {
                Debug.LogWarning($"GameObject <color=#FF7674>'{selectedObject.name}'</color> is not a Folder.");
                return;
            }

            string folderName = selectedObject.name;
            if (HierarchyDesigner_Configurable_Folder.RemoveFolderData(folderName))
            {
                Object.DestroyImmediate(folderComponent);
                EditorGUIUtility.SetIconForObject(selectedObject, null);
                Debug.Log($"GameObject <color=#73FF7A>'{folderName}'</color> was transformed back into a GameObject and removed from the Folders dictionary.");
            }
            else
            {
                Debug.LogWarning($"Folder data for GameObject <color=#FF7674>'{folderName}'</color> does not exist in the Folders dictionary.");
            }
        }
        #endregion

        #region Operations
        private static void CreateFolder(string folderName, bool shouldRename)
        {
            GameObject folder = new GameObject(folderName);
            folder.AddComponent<HierarchyDesignerFolder>();
            EditorGUIUtility.SetIconForObject(folder, HierarchyDesigner_Shared_Resources.FolderInspectorIcon);
            if (shouldRename) { EditorApplication.delayCall += () => BeginRename(folder); }
            Undo.RegisterCreatedObjectUndo(folder, $"Create {folderName}");
        }

        private static void BeginRename(GameObject obj)
        {
            Selection.activeObject = obj;
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
            EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent("Rename"));
        }

        private static bool FolderExists(string folderName)
        {
            #if UNITY_6000_0_OR_NEWER
            Transform[] allTransforms = GameObject.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            #else
            Transform[] allTransforms = Object.FindObjectsOfType<Transform>(true);
            #endif
            foreach (Transform t in allTransforms)
            {
                if (t.gameObject.GetComponent<HierarchyDesignerFolder>() && t.gameObject.name.Equals(folderName))
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