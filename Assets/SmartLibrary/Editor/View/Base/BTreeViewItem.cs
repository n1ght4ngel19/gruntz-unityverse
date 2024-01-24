using System.Collections;
using System.Collections.Generic;

namespace Bewildered.SmartLibrary.UI
{
    internal class BTreeViewItem : IReadOnlyList<BTreeViewItem>
    {
        private BTreeViewItem _parent;
        private List<BTreeViewItem> _children;

        public int Id { get; }

        public BTreeViewItem Parent
        {
            get { return _parent; }
        }

        public IEnumerable<BTreeViewItem> Children
        {
            get { return _children; }
        }

        public bool HasChildren
        {
            get { return _children != null && _children.Count > 0; }
        }

        public int ChildCount
        {
            get { return _children != null ? _children.Count : 0; }
        }
        
        int IReadOnlyCollection<BTreeViewItem>.Count
        {
            get { return _children.Count; }
        }
        

        public BTreeViewItem this[int index]
        {
            get { return _children[index]; }
        }

        public BTreeViewItem(int id)
        {
            Id = id;
        }

        public void AddChild(BTreeViewItem child)
        {
            if (child == null)
                return;

            if (_children == null)
                _children = new List<BTreeViewItem>();

            _children.Add(child);
            child._parent = this;
        }

        public void AddChildren(IEnumerable<BTreeViewItem> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        public void RemoveChild(BTreeViewItem child)
        {
            if (child == null)
                return;

            if (_children == null)
                return;

            _children.Remove(child);
        }

        public int GetSiblingIndex()
        {
            if (_parent == null)
                return -1;

            for (int i = 0; i < _parent._children.Count; i++)
            {
                if (_parent._children[i] == this)
                    return i;
            }

            return -1;
        }

        public bool IsChildOf(BTreeViewItem otherParent)
        {
            BTreeViewItem parent = this;
            while (parent != null)
            {
                if (parent == otherParent)
                    return true;
                parent = parent._parent;
            }

            return false;
        }

        public IEnumerator<BTreeViewItem> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
