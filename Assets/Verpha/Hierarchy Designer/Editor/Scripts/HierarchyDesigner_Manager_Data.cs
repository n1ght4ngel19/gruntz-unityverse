#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Manager_Data
    {
        #region Properties
        private const string mainFolderName = "Hierarchy Designer";
        private const string editorFolderName = "Editor";
        private const string savedDataFolderName = "Saved Data";
        private const string scriptsFolderName = "Scripts";
        #endregion

        #region Path Methods
        public static string GetDataFilePath(string fileName)
        {
            string rootPath = FindHierarchyDesignerRootPath();
            string fullPath = Path.Combine(rootPath, editorFolderName, savedDataFolderName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                AssetDatabase.Refresh();
            }
            return Path.Combine(fullPath, fileName);
        }

        public static string GetScriptsFilePath(string fileName)
        {
            string rootPath = FindHierarchyDesignerRootPath();
            string fullPath = Path.Combine(rootPath, editorFolderName, scriptsFolderName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                AssetDatabase.Refresh();
            }
            return Path.Combine(fullPath, fileName);
        }

        private static string FindHierarchyDesignerRootPath()
        {
            string[] guids = AssetDatabase.FindAssets(mainFolderName);
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (Directory.Exists(path) && path.EndsWith(mainFolderName))
                {
                    return path;
                }
            }
            Debug.LogWarning($"Hierarchy Designer root path not found. Defaulting to {Path.Combine(Application.dataPath, mainFolderName)}.");
            return Path.Combine(Application.dataPath, mainFolderName);
        }
        #endregion
    }
}
#endif