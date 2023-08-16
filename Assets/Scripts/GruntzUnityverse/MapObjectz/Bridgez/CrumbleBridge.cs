using GruntzUnityverse.Managerz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class CrumbleBridge : Bridge {
    protected override void LoadAnimationz() {
      string optionalDeath = isDeathBridge ? "Death" : "";

      Addressables.LoadAssetAsync<AnimationClip>($"Crumble{optionalDeath}Bridge_Down_{abbreviatedArea}.anim")
        .Completed += (handle) => {
        downAnim = handle.Result;
      };
    }

    public void Crumble() {
      animancer.Play(downAnim);

      LevelManager.Instance.SetBlockedAt(location, false);
    }
  }
}
