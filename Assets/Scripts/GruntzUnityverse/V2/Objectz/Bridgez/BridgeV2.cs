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
      if (isBlocking) {
        ToggleOn();
      } else {
        ToggleOff();
      }
    }

    public void ToggleOn() {
      Debug.Log("Bridge raised!");
      isBlocking = false;
      Animancer.Play(ToggleOnAnim);
    }

    public void ToggleOff() {
      Debug.Log("Bridge lowered!");
      isBlocking = true;
      Animancer.Play(ToggleOffAnim);
    }
    #endregion

  }
}
