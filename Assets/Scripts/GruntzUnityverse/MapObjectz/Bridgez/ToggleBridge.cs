using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class ToggleBridge : MapObject, IAudioSource {
    /// <summary>
    /// The time in seconds between the Bridge toggling its state.
    /// </summary>
    [Range(0f, 50f)]
    public float interval;
    private bool _isDeath;
    private bool _isDown;
    private AnimationClip _downAnim;
    private AnimationClip _upAnim;
    public AudioSource AudioSource { get; set; }
    public AudioClip toggleSound;
    // ------------------------------------------------------------ //

    protected override void Start() {
      InvokeRepeating(nameof(Toggle), 0, interval);
    }

    private void Update() {
      // Todo: Move to OnValidate() or handle in Setup()
      if (interval <= 0) {
        Debug.LogError("Interval has to be a positive number!");

        enabled = false;
      }
    }

    // ------------------------------------------------------------ //
    // CLASS METHODS
    // ------------------------------------------------------------ //
    public void Toggle() {
      animancer.Play(_isDown ? _upAnim : _downAnim);
      AudioSource.PlayOneShot(toggleSound);

      _isDown = !_isDown;
      ownNode.isBlocked = _isDown;
      ownNode.isWater = _isDown;
    }

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
      
      AudioSource = gameObject.AddComponent<AudioSource>();
      string optionalDeath = _isDeath ? "Death" : "";
      Addressables.LoadAssetAsync<AudioClip>($"{abbreviatedArea}_{optionalDeath}Bridge.wav").Completed += handle => {
        toggleSound = handle.Result;
      };
    }

    protected override void LoadAnimationz() {
      string optionalDeath = _isDeath ? "Death" : "";
      string downPath = $"{GlobalNamez.BridgeAnimzPath}/{area}/Clipz/{abbreviatedArea}_Toggle{optionalDeath}Bridge_Down.anim";
      string upPath = $"{GlobalNamez.BridgeAnimzPath}/{area}/Clipz/{abbreviatedArea}_Toggle{optionalDeath}Bridge_Up.anim";

      Addressables.LoadAssetAsync<AnimationClip>(downPath).Completed += handle => {
        _downAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>(upPath).Completed += handle => {
        _upAnim = handle.Result;
      };
    }
  }
}
