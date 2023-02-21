using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Toy : MapObject {
    [field: SerializeField] public AnimatorOverrideController OverrideController { get; set; }
    [field: SerializeField] public ToyType Type { get; set; }
  }
}
