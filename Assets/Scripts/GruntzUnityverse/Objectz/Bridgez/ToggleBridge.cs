using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class ToggleBridge : Bridge {
    /// <summary>
    /// The time in seconds between the Bridge toggling its state.
    /// </summary>
    public int interval;


    protected override void Update() {
      if (interval <= 0) {
        Debug.LogError("Interval has to be a positive number!");

        enabled = false;
      }
    }

    protected override void LoadAnimationz() {
      string optionalDeath = isDeathBridge ? "Death" : "";

      Addressables.LoadAssetAsync<AnimationClip>($"Toggle{optionalDeath}Bridge_Down_{abbreviatedArea}.anim")
          .Completed +=
        (handle) => {
          downAnim = handle.Result;
        };

      Addressables.LoadAssetAsync<AnimationClip>($"Toggle{optionalDeath}Bridge_Up_{abbreviatedArea}.anim")
          .Completed +=
        (handle) => {
          upAnim = handle.Result;
        };
    }
  }
}
