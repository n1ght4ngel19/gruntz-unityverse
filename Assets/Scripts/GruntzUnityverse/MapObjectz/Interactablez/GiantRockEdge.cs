using System.Collections;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Interactablez {
  public class GiantRockEdge : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    public GiantRock mainRock;

    protected override void Start() {
      base.Start();

      GameManager.Instance.currentLevelManager.SetBlockedAt(location, true);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, true);
    }

    public IEnumerator Break(float contactDelay) {
      StartCoroutine(mainRock.Break(contactDelay));

      yield return null;
    }
  }
}
