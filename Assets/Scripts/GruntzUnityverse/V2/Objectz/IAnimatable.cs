using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public interface IAnimatable {
    /// <summary>
    /// The Animator component of this object, used by Animancer to play animations.
    /// </summary>
    Animator Animator { get; set; }

    /// <summary>
    /// The Animancer component of this object, used for easy access to animations.
    /// </summary>
    AnimancerComponent Animancer { get; set; }
  }
}
