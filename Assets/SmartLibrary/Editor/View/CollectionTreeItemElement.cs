using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

namespace Bewildered.SmartLibrary.UI
{
    internal class CollectionTreeItemElement : VisualElement
    {
        public static readonly string ussClassName = "bewildered-collection-tree-item";
        public static readonly string nameUssClassName = ussClassName + "__name";
        public static readonly string iconUssClassName = ussClassName + "__icon";
        public static readonly string countUssClassName = ussClassName + "__count";
        public static readonly string settingsUssClassName = ussClassName + "__settings";

        private LibraryTreeViewItem _treeItem;

        internal RenamableLabel Label { get; }
        private Image _iconImage;
        private Label _countText;

        public LibraryCollection Collection
        {
            get 
            {
                if (_treeItem is CollectionTreeViewItem collectionTreeViewItem)
                    return collectionTreeViewItem.Collection;
                else
                    return null;
            }
        }

        public CollectionTreeItemElement()
        {
            AddToClassList(ussClassName);
            RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
            RegisterCallback<DragPerformEvent>(OnDragPerform);

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            // Icon for the collection.
            _iconImage= new Image();
            _iconImage.name = "collection-icon";
            _iconImage.AddToClassList(iconUssClassName);
            Add(_iconImage);

            // Name label for collection.
            Label = new RenamableLabel();
            Label.name = "collection-name";
            Label.AddToClassList(nameUssClassName);
            Label.RegisterValueChangedCallback(OnLabelValueChanged);
            Add(Label);

            _countText = new Label();
            _countText.name = "item-count";
            _countText.AddToClassList(countUssClassName);
            Add(_countText);

            var settingsElement = new Image();
            settingsElement.name = "settings";
            settingsElement.AddToClassList(settingsUssClassName);
            settingsElement.AddToClassList(LibraryConstants.IconUssClassName);
            settingsElement.image = LibraryConstants.CollectionSettingsIcon;
            settingsElement.RegisterCallback<MouseDownEvent>(OnSettingsSelected);
            Add(settingsElement);
        }

        public void BindToItem(LibraryTreeViewItem treeViewItem)
        {
            _treeItem = treeViewItem;
            if (_treeItem == null)
                return;
            
            // We don't notify a change because when we build the treeview, we register a valueChanged callback
            // that will refresh the treeview which would cause a infinite loop where on bind, we tell the treeview to rebind.
            ((INotifyValueChanged<string>)Label).SetValueWithoutNotify(_treeItem.Name);
            
            if (_treeItem is CollectionTreeViewItem collectionItem)
                _iconImage.image = collectionItem.Collection.Icon;
            else
                _iconImage.image = LibraryConstants.DefaultCollectionIcon;
            _countText.text = $"{_treeItem.Count}";
            Label.canBeRenamed = _treeItem is CollectionTreeViewItem;
        }

        private void OnSettingsSelected(MouseDownEvent evt)
        {
            if (Collection != null)
                Selection.activeObject = Collection;
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            LibraryDatabase.ItemsChanged += OnLibraryItemsChanged;
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            LibraryDatabase.ItemsChanged -= OnLibraryItemsChanged;
        }

        private void OnLibraryItemsChanged(LibraryItemsChangedEventArgs args)
        {
            if (args.collection == Collection || _treeItem is AllItemsTreeViewItem)
                _countText.text = $"{_treeItem.Count}";
        }

        private void OnLabelValueChanged(ChangeEvent<string> evt)
        {
            if (string.IsNullOrEmpty(evt.newValue))
            {
                evt.PreventDefault();
                evt.StopPropagation();
            }
            else if (_treeItem != null)
            {
                _treeItem.Name = evt.newValue;
            }
        }
        
        private void OnDragUpdated(DragUpdatedEvent evt)
        {
            if (DragAndDrop.objectReferences.Length <= 0 || !(Collection is ILibrarySet))
                return;
            
            var data =  DragAndDrop.GetGenericData(LibraryConstants.ItemDragDataName);
            if (data is UniqueID fromCollectionId)
            {
                if (fromCollectionId != UniqueID.Empty)
                {
                    // We reject the drag if it is the same collection the assets are from since non can added.
                    if (fromCollectionId == Collection.ID)
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    else if (!evt.ctrlKey)
                        DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    
                    return;
                }
            }

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }

        private void OnDragPerform(DragPerformEvent evt)
        {
            if (DragAndDrop.objectReferences.Length <= 0 || !(Collection is ILibrarySet librarySet))
                return;

            bool isFromCollection = false;
            var data = DragAndDrop.GetGenericData(LibraryConstants.ItemDragDataName);
            
            if (data is UniqueID fromCollectionId)
            {
                isFromCollection = true;
                // If we are dropping and dragging from the same collection, no assets can be added so we exit out.
                if (fromCollectionId == Collection.ID)
                    return;
            }
            else
            {
                // We set to -1000 because -1 is the id of the root collection, so we just need it to not be a valid id.
                fromCollectionId = UniqueID.Empty;
            }
            
            DragAndDrop.AcceptDrag();

            // If CTRL is not held, then we try to move the assets from their current collection if it supports it,
            // and if the item can be added to the this collection.
            if (!evt.ctrlKey)
            {
                // Check if we are dragging objects from another collection.
                if (isFromCollection) 
                {
                    // Check if the collection we are dragging from implements the ILibrarySet interface
                    // and thus can have items removed.
                    var fromCollection = LibraryDatabase.FindCollectionByID(fromCollectionId);
                    if (fromCollection is ILibrarySet fromLibrarySet && fromCollection.ID != Collection.ID)
                    {
                        // We add each item to the collection individually because if they are not added,
                        // then we don't remove them from their current collection.
                        var undoGroup = Undo.GetCurrentGroup();
                        foreach (var obj in DragAndDrop.objectReferences)
                        {
                            if (librarySet.Add(obj))
                                fromLibrarySet.Remove(obj);
                        }

                        
                        // Clear the selection when moving items.
                        if (panel.GetOwner() is SmartLibraryWindow libraryWindow)
                        {
                            libraryWindow.ClearItemSelection();
                        }

                        if (Undo.GetCurrentGroup() != undoGroup)
                            Undo.CollapseUndoOperations(undoGroup);

                        return;
                    }
                } 
            }
                
            // Default if CTRL is not held, or the assets are not from another collection,
            // or the collection does not support remove items.
            librarySet.UnionWith(DragAndDrop.objectReferences);
        }
    } 
}
