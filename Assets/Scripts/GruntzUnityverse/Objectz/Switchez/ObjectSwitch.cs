using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class ObjectSwitch : MapObject {
    public Sprite pressedSprite;
    private Sprite _releasedSprite;
    public bool isPressed;
    protected bool HasBeenPressed;

    protected override void Start() {
      base.Start();

      _releasedSprite = spriteRenderer.sprite;
    }

    public void PressSwitch() {
      isPressed = true;
      HasBeenPressed = true;
      spriteRenderer.sprite = pressedSprite;
    }

    public void ReleaseSwitch() {
      isPressed = false;
      HasBeenPressed = false;
      spriteRenderer.sprite = _releasedSprite;
    }

    // This is needed for situations where the Switch is toggled NOT by a Grunt (or other actor)
    protected void ToggleSwitch() {
      isPressed = !isPressed;
      spriteRenderer.sprite = isPressed ? pressedSprite : _releasedSprite;
    }
  }
}
