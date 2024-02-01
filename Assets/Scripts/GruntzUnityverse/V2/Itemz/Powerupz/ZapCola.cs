using System.Collections;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Powerupz {
  public class ZapCola : Powerup {
    [Range(5, 20)]
    public int healAmount;

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      target.statz.health += healAmount;

      yield return null;
    }
  }
}
