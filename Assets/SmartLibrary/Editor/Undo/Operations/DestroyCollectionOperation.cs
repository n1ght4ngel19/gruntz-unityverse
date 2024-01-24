using System;
using System.IO;
using UnityEngine;

namespace Bewildered.SmartLibrary.UndoSystem
{
    [Serializable]
    public class DestroyCollectionOperation : UndoOperation
    {
        [SerializeField] private string _collectionPath;
        
        public DestroyCollectionOperation(LibraryCollection collection)
        {
            _collectionPath = LibraryUtility.GetCollectionPath(collection);
        }
        
        public override void Undo()
        {
            UniqueID id = LibraryUtility.GetCollectionIDFromPath(_collectionPath);
            LibraryCollection collection = LibraryDatabase.FindCollectionByID(id);
            LibraryUtility.SaveCollection(collection);
        }

        public override void Redo()
        {
            File.Delete(_collectionPath);  
        }
    }
}