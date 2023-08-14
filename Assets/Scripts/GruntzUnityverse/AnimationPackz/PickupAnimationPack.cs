using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.AnimationPackz {
  public class PickupAnimationPack {
    public readonly Dictionary<string, AnimationClip> misc;
    public readonly Dictionary<string, AnimationClip> powerup;
    public readonly Dictionary<string, AnimationClip> tool;
    public readonly Dictionary<string, AnimationClip> toy;

    public PickupAnimationPack() {
      misc = new Dictionary<string, AnimationClip>();
      powerup = new Dictionary<string, AnimationClip>();
      tool = new Dictionary<string, AnimationClip>();
      toy = new Dictionary<string, AnimationClip>();

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_Coin.anim").Completed += (handle) => {
        misc.Add("Coin", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_Helpbox.anim").Completed += (handle) => {
        misc.Add("Helpbox", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_Megaphone.anim").Completed += (handle) => {
        misc.Add("Megaphone", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_WarpletterW.anim").Completed += (handle) => {
        misc.Add("WarpletterW", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_WarpletterA.anim").Completed += (handle) => {
        misc.Add("WarpletterA", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_WarpletterR.anim").Completed += (handle) => {
        misc.Add("WarpletterR", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_WarpletterP.anim").Completed += (handle) => {
        misc.Add("WarpletterP", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_Gauntletz.anim").Completed += (handle) => {
        misc.Add("Gauntletz", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_Shovel.anim").Completed += (handle) => {
        misc.Add("Shovel", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Misc_Beachball.anim").Completed += (handle) => {
        misc.Add("Beachball", handle.Result);
      };
    }
  }
}
