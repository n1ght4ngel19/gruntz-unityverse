using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Bewildered.SmartLibrary.UI
{
    public enum PaneOrientation { Vertical, Horizontal }
    
    public class MultiSplitView : VisualElement
    {
        public static readonly string UssClassName = "bewildered-multi-split-view";
        
        private static readonly string _dividersContainerUssClassName = UssClassName + "__dividers-container";
        private static readonly string _panesContainerUssClassName = UssClassName + "__panes-container";
        
        private static readonly string _dividerUssClassName = UssClassName + "__divider";
        private static readonly string _dividerAnchorUssClassName = UssClassName + "__divider-anchor";
        private static readonly string _dividerAnchorVerticalUssClassName = UssClassName + "__divider-anchor-vertical";
        private static readonly string _dividerAnchorHorizontalUssClassName = UssClassName + "__divider-anchor-horizontal";

        private VisualElement _panesContainer;
        private VisualElement _dividersContainer;
        private PaneOrientation _paneOrientation;

        public new class UxmlFactory : UxmlFactory<MultiSplitView, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private UxmlEnumAttributeDescription<PaneOrientation> _orientation = new UxmlEnumAttributeDescription<PaneOrientation> { name = "orientation", defaultValue = PaneOrientation.Horizontal };
            
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var orientation = _orientation.GetValueFromBag(bag, cc);

                ((MultiSplitView) ve).IsVertical = orientation == PaneOrientation.Vertical;
                ((MultiSplitView) ve).Refresh();
            }
        }

        public bool IsVertical
        {
            get { return _paneOrientation == PaneOrientation.Vertical; }
            set
            {
                _paneOrientation = value ? PaneOrientation.Vertical : PaneOrientation.Horizontal;
                UpdateLayoutDirection();
            }
        }

        public bool IsReversed
        {
            get
            {
                return resolvedStyle.flexDirection == FlexDirection.RowReverse ||
                       resolvedStyle.flexDirection == FlexDirection.ColumnReverse;
            }
        }
        
        public override VisualElement contentContainer
        {
            get { return _panesContainer; }
        }

        public MultiSplitView()
        {
            AddToClassList(UssClassName);
            styleSheets.Add(LibraryUtility.LoadStyleSheet("MultiSplitView"));
            
            _panesContainer = new VisualElement();
            _panesContainer.AddToClassList(_panesContainerUssClassName);
            hierarchy.Add(_panesContainer);

            _dividersContainer = new VisualElement();
            _dividersContainer.pickingMode = PickingMode.Ignore;
            _dividersContainer.AddToClassList(_dividersContainerUssClassName);
            hierarchy.Add(_dividersContainer);
        }

        
        public void Refresh()
        {
            _dividersContainer.Clear();

            for (int i = 0; i < _panesContainer.childCount; i++)
            {
                if (i > 0)
                {
                    _dividersContainer.Add(CreateDivider(_panesContainer[i - 1]));
                }

                _panesContainer[i].style.flexGrow = i == _panesContainer.childCount - 1 ? 1 : 0;
                
                // Some panes may not have the event if they were added via Add(..),
                // so we remove them before adding it again to all of them.
                _panesContainer[i].UnregisterCallback<GeometryChangedEvent>(OnPaneSizeChange);
                _panesContainer[i].RegisterCallback<GeometryChangedEvent>(OnPaneSizeChange);
            }
        }

        public void SetPaneDisplayed(int paneIndex, bool display)
        {
            _panesContainer[paneIndex].SetDisplay(display);
            if (paneIndex < _dividersContainer.childCount)
                _dividersContainer[paneIndex].SetDisplay(display);
        }
        
        public void AddPane(VisualElement pane)
        {
            VisualElement previousLastPane = _panesContainer.childCount > 0 ? _panesContainer[_panesContainer.childCount - 1] : null;
            pane.style.flexGrow = 1;

            _panesContainer.Add(pane);
            if (_panesContainer.childCount > 1)
            {
                _dividersContainer.Add(CreateDivider(previousLastPane));
                if (!IsReversed)
                    previousLastPane.style.flexGrow = 0;
            }
            pane.RegisterCallback<GeometryChangedEvent>(OnPaneSizeChange);
        }

        public void RemovePane(VisualElement element)
        {
            
        }

        private VisualElement CreateDivider(VisualElement pane)
        {
            var dividerAnchor = new VisualElement();
            dividerAnchor.AddToClassList(_dividerAnchorUssClassName);

            var divider = new VisualElement();
            divider.AddToClassList(_dividerUssClassName);
            
            dividerAnchor.Add(divider);
            
            UpdateDividerLayoutDirection(dividerAnchor);
            
            if (IsVertical)
            {
                dividerAnchor.style.top = IsReversed ? pane.worldBound.yMin : pane.worldBound.yMax;
            }
            else
            {
                dividerAnchor.style.left = IsReversed ? pane.worldBound.xMin : pane.worldBound.xMax;
            }
            
            dividerAnchor.AddManipulator(new SplitResizer(this, pane));

            return dividerAnchor;
        }

        private void UpdateDividerLayoutDirection(VisualElement divider)
        {
            divider.EnableInClassList(_dividerAnchorVerticalUssClassName, IsVertical);
            divider.EnableInClassList(_dividerAnchorHorizontalUssClassName, !IsVertical);

            int dividerIndex = _dividersContainer.IndexOf(divider);
            
            // The method is called when a divider is created and so may not be added yet.
            if (dividerIndex == -1)
                return;
            
            VisualElement pane = _panesContainer[dividerIndex];
            
            if (IsVertical)
            {
                divider.style.top = IsReversed ? pane.worldBound.yMin : pane.worldBound.yMax;
                divider.style.left = 0;
            }
            else
            {
                divider.style.left = IsReversed ? pane.worldBound.xMin : pane.worldBound.xMax;
                divider.style.top = 0;
            }
        }

        private void OnPaneSizeChange(GeometryChangedEvent evt)
        {
            var pane = (VisualElement) evt.target;
            int paneIndex = _panesContainer.IndexOf(pane);
            
            if (paneIndex < _dividersContainer.childCount)
            {
                VisualElement divider = _dividersContainer[paneIndex];
                if (IsVertical)
                {
                    divider.style.top = IsReversed ? evt.newRect.yMin : evt.newRect.yMax;
                }
                else
                {
                    divider.style.left = IsReversed ? evt.newRect.xMin : evt.newRect.xMax;   
                }
            }
        }

        private void UpdateLayoutDirection()
        {
            if ((IsVertical && _panesContainer.style.flexDirection == FlexDirection.Column) ||
                (!IsVertical && _panesContainer.style.flexDirection == FlexDirection.Row))
                return;

            _panesContainer.style.flexDirection = IsVertical ? FlexDirection.Column : FlexDirection.Row;
            foreach (VisualElement pane in _panesContainer.Children())
            {
                if (IsVertical)
                {
                    pane.style.height = pane.style.width;
                    pane.style.width = new StyleLength(StyleKeyword.Auto);
                }
                else
                {
                    pane.style.width = pane.style.height;
                    pane.style.height = new StyleLength(StyleKeyword.Auto);
                }
            }

            foreach (VisualElement divider in _dividersContainer.Children())
            {
                UpdateDividerLayoutDirection(divider);
            }
        }
    }
}
