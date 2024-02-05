using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Bridgez {
  public class BridgeV2 : GridObject, IToggleable {

    #region IToggleable
    // --------------------------------------------------
    // IToggleable
    // --------------------------------------------------
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public AnimancerComponent Animancer { get; set; }
    [field: SerializeField] public AnimationClip ToggleOnAnim { get; set; }
    [field: SerializeField] public AnimationClip ToggleOffAnim { get; set; }

    public void Toggle() {
      if (node.isWater) {
        ToggleOn();
      } else {
        ToggleOff();
      }
    }

    public void ToggleOn() {
      node.isWater = false;

      Animancer.Play(ToggleOnAnim);
    }

    public void ToggleOff() {
      node.isWater = true;

      Animancer.Play(ToggleOffAnim);
    }
    #endregion

  }
}
