using GruntzUnityverse.MapObjectz.Switchez;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public class ObjectSwitch : MapObject {
    public bool isPressed;
    protected bool hasBeenPressed;
    protected bool hasBeenReleased;
    protected Sprite pressedSprite;
    protected Sprite releasedSprite;
    private AudioClip _pressClip;
    private AudioClip _releaseClip;

    protected override void Start() {
      base.Start();

      if (this is not CheckpointSwitch) {
        SetupSpritez();
      }

      Addressables.LoadAssetAsync<AudioClip>("Assets/Audio/Soundz/Sound_Switch_Down.wav").Completed += handle => {
        _pressClip = handle.Result;
      };

      Addressables.LoadAssetAsync<AudioClip>("Assets/Audio/Soundz/Sound_Switch_Up.wav").Completed += handle => {
        _releaseClip = handle.Result;
      };

      hasBeenReleased = true;
    }

    protected virtual void PressSwitch() {
      isPressed = true;
      hasBeenPressed = true;
      hasBeenReleased = false;
      spriteRenderer.sprite = pressedSprite;
      AudioSource.PlayClipAtPoint(_pressClip, Camera.main.transform.position);
      Debug.Log("Pressing");
    }

    protected void ReleaseSwitch() {
      isPressed = false;
      hasBeenPressed = false;
      hasBeenReleased = true;
      spriteRenderer.sprite = releasedSprite;
      AudioSource.PlayClipAtPoint(_releaseClip, Camera.main.transform.position);
      Debug.Log("Releasing");
    }

    // This is needed for situations where the Switch is toggled NOT by a Grunt (or other actor)
    protected void ToggleSwitch() {
      isPressed = !isPressed;
      spriteRenderer.sprite = isPressed ? pressedSprite : releasedSprite;
    }

    protected virtual void SetupSpritez() {
      releasedSprite = spriteRenderer.sprite;

      Addressables.LoadAssetAsync<Sprite[]>($"{GlobalNamez.SwitchSpritezPath}/{GetType().Name}.png").Completed += handle => {
        pressedSprite = handle.Result[1];
      };
    }
  }
}
