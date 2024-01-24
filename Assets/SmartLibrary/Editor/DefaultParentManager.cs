using System.Collections;
using System.Collections.Generic;
using Bewildered.SmartLibrary.UI;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Bewildered.SmartLibrary
{
    [InitializeOnLoad]
    internal static class DefaultParentManager
    {
        private static Texture2D _defaultParentIcon;
        
        static DefaultParentManager()
        {
            _defaultParentIcon = LibraryUtility.LoadLibraryIcon("default_parent");
            ObjectChangeEvents.changesPublished += OnObjectChanged;
            SceneHierarchyHooks.addItemsToGameObjectContextMenu += OnGameObjectContextMenu;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
        }

        private static void OnHierarchyItemGUI(int instanceID, Rect selectionRect)
        {
            GameObject instance = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (instance == null)
                return;

            if (instance.TryGetComponent<CollectionDefaultParent>(out var collectionParent))
            {
                if (SmartLibraryWindow.LastActiveWindow != null &&
                    SmartLibraryWindow.LastActiveWindow.UseDefaultParent &&
                    SmartLibraryWindow.LastActiveWindow.SelectedCollection != null &&
                    collectionParent.CollectionIds.Contains(SmartLibraryWindow.LastActiveWindow.SelectedCollection.ID))
                {
                    Rect iconRect = selectionRect;
                    iconRect.xMin = iconRect.xMax - 16;
                    GUI.DrawTexture(iconRect, _defaultParentIcon, ScaleMode.ScaleAndCrop);
                }
            }
        }

        private static void OnObjectChanged(ref ObjectChangeEventStream stream)
        {
            for (int i = 0; i < stream.length; i++)
            {
                if (stream.GetEventType(i) == ObjectChangeKind.CreateGameObjectHierarchy)
                {
                    stream.GetCreateGameObjectHierarchyEvent(i, out var data);
                    OnGameObjectCreated(data);
                }
            }
        }

        private static void OnGameObjectCreated(CreateGameObjectHierarchyEventArgs data)
        {
            var dragData = DragAndDrop.GetGenericData(LibraryConstants.ItemDragDataName);
            if (dragData is UniqueID collectionId)
            {
                var collection = LibraryDatabase.FindCollectionByID(collectionId);
                //This should never be null, but it can't hurt to make sure.
                if (collection == null)
                    return;
                
                if (SmartLibraryWindow.LastActiveWindow == null)
                    return;
                
                if (!SmartLibraryWindow.LastActiveWindow.UseDefaultParent)
                    return;
                
                var newGameObject = EditorUtility.InstanceIDToObject(data.instanceId) as GameObject;
                newGameObject.transform.parent = collection.GetCreateDefaultSceneParent().transform;
                
            }
        }
        
        private static void OnGameObjectContextMenu(GenericMenu menu, GameObject go)
        {
            if (go == null || SmartLibraryWindow.LastActiveWindow == null || SmartLibraryWindow.LastActiveWindow.SelectedCollection == null)
            {
                menu.InsertItem(12, new GUIContent("Set as Default Parent for Collection"), null);
                return;
            }
            
            if (go.TryGetComponent<CollectionDefaultParent>(out var parentLink))
            {
                if (parentLink.CollectionIds.Contains(SmartLibraryWindow.LastActiveWindow.SelectedCollection.ID))
                {
                    menu.InsertItem(12, new GUIContent("Clear as Default Parent for Collection"), SetAsDefaultParent, go);
                    return;
                }
            }
          
            menu.InsertItem(12, new GUIContent("Set as Default Parent for Collection"), SetAsDefaultParent, go);
        }
        
        private static void SetAsDefaultParent(object userData)
        {
            var target = (GameObject)userData;

            UniqueID collectionId = SmartLibraryWindow.LastActiveWindow.SelectedCollection.ID;

            Undo.IncrementCurrentGroup();
            int undoIndex = Undo.GetCurrentGroup();
            
            if (!target.TryGetComponent(out CollectionDefaultParent parentLink))
            {
                parentLink = Undo.AddComponent<CollectionDefaultParent>(target);
                parentLink.hideFlags = HideFlags.DontSaveInBuild | HideFlags.HideInInspector;
            }
            
            Undo.RecordObject(parentLink, "Edit Collection Default Parent");
            
            if (!parentLink.CollectionIds.Contains(collectionId))
            {
                var parentLinks = GameObject.FindObjectsOfType<CollectionDefaultParent>();
                foreach (var defaultParentLink in parentLinks)
                {
                    if (defaultParentLink.CollectionIds.Contains(collectionId))
                        defaultParentLink.CollectionIds.Remove(collectionId);
                }

                SmartLibraryWindow.LastActiveWindow.UseDefaultParent = true;
                parentLink.CollectionIds.Add(collectionId);
            }
            else
            {
                SmartLibraryWindow.LastActiveWindow.UseDefaultParent = false;
                parentLink.CollectionIds.Remove(collectionId);
            }
            
            // Remove the link component if it no longer links to any other collections.
            if (parentLink.CollectionIds.Count == 0)
                Undo.DestroyObjectImmediate(parentLink);
            
            Undo.CollapseUndoOperations(undoIndex);
        }
    }
}
