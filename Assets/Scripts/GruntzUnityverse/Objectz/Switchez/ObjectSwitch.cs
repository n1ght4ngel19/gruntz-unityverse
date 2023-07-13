using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class ObjectSwitch : MapObject {
    public Sprite pressedSprite;
    public Sprite releasedSprite;
    public bool isPressed;
    protected bool HasBeenPressed;

    protected void PressSwitch() {
      isPressed = true;
      HasBeenPressed = true;
      SpriteRenderer.sprite = pressedSprite;
    }

    protected void ReleaseSwitch() {
      isPressed = false;
      HasBeenPressed = false;
      SpriteRenderer.sprite = releasedSprite;
    }

    // This is needed for situations where the Switch is toggled NOT by a Grunt (or other actor)
    protected void ToggleSwitch() {
      isPressed = !isPressed;
      SpriteRenderer.sprite = isPressed ? pressedSprite : releasedSprite;
    }
  }
}
