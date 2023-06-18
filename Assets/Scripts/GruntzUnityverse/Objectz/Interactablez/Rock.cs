using System.Collections;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
  public class Rock : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }

    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(Location, true);
      BreakAnimation = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Rockz/Clipz/RockBreak_{Area}");
    }

    public IEnumerator Break() {
      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Rock actually should break)
      yield return new WaitForSeconds(1.5f);

      Animancer.Play(BreakAnimation);

      // 1.5s is th length of the Rock explosion animation
      yield return new WaitForSeconds(1f);

      LevelManager.Instance.Rockz.Remove(this);
      LevelManager.Instance.SetBlockedAt(Location, false);
      LevelManager.Instance.SetHardTurnAt(Location, false);
      Destroy(gameObject);
    }
  }
}
