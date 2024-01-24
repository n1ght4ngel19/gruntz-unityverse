using System;
using UnityEngine;

namespace Bewildered.SmartLibrary.UndoSystem
{
    [Serializable]
    public abstract class UndoOperation
    {
        [SerializeField] private int _undoState;

        public int UndoState
        {
            get { return _undoState; }
            set { _undoState = value; }
        }
        
        public abstract void Undo();
        public abstract void Redo();
    }
}