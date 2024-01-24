using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bewildered.SmartLibrary.UndoSystem
{
    [Serializable]
    internal class NotifyItemsChangedOperation : UndoOperation
    {
        [SerializeField] private List<LibraryItem> _items;
        [SerializeField] private LibraryCollection _collection;
        [SerializeField] private LibraryItemsChangeType _changeType;
        
        public NotifyItemsChangedOperation(LibraryCollection collection, IEnumerable<LibraryItem> items, LibraryItemsChangeType type)
        {
            _collection = collection;
            _items = new List<LibraryItem>(items);
            _changeType = type;
        }
        
        public override void Undo()
        {
            var type = _changeType is LibraryItemsChangeType.Added
                ? LibraryItemsChangeType.Removed
                : LibraryItemsChangeType.Added;
            var args = new LibraryItemsChangedEventArgs(_items, _collection, type);
            LibraryDatabase.HandleLibraryItemsChanged(args, false);
        }

        public override void Redo()
        {
            Undo();
        }
    }
}