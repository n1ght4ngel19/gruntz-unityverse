using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public class PyramidV2 : GridObject, IAnimatable, IToggleable {
    public PyramidType type;

    // --------------------------------------------------
    // IAnimatable
    // --------------------------------------------------
    [field: SerializeField]
    public Animator Animator { get; set; }
    [field: SerializeField]
    public AnimancerComponent Animancer { get; set; }

    // --------------------------------------------------
    // IToggleable
    // --------------------------------------------------
    public void Toggle() { }
  }
}
