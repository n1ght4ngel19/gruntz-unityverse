using System.Linq;
using System.Threading.Tasks;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class CrumbleBridge : MapObject {
    public int crumbleDelay;
    private bool _isDeath;
    private AnimationClip _anim;
    // ------------------------------------------------------------ //

    private void Update() {
      if (!GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        return;
      }

      Crumble();
    }

    // ------------------------------------------------------------ //
    // OVERRIDES
    // ------------------------------------------------------------ //
    public override void Setup() {
      base.Setup();

      _isDeath = spriteRenderer.sprite.name.Contains("Death");
      ownNode.isBlocked = false;
      ownNode.isWater = false;
      ownNode.isDeath = false;

      LoadAnimationz();
    }

    protected override void LoadAnimationz() {
      string optionalDeath = _isDeath ? "Death" : "";

      Addressables.LoadAssetAsync<AnimationClip>(
          $"{GlobalNamez.BridgeAnimzPath}/{area}/Clipz/{abbreviatedArea}_Crumble{optionalDeath}Bridge.anim")
        .Completed += handle => {
        _anim = handle.Result;
      };
    }

    // ------------------------------------------------------------ //
    // CLASS METHODS
    // ------------------------------------------------------------ //
    private async void Crumble() {
      await Task.Delay(crumbleDelay * 1000);

      animancer.Play(_anim);

      await Task.Delay(200);

      ownNode.isBlocked = true;
      ownNode.isWater = true;
      ownNode.isDeath = _isDeath;

      SetEnabled(false);
    }
  }
}
