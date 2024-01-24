using System;
using UnityEngine;

namespace Bewildered.SmartLibrary
{
    /// <summary>
    /// Allows for referencing a <see cref="LibraryCollection"/> from another and keeping the reference between editor sessions.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="LibraryCollection"/>.</typeparam>
    [Serializable]
    public struct CollectionReference<T> where T : LibraryCollection
    {
        [SerializeField] private T _collection;
        [SerializeField] private UniqueID _id;

        /// <summary>
        /// The resolved <see cref="LibraryCollection"/>.
        /// </summary>
        public T Collection
        {
            get { return _collection; }
        }

        public CollectionReference(T collection)
        {
            _collection = collection;
            if (_collection != null)
                _id = _collection.ID;
            else
                _id = default;
        }

        /// <summary>
        /// Resolves the <see cref="LibraryCollection"/> reference. Call after all collections has been loaded.
        /// </summary>
        /// <returns>The resolved <see cref="LibraryCollection"/>.</returns>
        public T Resolve()
        {
            if (_id != default && _collection == null)
            {
                _collection = LibraryDatabase.FindCollectionByID(_id) as T;
                if (_collection == null)
                    _id = default;
            }

            return _collection;
        }
        
        public static implicit operator LibraryCollection(CollectionReference<T> reference)
        {
            return reference._collection;
        }

        public static implicit operator CollectionReference<T>(T collection)
        {
            return new CollectionReference<T>(collection);
        }
    }
}
