using System.Collections;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Collectiblez {
  public class CoinV2 : Collectible {

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      Debug.Log("Coin picked up!");
      GM.Instance.levelStatz.coinz++;
    }
  }
}
