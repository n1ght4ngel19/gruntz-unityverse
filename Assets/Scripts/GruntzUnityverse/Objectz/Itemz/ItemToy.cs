using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class ItemToy : MonoBehaviour {
    [field: SerializeField] public ToyType Type { get; set; }
  }
}
