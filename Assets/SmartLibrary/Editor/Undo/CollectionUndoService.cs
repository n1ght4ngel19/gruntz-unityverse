using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Bewildered.SmartLibrary.UndoSystem
{
    // Implementation explanation: We have a CollectionsUndoState which stores a int that is
    // incremented every time a collection undo is registered. And also registered an Unity undo operation.
    // That way, when a collection undo or redo is performed, the int will change with it.
    // We store a copy of the int value on the CollectionUndoService that updates every time
    // we register a collection undo or a undoRedo is performed.
    
    // When a undoRedo is performed, we can compare the stored int to
    // the one in CollectionsUndoState. If CollectionsUndoState is greater it means we know
    // a redo was performed and if it is less it was a undo. If they are the same it means it was not a collection undo/redo.
    
    /// <summary>
    /// Handles special behaviour for undo/redo that is not otherwise supported by Unity.
    /// Such as saving to file and invoking events.
    /// </summary>
    internal class CollectionUndoService : ScriptableSingleton<CollectionUndoService>
    {
        [SerializeField] private int _undoState;
        [SerializeField] private UStackRef<UndoOperation> _undoOperations = new UStackRef<UndoOperation>();
        [SerializeField] private UStackRef<UndoOperation> _redoOperations = new UStackRef<UndoOperation>();

        public int UndoState
        {
            get { return _undoState; }
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            Undo.undoRedoPerformed += instance.OnUndoRedoPerformed;
        }

        private void OnUndoRedoPerformed()
        {
            if (_undoState == CollectionsUndoState.instance.State)
                return;
            
            // Undo action.
            if (_undoState > CollectionsUndoState.instance.State)
            {
                // Multiple collection changes can be combined in to a single undo operation
                // like when removing a collection, so we need to support undoing all of them as well.
                while (_undoOperations.Count > 0 && _undoOperations.Peek().UndoState !=
                    CollectionsUndoState.instance.State)
                {
                    UndoCollectionChange();
                }
            }

            // Redo action.
            if (_undoState < CollectionsUndoState.instance.State)
            {
                // Multiple collection changes can be combined in to a single undo operation
                // like when removing a collection, so we need to support undoing all of them as well.
                while (_redoOperations.Count > 0 && _redoOperations.Peek().UndoState !=
                    CollectionsUndoState.instance.State)
                {
                    RedoCollectionChange();
                }
            }
                
            _undoState = CollectionsUndoState.instance.State;
        }

        /// <summary>
        /// Registers that a collection is about to be modified.
        /// </summary>
        internal static void RegisterUndo()
        {
            Undo.RegisterCompleteObjectUndo(CollectionsUndoState.instance, "Collection undo state modified");
            CollectionsUndoState.instance.State++;
            instance._undoState = CollectionsUndoState.instance.State;
            instance._redoOperations.Clear();
        }

        internal static void RegisterOperation(UndoOperation operation)
        {
            operation.UndoState = CollectionsUndoState.instance.State;
            instance._undoOperations.Push(operation);
        }

        private void UndoCollectionChange()
        {
            var undoOperation = _undoOperations.Pop();
            undoOperation.Undo();
            undoOperation.UndoState = CollectionsUndoState.instance.State;
            _redoOperations.Push(undoOperation);
        }

        private void RedoCollectionChange()
        {
            var undoOperation = _redoOperations.Pop();
            undoOperation.Redo();
            undoOperation.UndoState = CollectionsUndoState.instance.State;
            _undoOperations.Push(undoOperation);
        }
        
    }
}
