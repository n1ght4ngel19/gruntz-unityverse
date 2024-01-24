using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Bewildered.SmartLibrary")]

namespace Bewildered.SmartLibrary
{
    [AddComponentMenu("")] // An empty string will hide the component from the menu.
    internal class CollectionDefaultParent : MonoBehaviour
    {
        [SerializeField] private List<UniqueID> _collectionIds = new List<UniqueID>();

        public List<UniqueID> CollectionIds
        {
            get { return _collectionIds; }
        }
    }
}
