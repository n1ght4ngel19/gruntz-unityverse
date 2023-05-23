using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class Tool : MonoBehaviour {
    [field: SerializeField] public ToolType Type { get; set; }
    [field: SerializeField] public AnimatorOverrideController GruntAnimOverrider { get; set; }
  }
}
