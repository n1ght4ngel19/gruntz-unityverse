using System.Collections.Generic;
using GruntzUnityverse.Itemz.Powerupz;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Itemz.Toyz;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

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
      LoadPowerupPickupAnimations();
    }

    private void LoadMiscPickupAnimations() {
      // Todo: Create MiscItem enum and replace manual loading with foreach loop
      // foreach (MiscItemName miscItemName in System.Enum.GetValues(typeof(MiscItemName))) {
      //   Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{miscItemName}.anim").Completed += handle => {
      //     tool.Add(nameof(miscItemName), handle.Result);
      //   };
      // }

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Coin.anim").Completed += handle => {
        misc.Add("Coin", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Helpbox.anim").Completed += handle => {
        misc.Add("Helpbox", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_Megaphone.anim").Completed += handle => {
        misc.Add("Megaphone", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterW.anim").Completed += handle => {
        misc.Add("WarpletterW", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterA.anim").Completed += handle => {
        misc.Add("WarpletterA", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterR.anim").Completed += handle => {
        misc.Add("WarpletterR", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>("Pickup_WarpletterP.anim").Completed += handle => {
        misc.Add("WarpletterP", handle.Result);
      };
    }

    private void LoadToolPickupAnimations() {
      // foreach (ToolName toolName in System.Enum.GetValues(typeof(ToolName))) {
      //   if (toolName != ToolName.Barehandz) {
      //     Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{toolName}.anim").Completed += handle => {
      //       tool.Add(nameof(toolName), handle.Result);
      //       Debug.Log(toolName);
      //     };
      //   }
      // }

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(Gauntletz)}.anim").Completed += handle => {
        tool.Add(nameof(Gauntletz), handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(Shovel)}.anim").Completed += handle => {
        tool.Add(nameof(Shovel), handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(Warpstone)}.anim").Completed += handle => {
        tool.Add(nameof(Warpstone), handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(GooberStraw)}.anim").Completed += handle => {
        tool.Add(nameof(GooberStraw), handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(Club)}.anim").Completed += handle => {
        tool.Add(nameof(Club), handle.Result);
      };
    }

    private void LoadToyPickupAnimations() {
      // foreach (ToyName toyName in System.Enum.GetValues(typeof(ToyName))) {
      //   Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{toyName}.anim").Completed += handle => {
      //     toy.Add(nameof(toyName), handle.Result);
      //   };
      // }

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(Beachball)}.anim").Completed += handle => {
        toy.Add(nameof(Beachball), handle.Result);
      };
    }

    private void LoadPowerupPickupAnimations() {
      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(ZapCola)}Can.anim").Completed += handle => {
        powerup.Add($"{nameof(ZapCola)}Can", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(ZapCola)}Bottle.anim").Completed += handle => {
        powerup.Add($"{nameof(ZapCola)}Bottle", handle.Result);
      };

      Addressables.LoadAssetAsync<AnimationClip>($"Pickup_{nameof(ZapCola)}Keg.anim").Completed += handle => {
        powerup.Add($"{nameof(ZapCola)}Keg", handle.Result);
      };
    }
  }
}
