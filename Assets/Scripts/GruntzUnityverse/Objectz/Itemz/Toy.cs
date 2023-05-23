using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Toy : MonoBehaviour {
    [field: SerializeField] public ToyType Type { get; set; }
  }
}
