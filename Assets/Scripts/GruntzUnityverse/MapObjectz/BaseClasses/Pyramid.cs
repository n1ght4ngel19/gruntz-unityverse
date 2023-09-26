using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public class Pyramid : MapObject {
    private bool _isDown;
    private AnimationClip _downAnim;
    private AnimationClip _upAnim;

    // ------------------------------------------------------------ //
    // OVERRIDES
    // ------------------------------------------------------------ //
    public override void Setup() {
      base.Setup();

      _isDown = !spriteRenderer.sprite.name.EndsWith("_0");
      ownNode.isBlocked = !_isDown;
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"{GetType().Name}_Down.anim").Completed +=
        handle => {
          _downAnim = handle.Result;
        };

      Addressables.LoadAssetAsync<AnimationClip>($"{GetType().Name}_Up.anim").Completed +=
        handle => {
          _upAnim = handle.Result;
        };
    }

    public void Toggle() {
      animancer.Play(_isDown ? _upAnim : _downAnim);

      _isDown = !_isDown;
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, !_isDown);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, !_isDown);
    }
  }
}
