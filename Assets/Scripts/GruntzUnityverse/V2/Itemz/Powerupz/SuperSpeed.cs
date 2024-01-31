using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Powerupz {
  public class SuperSpeed : Powerup {
    /// <summary>
    /// The multiplier to apply to the grunt's base move speed.
    /// </summary>
    public float speedMultiplier;

    public override IEnumerator Activate() {
      affectedGrunt.statz.moveSpeed *= speedMultiplier;

      yield return new WaitForSeconds(duration);

      affectedGrunt.statz.moveSpeed /= speedMultiplier;
    }
  }
}
