#if UNITY_EDITOR
using UnityEditor;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    internal static class HierarchyDesigner_Manager_Initializer
    {
        static HierarchyDesigner_Manager_Initializer()
        {
            HierarchyDesigner_Manager_Editor.LoadCache();
            HierarchyDesigner_Configurable_GeneralSettings.Initialize();
            HierarchyDesigner_Configurable_AdvancedSettings.Initialize();
            HierarchyDesigner_Configurable_ShortcutsSettings.Initialize();
            HierarchyDesigner_Manager_Editor.Initialize();
            HierarchyDesigner_Configurable_DesignSettings.Initialize();
            HierarchyDesigner_Configurable_Folder.Initialize();
            HierarchyDesigner_Configurable_Separator.Initialize();
            HierarchyDesigner_Manager_GameObject.Initialize();
            HierarchyDesigner_Configurable_Presets.Initialize();
            HierarchyDesigner_Window_Main.Initialize();
        }
    }
}
#endif