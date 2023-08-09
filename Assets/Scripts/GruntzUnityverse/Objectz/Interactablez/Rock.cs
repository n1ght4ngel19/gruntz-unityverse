using System.Collections;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
  public class Rock : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }

    protected override void Start() {
      base.Start();

      AssignAreaBySpriteName();

      LevelManager.Instance.SetBlockedAt(location, true);
      LevelManager.Instance.SetHardTurnAt(location, true);
      BreakAnimation = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Rockz/Clipz/RockBreak_{area}_01");
    }

    public IEnumerator Break() {
      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Rock actually should break)
      yield return new WaitForSeconds(1.5f);

      if (this != null) {
        Animancer.Play(BreakAnimation);
      }

      transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
      transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

      //yield return new WaitForSeconds(1.5f);
      yield return new WaitForSeconds(BreakAnimation.length);

      LevelManager.Instance.Rockz.Remove(this);
      LevelManager.Instance.SetBlockedAt(location, false);
      LevelManager.Instance.SetHardTurnAt(location, false);

      enabled = false;
    }
  }
}
