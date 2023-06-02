using System.Collections;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Rock : MapObject {
    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(OwnLocation, true);
    }

    public override IEnumerator BeUsed() {
      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Rock actually should break)
      yield return new WaitForSeconds(1.5f);

      Animancer.Play(Resources.Load<AnimationClip>("Animationz/MapObjectz/Rockz/Clipz/RockBreak_RockyRoadz"));

      // 1.5s is th length of the Rock explosion animation
      yield return new WaitForSeconds(1f);

      LevelManager.Instance.Rockz.Remove(this);
      LevelManager.Instance.SetBlockedAt(OwnLocation, false);
      Destroy(gameObject);
    }
  }
}
