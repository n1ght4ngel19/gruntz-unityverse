#if UNITY_EDITOR
using System;

namespace Verpha.HierarchyDesigner
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class HierarchyDesigner_Shared_Attributes : Attribute
    {
        public HierarchyDesigner_Attribute_Tools Category { get; private set; }

        public HierarchyDesigner_Shared_Attributes(HierarchyDesigner_Attribute_Tools category)
        {
            Category = category;
        }
    }

    internal enum HierarchyDesigner_Attribute_Tools
    {
        Activate,
        Count,
        Lock,
        Rename,
        Select,
        Sort
    }
}
#endif