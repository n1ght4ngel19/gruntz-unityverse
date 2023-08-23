using System.Collections.Generic;
using GruntzUnityverse.Enumz;
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

      LoadMiscPickupAnimations();
      LoadToolPickupAnimations();
    }

    private void LoadMiscPickupAnimations() {
      // Todo: Create MiscItem enum and replace manual loading with foreach loop
      // foreach (MiscItemName miscItemName in System.Enum.GetValues(typeof(MiscItemName))) {
      //   Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{miscItemName}.anim").Completed += (handle) => {
      //     tool.Add(nameof(miscItemName), handle.Result);
      //   };
      // }

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Coin.anim").Completed += (handle) => {
        misc.Add("Coin", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Helpbox.anim").Completed += (handle) => {
        misc.Add("Helpbox", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Megaphone.anim").Completed += (handle) => {
        misc.Add("Megaphone", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterW.anim").Completed += (handle) => {
        misc.Add("WarpletterW", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterA.anim").Completed += (handle) => {
        misc.Add("WarpletterA", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterR.anim").Completed += (handle) => {
        misc.Add("WarpletterR", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterP.anim").Completed += (handle) => {
        misc.Add("WarpletterP", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Gauntletz.anim").Completed += (handle) => {
        misc.Add("Gauntletz", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Shovel.anim").Completed += (handle) => {
        misc.Add("Shovel", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Beachball.anim").Completed += (handle) => {
        misc.Add("Beachball", handle.Result);
      };
    }

    private void LoadToolPickupAnimations() {
      foreach (ToolName toolName in System.Enum.GetValues(typeof(ToolName))) {
        if (toolName != ToolName.Barehandz) {
          Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{toolName}.anim").Completed += (handle) => {
            tool.Add(nameof(toolName), handle.Result);
          };
        }
      }
    }
  }
}
