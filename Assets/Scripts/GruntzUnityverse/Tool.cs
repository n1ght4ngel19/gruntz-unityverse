using UnityEngine;

namespace GruntzUnityverse {
  public class Tool {
    [field: SerializeField] public AnimatorOverrideController OverrideController { get; set; }
    [field: SerializeField] public ToolType Type { get; set; }
  }
}
