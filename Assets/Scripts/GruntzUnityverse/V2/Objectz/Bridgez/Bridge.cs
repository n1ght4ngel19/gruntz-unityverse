using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Bridgez {
  public class Bridge : GridObject, IToggleable {
    public bool isDeathBridge;

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
