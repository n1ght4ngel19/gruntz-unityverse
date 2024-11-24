#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using System;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Manager_Data
    {
        #region Properties
        private const string mainFolderName = "Hierarchy Designer";
        private const string editorFolderName = "Editor";
        private const string savedDataFolderName = "Saved Data";
        private const string scriptsFolderName = "Scripts";
        private const string documentationFolderName = "Documentation";
        private const string patchNotesFileName = "Hierarchy Designer Patch Notes.txt";
        private static readonly string gistUrl = "https://gist.github.com/PedroVerpha/ae63e5c3b9934e4c8d39ce4d3afba18f";
        private static readonly string targetFileName = "HierarchyDesignerUpdateBoard.txt";
        private static readonly string githubUrl = "https://raw.githubusercontent.com/PedroVerpha/Hierarchy-Designer/main/Hierarchy%20Designer.unitypackage";
        private static readonly string unityPackageFileName = "Hierarchy Designer.unitypackage";
        private static readonly string versionUrl = "https://raw.githubusercontent.com/PedroVerpha/Hierarchy-Designer/main/Hierarchy%20Designer%20Current%20Version.txt";
        private static readonly string currentHierarchyDesignerVersionInProject = "1.1.8";
        #endregion

        #region Accessors
        public static string CurrentVersion => currentHierarchyDesignerVersionInProject;
        #endregion

        #region Methods
        public static string GetDataFilePath(string fileName)
        {
            string fullPath = GetFullPath(savedDataFolderName);
            return Path.Combine(fullPath, fileName);
        }

        public static string GetScriptsFilePath(string fileName)
        {
            string fullPath = GetFullPath(scriptsFolderName);
            return Path.Combine(fullPath, fileName);
        }

        public static string GetPatchNotesFilePath()
        {
            string fullPath = GetFullPath(documentationFolderName);
            return Path.Combine(fullPath, patchNotesFileName);
        }

        private static string GetFullPath(string subFolderName)
        {
            string rootPath = FindHierarchyDesignerRootPath();
            string fullPath = Path.Combine(rootPath, editorFolderName, subFolderName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                AssetDatabase.Refresh();
            }
            return fullPath;
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

        public static async Task<string> FetchMessagesFromGist()
        {
            string rawUrl = await GetRawUrlFromGistPage();
            if (string.IsNullOrEmpty(rawUrl))
            {
                return "Failed to load messages.";
            }

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(rawUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    //Debug.LogError($"Failed to fetch messages from Gist raw URL: {response.ReasonPhrase}");
                    return "Failed to load messages.";
                }
            }
        }

        private static async Task<string> GetRawUrlFromGistPage()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "UnityHttpClient");
                HttpResponseMessage response = await client.GetAsync(gistUrl);

                if (response.IsSuccessStatusCode)
                {
                    string htmlContent = await response.Content.ReadAsStringAsync();
                    string pattern = $@"href=[""'](?<url>\/PedroVerpha\/[a-zA-Z0-9]+\/raw\/[a-zA-Z0-9]+\/{Regex.Escape(targetFileName)})[""']";
                    Match match = Regex.Match(htmlContent, pattern);

                    if (match.Success)
                    {
                        string rawUrlPart = match.Groups["url"].Value;
                        string rawUrl = "https://gist.githubusercontent.com" + rawUrlPart;
                        return rawUrl;
                    }
                    else
                    {
                        //Debug.LogError("Failed to parse raw URL from Gist HTML.");
                        return null;
                    }
                }
                else
                {
                    //Debug.LogError($"Failed to fetch Gist page: {response.ReasonPhrase}");
                    return null;
                }
            }
        }

        public static async Task DownloadLatestHierarchyDesignerVersion()
        {
            string latestVersion = await FetchLatestVersionFromGithub();

            if (IsNewerVersion(latestVersion, currentHierarchyDesignerVersionInProject))
            {
                Debug.Log($"Hierarchy Designer: A newer version ({latestVersion}) is available. Downloading update...");
                string downloadPath = GetDataFilePath(unityPackageFileName);
                await DownloadFileFromGithub(downloadPath);
            }
            else
            {
                EditorUtility.DisplayDialog("No Update Needed.", "You already have the latest version of Hierarchy Designer.", "OK");
            }
        }

        private static async Task<string> FetchLatestVersionFromGithub()
        {
            using UnityWebRequest request = UnityWebRequest.Get(versionUrl);
            #if UNITY_6000_0_OR_NEWER
            await request.SendWebRequest();
            #else
            request.SendWebRequest();
            #endif

            while (!request.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text.Trim();
            }
            else
            {
                Debug.LogError($"Failed to fetch the latest version: {request.error}");
                return null;
            }
        }

        private static bool IsNewerVersion(string latestVersion, string currentVersion)
        {
            if (string.IsNullOrEmpty(latestVersion) || string.IsNullOrEmpty(currentVersion)) return false;

            Version latest = new Version(latestVersion);
            Version current = new Version(currentVersion);

            return latest.CompareTo(current) > 0;
        }

        private static async Task DownloadFileFromGithub(string downloadPath)
        {
            using UnityWebRequest request = UnityWebRequest.Get(githubUrl);
            request.downloadHandler = new DownloadHandlerFile(downloadPath);
            #if UNITY_6000_0_OR_NEWER
            await request.SendWebRequest();
            #else
            request.SendWebRequest();
            #endif

            while (!request.isDone)
            {
                EditorUtility.DisplayProgressBar("Downloading", "Downloading the latest asset package...", request.downloadProgress);
                await Task.Yield();
            }

            EditorUtility.ClearProgressBar();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File downloaded successfully.");

                if (IsValidUnityPackage(downloadPath))
                {
                    ImportDownloadedPackage(downloadPath);
                }
                else
                {
                    Debug.LogError("The downloaded file is not a valid Unity package.");
                    EditorUtility.DisplayDialog("Invalid Package", "The downloaded file is not a valid Unity package. Please check the file or try again later.", "OK");
                }
            }
            else
            {
                Debug.LogError($"Failed to download file: {request.error}");
                EditorUtility.DisplayDialog("Download Failed", "Failed to download the latest version. Please check your internet connection or try again later.", "OK");
            }
        }

        private static bool IsValidUnityPackage(string filePath)
        {
            try
            {
                using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4];
                fs.Read(buffer, 0, 4);
                return buffer[0] == 0x1F && buffer[1] == 0x8B;
            }
            catch
            {
                return false;
            }
        }

        private static void ImportDownloadedPackage(string packagePath)
        {
            if (File.Exists(packagePath))
            {
                AssetDatabase.ImportPackage(packagePath, true);
                Debug.Log("Package imported successfully.");
                EditorUtility.DisplayDialog("Download Complete", "The latest asset version has been downloaded successfully.", "OK");
            }
            else
            {
                Debug.LogError("Failed to import package: File does not exist.");
            }
        }
#endregion
    }
}
#endif