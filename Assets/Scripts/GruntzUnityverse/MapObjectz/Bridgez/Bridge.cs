using GruntzUnityverse.Managerz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class Bridge : MapObject {
    public bool isDown;
    public bool isDeathBridge;
    protected AnimationClip downAnim;
    protected AnimationClip upAnim;


    protected virtual void Update() {
      LevelManager.Instance.SetBlockedAt(location, isDown);
      LevelManager.Instance.SetWaterAt(location, isDown);
      LevelManager.Instance.SetDeathAt(location, isDown);

      enabled = false;
    }

    protected override void LoadAnimationz() {
      string optionalDeath = isDeathBridge ? "Death" : "";

      Addressables.LoadAssetAsync<AnimationClip>($"{optionalDeath}Bridge_Down_{abbreviatedArea}.anim").Completed +=
        (handle) => {
          downAnim = handle.Result;
        };

      Addressables.LoadAssetAsync<AnimationClip>($"{optionalDeath}Bridge_Up_{abbreviatedArea}.anim").Completed +=
        (handle) => {
          upAnim = handle.Result;
        };
    }

    public void Toggle() {
      animancer.Play(isDown ? upAnim : downAnim);

      isDown = !isDown;
      LevelManager.Instance.SetBlockedAt(location, isDown);
      LevelManager.Instance.SetWaterAt(location, isDown);
    }
  }
}
