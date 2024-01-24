using System;
using UnityEditor;
using UnityEngine;

namespace Bewildered.SmartLibrary.UndoSystem
{
    [Serializable]
    internal class NotifyCollectionAddRemoveOperation : UndoOperation
    {
        [SerializeField] private LibraryCollection _parentCollection;
        [SerializeField] private LibraryCollection _subcollection;
        [SerializeField] private int _previousIndex;
   
        [SerializeField] private bool _added;

        public NotifyCollectionAddRemoveOperation(LibraryCollection collection, LibraryCollection subcollection, int previousIndex, HierarchyChangeType type)
        {
            _parentCollection = collection;
            _subcollection = subcollection;
            _previousIndex = previousIndex;
            _added = type is HierarchyChangeType.Added;
        }
        
        public override void Undo()
        {
            var type = _added ? HierarchyChangeType.Removed : HierarchyChangeType.Added;
            
            var args = new LibraryHierarchyChangedEventArgs(_subcollection, _parentCollection, _previousIndex, type);
            
            _added = !_added;
            
            LibraryDatabase.HandleLibraryHierarchyChanged(args, false);
        }

        public override void Redo()
        {
            Undo();
        }
    }
}