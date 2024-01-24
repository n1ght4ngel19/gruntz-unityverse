using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Bewildered.SmartLibrary.UI
{
    internal class CollectionsTreeView : BTreeView
    {
        internal static event System.Action<LibraryCollection, string> OnCollectionRenamed;

        private LibraryCollection _rootCollection;
        private int _nextId = 0;

        public CollectionsTreeView()
        {
            _rootCollection = LibraryDatabase.RootCollection;
            Rebuild();
            MakeItem = MakeCollectionItem;
            BindItem = BindCollectionItem;
            this.AddManipulator(new CollectionsTreeViewDragger(OnDropped));
            this.AddManipulator(new DragAutoScroller());
            this.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));
            this.Q<ScrollView>().contentContainer.RegisterCallback<KeyDownEvent>(OnKeyDown);

            RegisterCallback<AttachToPanelEvent>(evt => LibraryDatabase.HierarchyChanged += OnLibraryHierarchyChanged);
            RegisterCallback<DetachFromPanelEvent>(evt => LibraryDatabase.HierarchyChanged -= OnLibraryHierarchyChanged);

            RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == (int)MouseButton.RightMouse)
                {
                    var scrollView = this.Q<ScrollView>();
                    foreach (var element in scrollView.Children())
                    {
                        if (element.worldBound.Contains(evt.mousePosition))
                        {
                            int index = (int)element.Q<Toggle>().userData;
                            SetSelection(ItemWrappers[index].item.Id);
                            return;
                        }
                    }
                }
            });
        }
        
        public CollectionsTreeView(LibraryCollection rootCollection)
        {
            _rootCollection = rootCollection;
        }

        /// <summary>
        /// Rebuilds the tree from the <see cref="LibraryCollection"/>s in the library.
        /// </summary>
        public void Rebuild()
        {
            var items = new List<BTreeViewItem>
            {
                new AllItemsTreeViewItem()
            };

            if (_rootCollection != null)
            {
                // Create the tree items. Add the items created from the BaseCollections collections to the RootItems list.
                CreateTreeItems(_rootCollection.Subcollections, childItem => items.Add(childItem));    
            }
            
            RootItems = items;
        }
        
        /// <summary>
        /// Creates <see cref="CollectionTreeViewItem"/>s for each <see cref="LibraryCollection"/> in the specified set of collections,
        /// invoking the specified add action after creating each item. Runs recursively on all subcollections.
        /// </summary>
        /// <param name="collections">The <see cref="LibraryCollection"/>s to create <see cref="CollectionTreeViewItem"/>s for.</param>
        /// <param name="add">The action to take to add an item. For baseCollections should add item to rootItems; otherwise add as a child to the current item.</param>
        private void CreateTreeItems(IEnumerable<LibraryCollection> collections, System.Action<BTreeViewItem> add)
        {
            foreach (LibraryCollection collection in collections)
            {
                var item = new CollectionTreeViewItem(collection);
                add(item);

                CreateTreeItems(collection.Subcollections, childItem => item.AddChild(childItem));
            }
        }

        public void ScrollToCollection(LibraryCollection collection)
        {
            ScrollToItem(collection.GetInstanceID());
        }

        public void BeginRenamingCollection(LibraryCollection collection)
        {
            int id = collection.GetInstanceID();
            var element = ItemWrappers[GetItemIndex(id, true)].element;
            var treeElement = element.Q<CollectionTreeItemElement>();
            treeElement.Label.BeginRenaming();
        }

        public void SetCollectionSelection(LibraryCollection collection)
        {
            if (collection == null)
                SetSelection(-1);
            else
                SetSelection(collection.GetInstanceID());
        }

        private VisualElement MakeCollectionItem()
        {
            return new CollectionTreeItemElement();
        }

        private void BindCollectionItem(VisualElement element, BTreeViewItem item)
        {
            var collectionElement = (CollectionTreeItemElement)element;
            var libraryTreeItem = (LibraryTreeViewItem)item;
            collectionElement.BindToItem(libraryTreeItem);

            if (item is CollectionTreeViewItem collectionTreeItem)
                collectionElement.Label.RegisterValueChangedCallback(evt => OnCollectionRenamed?.Invoke(collectionTreeItem.Collection, evt.newValue));

            if (item is AllItemsTreeViewItem)
                collectionElement.name = "allItemsCollection";
            else
                collectionElement.name = item.GetType().Name;
        }


        private void OnDropped(TreeDragArgs args)
        {
            LibraryCollection parentCollection;
            if (args.parentItem is CollectionTreeViewItem parentTreeViewItem)
            {
                parentCollection = parentTreeViewItem.Collection;
            }
            else
            {
                parentCollection = LibraryDatabase.RootCollection;
                args.insertIndex -= 1;
            }

            if (args.targetItem is CollectionTreeViewItem targetTreeViewItem)
            {
                var targetCollection = targetTreeViewItem.Collection;

                if (args.dropPosition == DragAndDropPosition.OverItem)
                {
                    parentCollection.AddSubcollection(targetCollection);
                }
                else if (args.dropPosition == DragAndDropPosition.BetweenItems || args.dropPosition == DragAndDropPosition.OutsideItems)
                {
                    if (parentCollection == targetCollection.Parent)
                        parentCollection.MoveSubcollection(args.insertIndex, targetCollection.GetSiblingIndex());
                    else
                        parentCollection.InsertSubcollection(args.insertIndex, targetCollection);
                }
            }

            if (args.parentItem != null)
                ExpandItem(args.parentItem.Id);
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Delete)
            {
                DeleteSelection();
            }
        }
        
        private void OnLibraryHierarchyChanged(LibraryHierarchyChangedEventArgs evt)
        {
            int id = SelectedItem.Id;
            
            if (evt.type is HierarchyChangeType.Removed)
            {
                LibraryTreeViewItem childItem = null; // The item of the collection that has/will be removed.
                if (evt.collection.Parent == null)
                {
                    childItem = (LibraryTreeViewItem)RootItems[evt.index + 1];
                }
                else
                {
                    var parentItem = FindItem(evt.collection.GetInstanceID());
                    childItem = (LibraryTreeViewItem)parentItem[evt.index]; // will be removed along with all sub items.
                }

                // Get the new ID of the selected item if it needs to change.
                if (SelectedItem == childItem || SelectedItem.IsChildOf(childItem))
                {
                    if (evt.collection.Parent == null)
                        id = GetNextRootId();
                    else
                        id = GetNextId();
                }
            }
            
            Rebuild();
            
            SetSelection(id);
            

            int GetNextId()
            {
                BTreeViewItem parentItem = FindItem(evt.collection.GetInstanceID());

                // No siblings so we need to get the parent instead.
                if (parentItem.ChildCount == 1)
                    return parentItem.Id;
                
                
                // Set next selection to be sibling.
                
                if (evt.index == parentItem.ChildCount - 1) // is least item.
                {
                    return parentItem[evt.index - 1].Id;
                }
                else
                {
                    return parentItem[evt.index + 1].Id;
                }
            }

            int GetNextRootId()
            {
                // The root has an additional ALlItems item, so we need to adjust the index when in the root.
                int ExtraTreeItemCount = 1;
                
                if (RootItems.Count == 1 + ExtraTreeItemCount)
                    return -1;
                
                if (evt.index == RootItems.Count - 1 - ExtraTreeItemCount) // The item being removed has no siblings after it. 
                {
                    return RootItems[evt.index + 1 - ExtraTreeItemCount].Id;
                }
                else
                {
                    return RootItems[evt.index + 1 + ExtraTreeItemCount].Id;
                }
            }
        }

        private void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            bool isCollection = false;
            var scrollView = this.Q<ScrollView>();
            VisualElement selectedElement = null;
            foreach (var element in scrollView.Children())
            {
                if (element.worldBound.Contains(evt.mousePosition))
                {
                    selectedElement = element;
                    int index = (int)element.Q<Toggle>().userData;
                    isCollection = ItemWrappers[index].item == SelectedItem && SelectedItem is CollectionTreeViewItem;
                    break;
                }
            }
            
            DropdownMenuAction.Status status = isCollection ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled;
            
            evt.menu.AppendAction("Delete _delete", a => DeleteSelection(), status);
            evt.menu.AppendAction("Rename", a => selectedElement.Q<CollectionTreeItemElement>().Label.BeginRenaming(), status);
            evt.menu.AppendAction("Settings", a => Selection.activeObject = ((CollectionTreeViewItem)SelectedItem).Collection, status);
            evt.menu.AppendSeparator();
            LibraryUtility.BuildCreateCollectionMenu(evt.menu, this, isCollection);
            evt.menu.AppendSeparator();
            evt.menu.AppendAction("Properties... _&P", a => LibraryUtility.OpenPropertyEditor(((CollectionTreeViewItem)SelectedItem).Collection), status);
        }

        private void DeleteSelection()
        {
            if (SelectedItem is CollectionTreeViewItem collectionTreeItem)
            {
                LibraryCollection.DestroyCollection(collectionTreeItem.Collection);
            }
        }

        private int NextId()
        {
            return _nextId++;
        }
        
        public new class UxmlFactory : UxmlFactory<CollectionsTreeView, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlIntAttributeDescription _itemHeight = new UxmlIntAttributeDescription { name = "item-height", obsoleteNames = new[] { "itemHeight" }, defaultValue = 24 };
            private readonly UxmlEnumAttributeDescription<SelectionType> _selectionType = new UxmlEnumAttributeDescription<SelectionType> { name = "selection-type", defaultValue = SelectionType.Single };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var itemHeight = 0;
                var treeView = (BTreeView)ve;
                // Avoid setting itemHeight unless it's explicitly defined.
                // Setting itemHeight property will activate inline property mode.
                if (_itemHeight.TryGetValueFromBag(bag, cc, ref itemHeight))
                    treeView.ItemHeight = itemHeight;
                treeView.SelectionType = _selectionType.GetValueFromBag(bag, cc);
            }
        }
    }
}
