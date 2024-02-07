using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Bridgez {
  public class BridgeV2 : GridObject, IToggleable {

    #region IToggleable
    // --------------------------------------------------
    // IToggleable
    // --------------------------------------------------
    public Animator Animator { get; set; }
    public AnimancerComponent Animancer { get; set; }
    [field: SerializeField] public AnimationClip ToggleOnAnim { get; set; }
    [field: SerializeField] public AnimationClip ToggleOffAnim { get; set; }

    public void Toggle() {
      if (node.isWater) {
        ToggleOn();
      } else {
        ToggleOff();
      }

      node.isWater = actAsWater;
    }

    public void ToggleOn() {
      actAsWater = false;
      Animancer.Play(ToggleOnAnim);
    }

    public void ToggleOff() {
      actAsWater = true;
      Animancer.Play(ToggleOffAnim);
    }
    #endregion

  }
}
