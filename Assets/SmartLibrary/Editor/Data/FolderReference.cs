using System;
using UnityEngine;
using UnityEditor;

namespace Bewildered.SmartLibrary
{
    public enum FolderMatchOption { AnyDepth, TopOnly }

    /// <summary>
    /// Used to reference a folder by its GUID, and determine how its contents should be evaluated.
    /// </summary>
    [Serializable]
    public class FolderReference
    {
        [SerializeField] private FolderMatchOption _matchOption;
        [SerializeField] private string _folderGuid = string.Empty;
        [SerializeField] private bool _doInclude = true;

        public FolderMatchOption MatchOption
        {
            get { return _matchOption; }
            set { _matchOption = value; }
        }

        /// <summary>
        /// The asset GUID of the folder. Returns an empty string if no folder is set.
        /// </summary>
        public string FolderGuid
        {
            get { return _folderGuid; }
            set { _folderGuid = value; }
        }

        /// <summary>
        /// The path of the folder that is set. Returns an empty string if no folder is set or an invalid one is.
        /// </summary>
        public string Path
        {
            get
            {
                // We get the path from the GUID every time since we don't know when the folder may have changed path.
                return AssetDatabase.GUIDToAssetPath(_folderGuid);
            }
        }

        /// <summary>
        /// Whether assets in the referenced folder should be be included or excluded.
        /// </summary>
        public bool DoInclude
        {
            get { return _doInclude; }
            set { _doInclude = value; }
        }

        /// <summary>
        /// Determines if the specified path is within the <see cref="FolderReference"/> and
        /// if it matches the <see cref="MatchOption"/>.
        /// </summary>
        /// <param name="path">The path to compare with.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="path"/> is inside the referenced folder and matches <see cref="MatchOption"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidPath(string path)
        {
            // We cache the folder path since each call requires getting the path from the GUID.
            var folderPath = Path;
            if (string.IsNullOrEmpty(folderPath))
                return false;

            if (path.Equals(folderPath, StringComparison.InvariantCultureIgnoreCase))
                return false;

            bool contains = path.Contains(folderPath);

            if (_matchOption == FolderMatchOption.TopOnly)
            {
                // Insure that the path isn't longer than the folder path while excluding the file name.
                contains &= path.LastIndexOf('/') + 1 == folderPath.Length;
            }

            return _doInclude ? contains : !contains;
        }
    }
}
