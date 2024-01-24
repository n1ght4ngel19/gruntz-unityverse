using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bewildered.SmartLibrary
{
    internal static class ElementExtensions
    {
        private static PropertyInfo _pseudoStatesProperty;
        private static Type _unityPseudoStatesType;
        
        private static PropertyInfo _panelOwnerObjectProperty;
        private static Type _hostViewType;
        private static PropertyInfo _actualViewProperty;

        /// <summary>
        /// Sets the element's display style.
        /// </summary>
        public static void SetDisplay(this VisualElement element, bool doDisplay)
        {
            element.style.display = doDisplay ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Returns the <see cref="ScriptableObject"/> that owns the <see cref="IPanel"/>.
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        public static ScriptableObject GetOwner(this IPanel panel)
        {
            if (_panelOwnerObjectProperty == null)
            {
                var basePanelType = typeof(VisualElement).Assembly.GetType("UnityEngine.UIElements.BaseVisualElementPanel");
                _panelOwnerObjectProperty = basePanelType.GetProperty("ownerObject");
                
                _hostViewType = typeof(EditorWindow).Assembly.GetType("UnityEditor.HostView");
                _actualViewProperty = _hostViewType.GetProperty("actualView", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            var ownerObject = _panelOwnerObjectProperty.GetValue(panel);
            
            // For panels for EditorWindows the owner is actually the GUIView (HostView or the subclass DockView).
            // So we need to get the ActualView to get the editor window that the panel is for.
            if (ownerObject.GetType() == _hostViewType || ownerObject.GetType().IsSubclassOf(_hostViewType))
                return _actualViewProperty.GetValue(ownerObject) as ScriptableObject;
            else
                return ownerObject as ScriptableObject;
        }

        public static void SetPseudoStates(this VisualElement element, PseudoStates pseudoState)
        {
            if (_pseudoStatesProperty == null)
                _pseudoStatesProperty = typeof(VisualElement).GetProperty("pseudoStates", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (_unityPseudoStatesType == null)
                _unityPseudoStatesType = _pseudoStatesProperty.PropertyType;


            _pseudoStatesProperty.SetValue(element, Enum.Parse(_unityPseudoStatesType, pseudoState.ToString()));
        }

        public static PseudoStates GetPseudoStates(this VisualElement element)
        {
            if (_pseudoStatesProperty == null)
                _pseudoStatesProperty = typeof(VisualElement).GetProperty("pseudoStates", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var value = _pseudoStatesProperty.GetValue(element);

            return (PseudoStates)Enum.Parse(typeof(PseudoStates), value.ToString());
        }
    }

    [Flags]
    internal enum PseudoStates
    {
        Active = 1 << 0,     // control is currently pressed in the case of a button
        Hover = 1 << 1,     // mouse is over control, set and removed from dispatcher automatically
        Checked = 1 << 3,     // usually associated with toggles of some kind to change visible style
        Disabled = 1 << 5,     // control will not respond to user input
        Focus = 1 << 6,     // control has the keyboard focus. This is activated deactivated by the dispatcher automatically
        Root = 1 << 7,     // set on the root visual element
    }
}
