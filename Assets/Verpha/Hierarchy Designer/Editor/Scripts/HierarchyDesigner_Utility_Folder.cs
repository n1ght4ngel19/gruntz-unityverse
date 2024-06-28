#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Utility_Folder
    {
        #region Properties
        private static readonly Texture2D folderInspectorIcon = HierarchyDesigner_Shared_Resources.FolderInspectorIcon;
        #endregion

        #region Menu Items
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folder + "/Create All Folders", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateAllFolders()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData> folder in HierarchyDesigner_Configurable_Folder.GetAllFoldersData(false))
            {
                CreateFolder(folder.Key, false);
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folder + "/Create Default Folder", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateDefaultFolder()
        {
            CreateFolder("New Folder", true);
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folder + "/Create Missing Folders", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
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
        #endregion

        #region Methods
        private static void CreateFolder(string folderName, bool shouldRename)
        {
            GameObject folder = new GameObject(folderName);
            folder.AddComponent<HierarchyDesignerFolder>();
            if (folderInspectorIcon != null) { EditorGUIUtility.SetIconForObject(folder, folderInspectorIcon); }
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