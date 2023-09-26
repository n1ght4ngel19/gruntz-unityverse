using System.Linq;
using System.Threading.Tasks;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class CrumbleBridge : MapObject, IAudioSource {
    public int crumbleDelay;
    private bool _isDeath;
    private AnimationClip _anim;
    public AudioSource AudioSource { get; set; }
    public AudioClip crumbleSound;
    // ------------------------------------------------------------ //

    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.navigator.ownNode != ownNode)) {
        return;
      }

      Crumble();
    }

    // ------------------------------------------------------------ //
    // CLASS METHODS
    // ------------------------------------------------------------ //
    private async void Crumble() {
      await Task.Delay(crumbleDelay * 1000);

      animancer.Play(_anim);
      AudioSource.PlayOneShot(crumbleSound);

      await Task.Delay(200);

      ownNode.isBlocked = true;
      ownNode.isWater = true;
      ownNode.isDeath = _isDeath;

      SetEnabled(false);
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

      AudioSource = gameObject.AddComponent<AudioSource>();
      string optionalDeath = _isDeath ? "Death" : "";
      Addressables.LoadAssetAsync<AudioClip>($"{abbreviatedArea}_{optionalDeath}Bridge.wav").Completed += handle => {
        crumbleSound = handle.Result;
      };
    }

    protected override void LoadAnimationz() {
      string optionalDeath = _isDeath ? "Death" : "";

      Addressables.LoadAssetAsync<AnimationClip>(
          $"{GlobalNamez.BridgeAnimzPath}/{area}/Clipz/{abbreviatedArea}_Crumble{optionalDeath}Bridge.anim")
        .Completed += handle => {
        _anim = handle.Result;
      };
    }
  }
}
