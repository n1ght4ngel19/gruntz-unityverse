using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  // Todo: Inherit from Item
  public class Toy : MonoBehaviour {
    [field: SerializeField] public ToyName Name { get; set; }
  }
}
