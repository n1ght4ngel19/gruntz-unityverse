using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class ObjectSwitch : MapObject {
    public bool isPressed;
    protected bool hasBeenPressed;
    protected Sprite pressedSprite;
    protected Sprite releasedSprite;

    protected override void Start() {
      base.Start();

      if (this is not CheckpointSwitch) {
        SetupSpritez();
      }
    }

    protected virtual void PressSwitch() {
      isPressed = true;
      hasBeenPressed = true;
      spriteRenderer.sprite = pressedSprite;
    }

    protected void ReleaseSwitch() {
      isPressed = false;
      hasBeenPressed = false;
      spriteRenderer.sprite = releasedSprite;
    }

    // This is needed for situations where the Switch is toggled NOT by a Grunt (or other actor)
    protected void ToggleSwitch() {
      isPressed = !isPressed;
      spriteRenderer.sprite = isPressed ? pressedSprite : releasedSprite;
    }

    protected virtual void SetupSpritez() {
      releasedSprite = spriteRenderer.sprite;

      string typeAsString = GetType().ToString().Split(".").Last();

      Addressables.LoadAssetAsync<Sprite[]>($"{GlobalNamez.SwitchSpritezPath}/{typeAsString}.png").Completed += handle => {
        pressedSprite = handle.Result[1];
      };
    }
  }
}
