using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bewildered.SmartLibrary
{
    [Serializable]
    internal class RootLibraryCollection : LibraryCollection, ISerializationCallbackReceiver
    {
        [Serializable]
        private struct CollectionsContainingItemPair
        {
            public LibraryItem item;
            public List<UniqueID> collectionIds;
        }
        
        [SerializeField] private List<int> _allIds = new List<int>();
        //[SerializeField] private List<SerializedData> _allCollections = new List<SerializedData>();
        [SerializeField] private List<LibraryCollection> _allCollections = new List<LibraryCollection>();
        [SerializeField] private List<CollectionsContainingItemPair> _itemCollectionIdPairs = new List<CollectionsContainingItemPair>();

        public Dictionary<LibraryItem, List<UniqueID>> CollectionsContainingItems
        {
            get;
        } = new Dictionary<LibraryItem, List<UniqueID>>();

        public Dictionary<string, LibraryItem> AssetGuidToItem { get; } =
            new Dictionary<string, LibraryItem>();

        private Dictionary<int, LibraryCollection> _legacyIdMap = new Dictionary<int, LibraryCollection>();
        public Dictionary<int, LibraryCollection> LegacyIDToCollectionMap
        {
            get
            {
                if (_legacyIdMap.Count == 0)
                {
                    for (int i = 0; i < _allIds.Count; i++)
                    {
                        if (_allCollections[i] == null)
                            continue;
                
                        _legacyIdMap.Add(_allIds[i], _allCollections[i]);
                    }
                }

                return _legacyIdMap;
            }
        }

        public RootLibraryCollection()
        {
            Root = this;
        }

        public override void UpdateItems() { }

        internal void AddCollectionToTree(LibraryCollection collection)
        {
            int undoGroup = PrepareChange();
            collection.Root = this;
            collection.SubcollectionsInternal.ForEach(AddCollectionToTree);
            
            SessionData.CacheCollectionData(collection);
            
            // Register all items.
            foreach (var item in collection)
            {
                RegisterWithCollection(item, collection.ID);
            }
            
            FinishChange(undoGroup);
        }

        internal void RemoveCollectionFromTree(LibraryCollection collection)
        {
            PrepareChange();
            // Remove all the specified collections subcollections from the tree as well.
            collection.SubcollectionsInternal.ForEach(RemoveCollectionFromTree);

            SessionData.RemoveCollectionData(collection);
            
            // Unregister all items.
            foreach (var item in collection)
            {
                UnregisterWithCollection(item, collection.ID);
            }
            
            FinishChange();
        }
        
        internal void RegisterWithCollection(LibraryItem item, UniqueID collectionId)
        {
            if (CollectionsContainingItems.TryGetValue(item, out List<UniqueID> ids))
                ids.Add(collectionId);
            else
                CollectionsContainingItems.Add(item, new List<UniqueID>() { collectionId });
        }

        internal void UnregisterWithCollection(LibraryItem item, UniqueID collectionId)
        {
            if (CollectionsContainingItems.TryGetValue(item, out List<UniqueID> ids))
            {
                ids.Remove(collectionId);
                if (ids.Count == 0)
                {
                    CollectionsContainingItems.Remove(item);
                }
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // // Clear serialized data.
            // _allIds.Clear();
            // _allCollections.Clear();
            //
            // // Serialize the data.
            // foreach (var pair in SessionData.instance.IDToCollectionMap)
            // {
            //     _allIds.Add(pair.Key);
            //     _allCollections.Add(pair.Value);
            // }
            _itemCollectionIdPairs.Clear();
            foreach (var pair in CollectionsContainingItems)
            {
                _itemCollectionIdPairs.Add(new CollectionsContainingItemPair() { item = pair.Key, collectionIds =  pair.Value});
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Deserialize and add the serialized data to the dictionary so it can be used to construct the tree.
            // for (int i = 0; i < _allIds.Count; i++)
            // {
            //     if (_allCollections[i] == null)
            //         continue;
            //
            //     SessionData.instance.IDToCollectionMap.Add(_allIds[i], _allCollections[i]);
            // }
            
            CollectionsContainingItems.Clear();
            AssetGuidToItem.Clear();
            foreach (var itemPair in _itemCollectionIdPairs)
            {
                CollectionsContainingItems[itemPair.item] = itemPair.collectionIds;
                AssetGuidToItem[itemPair.item.GUID] = itemPair.item;
            }
        }
    }
}
