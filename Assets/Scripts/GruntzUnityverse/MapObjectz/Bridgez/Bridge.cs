using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class Bridge : MapObject {
    private bool _isDeath;
    private bool _isDown;
    private AnimationClip _downAnim;
    private AnimationClip _upAnim;

    // ------------------------------------------------------------ //
    // OVERRIDES
    // ------------------------------------------------------------ //
    public override void Setup() {
      base.Setup();

      _isDeath = spriteRenderer.sprite.name.Contains("Death");
      _isDown = !spriteRenderer.sprite.name.EndsWith("_0");
      ownNode.isBlocked = _isDown;
      ownNode.isWater = _isDown;
      ownNode.isDeath = _isDown && _isDeath;

      LoadAnimationz();
    }

    protected override void LoadAnimationz() {
      string optionalDeath = _isDeath ? "Death" : "";
      string downPath = $"{GlobalNamez.BridgeAnimzPath}/{area}/Clipz/{abbreviatedArea}_{optionalDeath}Bridge_Down.anim";
      string upPath = $"{GlobalNamez.BridgeAnimzPath}/{area}/Clipz/{abbreviatedArea}_{optionalDeath}Bridge_Up.anim";

      Addressables.LoadAssetAsync<AnimationClip>(downPath).Completed += handle => {
        _downAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>(upPath).Completed += handle => {
        _upAnim = handle.Result;
      };
    }

    // ------------------------------------------------------------ //
    // CLASS METHODS
    // ------------------------------------------------------------ //
    public void Toggle() {
      animancer.Play(_isDown ? _upAnim : _downAnim);

      _isDown = !_isDown;
      ownNode.isBlocked = _isDown;
      ownNode.isWater = _isDown;
    }
  }
}
