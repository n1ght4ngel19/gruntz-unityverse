using System.Collections;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Rock : MapObject {
    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(OwnLocation, true);
    }

    /// <summary>
    /// Destroys the Rock.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> since this is a <see cref="Coroutine"/></returns>
    public IEnumerator Break() {
      OwnAnimator.Play("RockBreak");

      yield return new WaitForSeconds(OwnAnimator.GetCurrentAnimatorStateInfo(0).length);

      LevelManager.Instance.Rockz.Remove(this);
      LevelManager.Instance.SetBlockedAt(OwnLocation, false);
      Destroy(gameObject);
    }
  }
}
