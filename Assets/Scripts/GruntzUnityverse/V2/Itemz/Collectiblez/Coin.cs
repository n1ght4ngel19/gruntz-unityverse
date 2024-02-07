using System.Collections;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.V2.Itemz.Collectiblez {
  public class CoinV2 : ItemV2 {
    protected override void LoadAnims() {
      Addressables.LoadAssetAsync<AnimationClip>("Coin_Rotating.anim").Completed += handle => rotatingAnim = handle.Result;
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Coin.anim").Completed += handle => pickupAnim = handle.Result;
    }

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      Debug.Log("Coin picked up!");
      GM.Instance.levelStatz.coinz++;
    }
  }
}
