using UnityEngine;

namespace GruntzUnityverse {
  public class Toy {
    [field: SerializeField] public AnimatorOverrideController OverrideController { get; set; }
    [field: SerializeField] public ToyType Type { get; set; }
  }
}
