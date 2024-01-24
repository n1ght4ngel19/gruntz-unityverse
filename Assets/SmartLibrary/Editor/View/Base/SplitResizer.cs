using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    public class SplitResizer : MouseManipulator
    {
        private Vector2 _start;
        private bool _active;
        private MultiSplitView _splitView;
        private VisualElement _pane;

        public SplitResizer(MultiSplitView splitView, VisualElement pane)
        {
            _splitView = splitView;
            _pane = pane;
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void ApplyDragDelta(float delta)
        {
            float previousLength = _splitView.IsVertical ? _pane.resolvedStyle.height : _pane.resolvedStyle.width;

            float newLength = previousLength + delta;

            if (_splitView.IsVertical)
            {
                newLength = Mathf.Max(newLength, _pane.style.minHeight.value.value, 10);
                _pane.style.height = newLength;
            }
            else
            {
                newLength = Mathf.Max(newLength, _pane.style.minWidth.value.value, 10);
                _pane.style.width = newLength;   
            }
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            // If the resizer is already active then don't want to start another.
            if (_active)
            {
                // Something has gone wrong as two mouse down events should not be called while active is true.
                evt.StopImmediatePropagation();
                return;
            }

            if (!CanStartManipulation(evt))
                return;

            _start = evt.localMousePosition;

            _active = true;
            target.CaptureMouse();
            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (!_active || !target.HasMouseCapture())
                return;
            
            Vector2 diff = evt.localMousePosition - _start;
            float directionDiff = _splitView.IsVertical ? diff.y : diff.x;

            float delta = 1 * directionDiff;
            
            ApplyDragDelta(delta);
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (!_active || !target.HasMouseCapture() || !CanStopManipulation(evt))
                return;

            _active = false;
            target.ReleaseMouse();
            evt.StopPropagation();
        }
    }
}
