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
      tool = new Dictionary<string, AnimationClip>();
      toy = new Dictionary<string, AnimationClip>();
      powerup = new Dictionary<string, AnimationClip>();

      LoadMiscPickupAnimations();
      LoadToolPickupAnimations();
      LoadToyPickupAnimations();
    }

    private void LoadMiscPickupAnimations() {
      // Todo: Create MiscItem enum and replace manual loading with foreach loop
      // foreach (MiscItemName miscItemName in System.Enum.GetValues(typeof(MiscItemName))) {
      //   Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{miscItemName}.anim").Completed += (handle) => {
      //     tool.Add(nameof(miscItemName), handle.Result);
      //   };
      // }

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

    }

    private void LoadToolPickupAnimations() {
      // foreach (ToolName toolName in System.Enum.GetValues(typeof(ToolName))) {
      //   if (toolName != ToolName.Barehandz) {
      //     Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{toolName}.anim").Completed += (handle) => {
      //       tool.Add(nameof(toolName), handle.Result);
      //       Debug.Log(toolName);
      //     };
      //   }
      // }

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Tool_Gauntletz.anim").Completed += (handle) => {
        tool.Add("Gauntletz", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Tool_Shovel.anim").Completed += (handle) => {
        tool.Add("Shovel", handle.Result);
      };
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Tool_Warpstone.anim").Completed += (handle) => {
        tool.Add("Warpstone", handle.Result);
      };
    }

    private void LoadToyPickupAnimations() {
      // foreach (ToyName toyName in System.Enum.GetValues(typeof(ToyName))) {
      //   Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{toyName}.anim").Completed += (handle) => {
      //     toy.Add(nameof(toyName), handle.Result);
      //   };
      // }
      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Toy_Beachball.anim").Completed += (handle) => {
        toy.Add("Beachball", handle.Result);
      };
    }
  }
}
