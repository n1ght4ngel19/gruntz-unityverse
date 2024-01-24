using System;
using UnityEditor;
using UnityEngine;

namespace Bewildered.SmartLibrary.UndoSystem
{
    [Serializable]
    internal class NotifyCollectionMovedOperation : UndoOperation
    {
        [SerializeField] private LibraryCollection _parentCollection;
        [SerializeField] private LibraryCollection _subcollection;
        [SerializeField] private int _previousIndex;

        public NotifyCollectionMovedOperation(LibraryCollection parentCollection, LibraryCollection subcollection, int previousIndex)
        {
            _parentCollection = parentCollection;
            _subcollection = subcollection;
            _previousIndex = previousIndex;
        }
        
        public override void Undo()
        {
            // On undoing destroying a collection, the destroyed collections will be 'null',
            // so we need to get the instances from their instance ids as a way to 'force load' them.
            _parentCollection = (LibraryCollection)EditorUtility.InstanceIDToObject(_parentCollection.GetInstanceID());
            _subcollection = (LibraryCollection)EditorUtility.InstanceIDToObject(_subcollection.GetInstanceID());
            var args = new LibraryHierarchyChangedEventArgs(_subcollection, _parentCollection, _previousIndex, HierarchyChangeType.Moved);

            _previousIndex = _subcollection.GetSiblingIndex();
            
            LibraryDatabase.HandleLibraryHierarchyChanged(args, false);
        }

        public override void Redo()
        {
            Undo();
        }
    }
}