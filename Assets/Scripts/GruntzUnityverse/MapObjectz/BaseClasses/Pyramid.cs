using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public class Pyramid : MapObject {
    public bool isDown;
    private AnimationClip _downAnim;
    private AnimationClip _upAnim;


    protected override void Start() {
      base.Start();

      LoadAnimationz();
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, !isDown);
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"{GetType().Name}_Down.anim").Completed +=
        (handle) => {
          _downAnim = handle.Result;
        };

      Addressables.LoadAssetAsync<AnimationClip>($"{GetType().Name}_Up.anim").Completed +=
        (handle) => {
          _upAnim = handle.Result;
        };
    }

    public void Toggle() {
      animancer.Play(isDown ? _upAnim : _downAnim);

      isDown = !isDown;
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, !isDown);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, !isDown);
    }
  }
}
