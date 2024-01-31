using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Pyramidz {
  public abstract class PyramidV2 : GridObject, IToggleable {

    #region IToggleable
    // --------------------------------------------------
    // IToggleable
    // --------------------------------------------------
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public AnimancerComponent Animancer { get; set; }
    [field: SerializeField] public AnimationClip ToggleOnAnim { get; set; }
    [field: SerializeField] public AnimationClip ToggleOffAnim { get; set; }

    public virtual void Toggle() {
      if (actAsObstacle) {
        ToggleOff();
      } else {
        ToggleOn();
      }
    }

    public virtual void ToggleOn() {
      actAsObstacle = true;
      Animancer.Play(ToggleOnAnim);
    }

    public virtual void ToggleOff() {
      actAsObstacle = false;
      Animancer.Play(ToggleOffAnim);
    }
    #endregion

  }
}
