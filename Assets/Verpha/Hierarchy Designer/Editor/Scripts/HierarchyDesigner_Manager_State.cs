#if UNITY_EDITOR
using UnityEditor;

namespace Verpha.HierarchyDesigner
{
    internal class HierarchyDesigner_Manager_State : ScriptableSingleton<HierarchyDesigner_Manager_State>
    {
        #region Properties
        public bool isLeftPanelCollapsed = false;
        public bool utilitiesFoldout = false;
        public bool configurationsFoldout = false;
        public HierarchyDesigner_Window_Main.CurrentWindow currentWindow = HierarchyDesigner_Window_Main.CurrentWindow.Home;
        #endregion

        /*
        #region Methods
       
        public new void Save(bool persistBetweenSessions = false)
        {
            if (persistBetweenSessions)
            {
                base.Save(true);
            }
        }
        #endregion
        */
    }
}
#endif