using System;
using System.Collections.Generic;
using Bewildered.SmartLibrary.UI;

namespace Bewildered.SmartLibrary
{
    /// <summary>
    /// Comparer for <see cref="FilterEntry{T}"/> of type <see cref="LibraryItem"/>.
    /// </summary>
    internal class LibraryItemEntryComparer : IComparer<FilterEntry<LibraryItem>>
    {
        private readonly ItemSortOrder _sortOrder;

        public LibraryItemEntryComparer(ItemSortOrder sortOrder)
        {
            _sortOrder = sortOrder;
        }

        public int Compare(FilterEntry<LibraryItem> x, FilterEntry<LibraryItem> y)
        {
            // Sorts by score then by alphabetical.
            int scoreResult = y.Result.score.CompareTo(x.Result.score);
            if (scoreResult != 0)
                return scoreResult;

            int nameResult = _sortOrder != ItemSortOrder.NameDescending ?
                string.CompareOrdinal(x.Result.itemName, y.Result.itemName) :
                string.CompareOrdinal(y.Result.itemName, x.Result.itemName);

            int typeResult = _sortOrder != ItemSortOrder.TypeDescending ?
                string.CompareOrdinal(x.Item.Type.Name, y.Item.Type.Name) :
                string.CompareOrdinal(y.Item.Type.Name, x.Item.Type.Name);

            int dateResult = _sortOrder != ItemSortOrder.DateModifiedDescending
                ? DateTime.Compare(x.Item.LastModifiedDate, y.Item.LastModifiedDate)
                : DateTime.Compare(y.Item.LastModifiedDate, x.Item.LastModifiedDate);

            switch (_sortOrder)
            {
                case ItemSortOrder.NameAscending:
                case ItemSortOrder.NameDescending:
                {
                    if (nameResult != 0)
                        return nameResult;
                    else
                        return typeResult;
                }
                case ItemSortOrder.TypeAscending:
                case ItemSortOrder.TypeDescending:
                {
                    if (typeResult != 0)
                        return typeResult;
                    else
                        return nameResult;
                }
                case ItemSortOrder.DateModifiedAscending:
                case ItemSortOrder.DateModifiedDescending:
                {
                    if (dateResult != 0)
                        return dateResult;
                    else
                        return nameResult;
                }
                default:
                    return nameResult;
            }
        }
    }
}