using System.Collections;
using System.Linq;
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

      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Rock actually should break)
      // yield return new WaitForSeconds(1.5f);
      //
      // foreach (GiantRockEdge edge in mainRock.edges.Where(edge => edge != this)) {
      //   LevelManager.Instance.SetBlockedAt(edge.location, false);
      //   LevelManager.Instance.SetHardTurnAt(edge.location, false);
      //   edge.animancer.Play(BreakAnimation);
      //
      //   Destroy(edge.gameObject);
      // }
      //
      // mainRock.animancer.Play(mainRock.BreakAnimation);
      //
      // animancer.Play(BreakAnimation);
      //
      // yield return new WaitForSeconds(1f);
      //
      // LevelManager.Instance.SetBlockedAt(location, false);
      // LevelManager.Instance.SetHardTurnAt(location, false);
      //
      // Destroy(gameObject);
    }
  }
}
