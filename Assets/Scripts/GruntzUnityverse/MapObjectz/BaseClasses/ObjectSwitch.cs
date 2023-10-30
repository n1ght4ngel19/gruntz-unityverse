using System.Linq;
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

    // ------------------------------------------------------------ //
    // CLASS METHODS
    // ------------------------------------------------------------ //
    protected virtual void PressSwitch() {
      isPressed = true;
      hasBeenPressed = true;
      hasBeenReleased = false;
      spriteRenderer.sprite = pressedSprite;
      AudioSource.PlayClipAtPoint(_pressClip, Camera.main.transform.position);
    }

    protected virtual void ReleaseSwitch() {
      isPressed = false;
      hasBeenReleased = true;
      spriteRenderer.sprite = releasedSprite;
      AudioSource.PlayClipAtPoint(_releaseClip, Camera.main.transform.position);
    }

    // This is needed for situations where the Switch is toggled NOT by a Grunt (or other actor)
    protected void ToggleSwitch() {
      isPressed = !isPressed;
      spriteRenderer.sprite = isPressed ? pressedSprite : releasedSprite;
    }

    protected virtual void SetupSpritez() {
      // Todo: Rename all Addressable assets to simple {SwitchType}.png
      Addressables.LoadAssetAsync<Sprite[]>($"{GlobalNamez.SwitchSpritezPath}/{GetType().Name}.png").Completed += handle => {
        releasedSprite = handle.Result[0];
        pressedSprite = handle.Result[1];
      };
    }

    public bool IsBeingPressed() {
      return GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.navigator.ownNode == ownNode)
        || GameManager.Instance.currentLevelManager.rollingBallz.Any(ball => ball.ownNode == ownNode);
    }

    // ------------------------------------------------------------ //
    // OVERRIDES
    // ------------------------------------------------------------ //
    public override void Setup() {
      base.Setup();

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
  }
}
