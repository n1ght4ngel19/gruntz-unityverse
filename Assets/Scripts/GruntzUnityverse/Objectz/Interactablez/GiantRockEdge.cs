using System.Collections;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
  public class GiantRockEdge : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    public GiantRock mainRock;

    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(location, true);
      LevelManager.Instance.SetHardTurnAt(location, true);
    }

    public IEnumerator Break() {
      StartCoroutine(mainRock.Break());

      yield return null;
    }
  }
}
