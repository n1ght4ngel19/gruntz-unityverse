using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Interactablez {
  public interface IBreakable {
    public AnimationClip BreakAnimation { get; set; }

    public IEnumerator Break(float contactDelay);
  }
}
