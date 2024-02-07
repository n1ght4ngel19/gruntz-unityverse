using System.Collections;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Collectiblez {
  public class Coin : ItemV2 {
    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      Debug.Log("Coin picked up!");
      GM.Instance.levelStatz.coinz++;
    }
  }
}
