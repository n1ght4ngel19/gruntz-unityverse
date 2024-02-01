using System.Collections;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Powerupz {
  public class SuperSpeed : Powerup {
    /// <summary>
    /// The multiplier to apply to the grunt's base move speed.
    /// </summary>
    public float speedMultiplier;

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      target.statz.moveSpeed *= speedMultiplier;

      yield return new WaitForSeconds(duration);

      target.statz.moveSpeed /= speedMultiplier;
    }
  }
}
