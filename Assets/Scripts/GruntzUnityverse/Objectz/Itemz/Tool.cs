using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Tool : MapObject {
    [field: SerializeField] public AnimatorOverrideController OverrideController { get; set; }
    [field: SerializeField] public ToolType Type { get; set; }
  }
}
