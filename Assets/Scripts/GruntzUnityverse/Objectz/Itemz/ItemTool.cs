using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Itemz {
  public class ItemTool : MonoBehaviour {
    public ToolType Type { get; set; }
    [field: SerializeField] public AnimatorOverrideController GruntAnimOverrider { get; set; }
  }
}
