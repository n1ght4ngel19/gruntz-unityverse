using Animancer;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public interface IAnimatable {
    Animator Animator { get; set; }

    AnimancerComponent Animancer { get; set; }
  }
}
