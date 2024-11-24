#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Utility_GameObject
    {
        #region Context menu
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Refresh + "/Refresh All GameObjects' Data", false, HierarchyDesigner_Shared_MenuItems.LayerEighteen)]
        public static void ContextMenu_Refresh_AllGameObjectsData() => RefreshAllGameObjectsData();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Refresh + "/Refresh Selected GameObject's Data", false, HierarchyDesigner_Shared_MenuItems.LayerEighteen)]
        public static void ContextMenu_Refresh_SelectedGameObjectsData() => RefreshSelectedGameObjectsData();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Refresh + "/Refresh Selected Main Icon", false, HierarchyDesigner_Shared_MenuItems.LayerNineteen)]
        public static void ContextMenu_Refresh_SelectedMainIcon() => RefreshMainIconForSelectedGameObject();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Refresh + "/Refresh Selected Component Icons", false, HierarchyDesigner_Shared_MenuItems.LayerNineteen + 1)]
        public static void ContextMenu_Refresh_SelectedComponentIcons() => RefreshComponentIconsForSelectedGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Refresh + "/Refresh Selected Hierarchy Tree Icon", false, HierarchyDesigner_Shared_MenuItems.LayerNineteen + 1)]
        public static void ContextMenu_Refresh_SelectedHierarchyTreeIcon() => RefreshHierarchyTreeIconForSelectedGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Refresh + "/Refresh Selected Tag", false, HierarchyDesigner_Shared_MenuItems.LayerNineteen + 1)]
        public static void ContextMenu_Refresh_SelectedTag() => RefreshTagForSelectedGameObjects();

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Refresh + "/Refresh Selected Layer", false, HierarchyDesigner_Shared_MenuItems.LayerNineteen + 2)]
        public static void ContextMenu_Refresh_SelectedLayer() => RefreshLayerForSelectedGameObjects();
        #endregion

        #region Methods
        public static void RefreshAllGameObjectsData()
        {
            if (!HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectMainIcon &&
                !HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectComponentIcons &&
                !HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyTree &&
                !HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectTag &&
                !HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectLayer)
            {
                Debug.Log("No GameObject data is enabled for refreshing.");
                return;
            }

            #if UNITY_6000_0_OR_NEWER
            GameObject[] allGameObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            #else
            GameObject[] allGameObjects = Object.FindObjectsOfType<GameObject>();
            #endif

            foreach (GameObject gameObject in allGameObjects)
            {
                RefreshGameObjectData(gameObject);
            }
        }

        public static void RefreshSelectedGameObjectsData()
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length > 0)
            {
                foreach (GameObject selectedGameObject in selectedGameObjects)
                {
                    RefreshGameObjectData(selectedGameObject);
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select one or more GameObjects to refresh their data.");
            }
        }

        public static void RefreshMainIconForSelectedGameObject()
        {
            if (!HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectMainIcon) { return; }

            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length > 0)
            {
                foreach (GameObject selectedGameObject in selectedGameObjects)
                {
                    int instanceID = selectedGameObject.GetInstanceID();
                    if (HierarchyDesigner_Manager_GameObject.gameObjectDataCache.TryGetValue(instanceID, out HierarchyDesigner_Manager_GameObject.GameObjectData data))
                    {
                        data.MainIcon = HierarchyDesigner_Manager_GameObject.GetGameObjectMainIcon(selectedGameObject);
                        HierarchyDesigner_Manager_GameObject.gameObjectDataCache[instanceID] = data;
                    }
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select a GameObject to refresh its main icon.");
            }
        }

        public static void RefreshComponentIconsForSelectedGameObjects()
        {
            if (!HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectComponentIcons) { return; }

            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length > 0)
            {
                foreach (GameObject selectedGameObject in selectedGameObjects)
                {
                    int instanceID = selectedGameObject.GetInstanceID();
                    if (HierarchyDesigner_Manager_GameObject.gameObjectDataCache.TryGetValue(instanceID, out HierarchyDesigner_Manager_GameObject.GameObjectData data))
                    {
                        data.ComponentIcons = HierarchyDesigner_Manager_GameObject.GetComponentIcons(selectedGameObject);
                        HierarchyDesigner_Manager_GameObject.gameObjectDataCache[instanceID] = data;
                    }
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select one or more GameObjects to refresh their component icons.");
            }
        }

        public static void RefreshHierarchyTreeIconForSelectedGameObjects()
        {
            if (!HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyTree) { return; }

            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length > 0)
            {
                foreach (GameObject selectedGameObject in selectedGameObjects)
                {
                    int instanceID = selectedGameObject.GetInstanceID();
                    if (HierarchyDesigner_Manager_GameObject.gameObjectDataCache.TryGetValue(instanceID, out HierarchyDesigner_Manager_GameObject.GameObjectData data))
                    {
                        data.HierarchyTreeIcon = HierarchyDesigner_Manager_GameObject.GetOrCreateBranchIcon(selectedGameObject.transform);
                        HierarchyDesigner_Manager_GameObject.gameObjectDataCache[instanceID] = data;
                    }
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select one or more GameObjects to refresh their hierarchy tree icons.");
            }
        }

        public static void RefreshTagForSelectedGameObjects()
        {
            if (!HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectTag) { return; }

            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length > 0)
            {
                foreach (GameObject selectedGameObject in selectedGameObjects)
                {
                    int instanceID = selectedGameObject.GetInstanceID();
                    if (HierarchyDesigner_Manager_GameObject.gameObjectDataCache.TryGetValue(instanceID, out HierarchyDesigner_Manager_GameObject.GameObjectData data))
                    {
                        data.Tag = selectedGameObject.tag;
                        HierarchyDesigner_Manager_GameObject.gameObjectDataCache[instanceID] = data;
                    }
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select one or more GameObjects to refresh their tags.");
            }
        }

        public static void RefreshLayerForSelectedGameObjects()
        {
            if (!HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectLayer) { return; }

            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length > 0)
            {
                foreach (GameObject selectedGameObject in selectedGameObjects)
                {
                    int instanceID = selectedGameObject.GetInstanceID();
                    if (HierarchyDesigner_Manager_GameObject.gameObjectDataCache.TryGetValue(instanceID, out HierarchyDesigner_Manager_GameObject.GameObjectData data))
                    {
                        data.Layer = LayerMask.LayerToName(selectedGameObject.layer);
                        HierarchyDesigner_Manager_GameObject.gameObjectDataCache[instanceID] = data;
                    }
                }
            }
            else
            {
                Debug.LogWarning("No GameObject selected. Please select one or more GameObjects to refresh their layers.");
            }
        }
        #endregion

        #region Operations
        private static void RefreshGameObjectData(GameObject gameObject)
        {
            int instanceID = gameObject.GetInstanceID();
            if (!HierarchyDesigner_Manager_GameObject.gameObjectDataCache.TryGetValue(instanceID, out HierarchyDesigner_Manager_GameObject.GameObjectData data))
            {
                data = new HierarchyDesigner_Manager_GameObject.GameObjectData();
            }

            if (HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectMainIcon)
            {
                data.MainIcon = HierarchyDesigner_Manager_GameObject.GetGameObjectMainIcon(gameObject);
            }

            if (HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectComponentIcons)
            {
                data.ComponentIcons = HierarchyDesigner_Manager_GameObject.GetComponentIcons(gameObject);
            }

            if (HierarchyDesigner_Configurable_GeneralSettings.EnableHierarchyTree && gameObject.transform.parent != null)
            {
                data.HierarchyTreeIcon = HierarchyDesigner_Manager_GameObject.GetOrCreateBranchIcon(gameObject.transform);
            }

            if (HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectTag)
            {
                data.Tag = gameObject.tag;
            }

            if (HierarchyDesigner_Configurable_GeneralSettings.EnableGameObjectLayer)
            {
                data.Layer = LayerMask.LayerToName(gameObject.layer);
            }

            HierarchyDesigner_Manager_GameObject.gameObjectDataCache[instanceID] = data;
        }
        #endregion
    }
}
#endif