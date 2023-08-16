using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class ObjectSwitch : MapObject {
    public Sprite pressedSprite;
    private Sprite _releasedSprite;
    public bool isPressed;
    protected bool hasBeenPressed;

    protected override void Start() {
      base.Start();

      _releasedSprite = spriteRenderer.sprite;
    }

    protected void PressSwitch() {
      isPressed = true;
      hasBeenPressed = true;
      spriteRenderer.sprite = pressedSprite;
    }

    protected void ReleaseSwitch() {
      isPressed = false;
      hasBeenPressed = false;
      spriteRenderer.sprite = _releasedSprite;
    }

    // This is needed for situations where the Switch is toggled NOT by a Grunt (or other actor)
    protected void ToggleSwitch() {
      isPressed = !isPressed;
      spriteRenderer.sprite = isPressed ? pressedSprite : _releasedSprite;
    }
  }
}
