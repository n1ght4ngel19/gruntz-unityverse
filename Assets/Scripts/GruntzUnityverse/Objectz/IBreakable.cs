using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public interface IBreakable {
    AnimationClip BreakAnimation { get; set; }

    public IEnumerator Break();
  }
}
