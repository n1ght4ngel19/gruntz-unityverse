using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public interface IBreakable {
    public AnimationClip BreakAnimation { get; set; }

    public IEnumerator Break();
  }
}
