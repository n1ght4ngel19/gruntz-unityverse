using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class Bridge : MapObject {
    public bool isDown;
    public bool isDeathBridge;
    protected AnimationClip downAnim;
    protected AnimationClip upAnim;


    protected override void Start() {
      base.Start();

      LoadAnimationz();
    }

    protected virtual void Update() {
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, isDown);
      GameManager.Instance.currentLevelManager.SetWaterAt(location, isDown);
      GameManager.Instance.currentLevelManager.SetDeathAt(location, isDown);

      enabled = false;
    }

    protected override void LoadAnimationz() {
      string optionalDeath = isDeathBridge ? "Death" : "";

      Addressables.LoadAssetAsync<AnimationClip>($"Assets/Animationz/MapObjectz/Bridgez/{area}/Clipz/{abbreviatedArea}_{optionalDeath}Bridge_Down.anim").Completed +=
        (handle) => {
          downAnim = handle.Result;
        };

      Addressables.LoadAssetAsync<AnimationClip>($"Assets/Animationz/MapObjectz/Bridgez/{area}/Clipz/{abbreviatedArea}_{optionalDeath}Bridge_Up.anim").Completed +=
        (handle) => {
          upAnim = handle.Result;
        };
    }

    public void Toggle() {
      animancer.Play(isDown ? upAnim : downAnim);

      isDown = !isDown;
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, isDown);
      GameManager.Instance.currentLevelManager.SetWaterAt(location, isDown);
    }
  }
}
