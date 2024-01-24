// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace Bewildered.SmartLibrary.UI
// {
//     public enum HierarchyChangeType
//     {
//         Add,
//         Remove,
//         Move
//     }
//     
//     public class HierarchyChangedEvent : EventBase<HierarchyChangedEvent>
//     {
//         private static HashSet<int> _initializedPanels = new HashSet<int>();
//         private static EventInfo _hierarchyChangedInfo;
//
//         public VisualElement Child { get; private set; }
//
//         [InitializeOnLoadMethod, RuntimeInitializeOnLoadMethod]
//         private static void Setup()
//         {
//             _initializedPanels.Clear();
//             Type basePanelType = typeof(IPanel).Assembly.GetType("UnityEngine.UIElements.BaseVisualElementPanel");
//             _hierarchyChangedInfo = basePanelType.GetEvent("hierarchyChanged", TypeAccessor.Flags);
//             
//             // Get repaint.
//             Type panelType = typeof(IPanel).Assembly.GetType("UnityEngine.UIElements.Panel");
//             EventInfo beforeAnyRepaintInfo = panelType.GetEvent("beforeAnyRepaint", TypeAccessor.Flags);
//
//             var delegateType = beforeAnyRepaintInfo.EventHandlerType;
//
//             Action<IPanel> onPanelRepaint = OnPanelRepaint;
//
//             var instance = Delegate.CreateDelegate(delegateType, onPanelRepaint, onPanelRepaint.GetType().GetMethod("Invoke"));
//             
//             beforeAnyRepaintInfo.GetAddMethod(true).Invoke(null, new[] { instance });
//         }
//
//         private static void OnPanelRepaint(IPanel panel)
//         {
//             // has to be teh hashcode instead of the panel as otherwise you get errors about native collections not being disposed.
//             if (_initializedPanels.Add(panel.GetHashCode()))
//             {
//                 var delegateType = _hierarchyChangedInfo.EventHandlerType;
//                 Action<VisualElement, Enum> onPanelHierarchyChanged = OnPanelHierarchyChanged;
//                 
//                 var instance = Delegate.CreateDelegate(delegateType, onPanelHierarchyChanged, onPanelHierarchyChanged.GetType().GetMethod("Invoke"));
//                 _hierarchyChangedInfo.GetAddMethod(true).Invoke(panel, new[] { instance });
//             }
//         }
//
//         // Element: The element that was added/removed.
//         private static void OnPanelHierarchyChanged(VisualElement element, Enum changeTypeEnum)
//         {
//             //Debug.Log($"Pre-Changed: {element}, Type: {changeTypeEnum}");
//
//             using (var pooledEvent = GetPooled())
//             {
//                 pooledEvent.Child = element;
//                 
//                 pooledEvent.target = element.parent;
//                 //pooledEvent.bubbles = true;
//                 element.parent.SendEvent(pooledEvent);
//             }
//             
//             //HierarchyChangeType changeType = (HierarchyChangeType)Enum.Parse(typeof(HierarchyChangeType), changeTypeEnum.ToString());
//
//             //Debug.Log($"Changed: {element}, Type: {changeType}");
//         }
//     }
// }
