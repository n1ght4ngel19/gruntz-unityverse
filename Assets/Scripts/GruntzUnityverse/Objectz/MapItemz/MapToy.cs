using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.MapItemz {
  public class MapToy : MapItem {
    [field: SerializeField] public AnimatorOverrideController OverrideController { get; set; }
    [field: SerializeField] public ToyType Type { get; set; }
  }
}
