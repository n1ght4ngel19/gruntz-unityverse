#if UNITY_EDITOR
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    internal class HierarchyDesigner_Shared_SerializableList<T>
    {
        public List<T> items;

        public HierarchyDesigner_Shared_SerializableList(List<T> items)
        {
            this.items = items;
        }
    }
}
#endif