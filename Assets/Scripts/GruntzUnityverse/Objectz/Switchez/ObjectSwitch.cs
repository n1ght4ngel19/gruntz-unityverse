using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class ObjectSwitch : MapObject {
    public bool IsPressed;
    protected bool HasBeenPressed;
    public Sprite pressedSprite;
    private Sprite _releasedSprite;

    protected override void Start() {
      base.Start();

      _releasedSprite = SpriteRenderer.sprite;
    }

    protected void PressSwitch() {
      IsPressed = true;
      HasBeenPressed = true;
      SpriteRenderer.sprite = pressedSprite;
    }

    protected void ReleaseSwitch() {
      IsPressed = false;
      HasBeenPressed = false;
      SpriteRenderer.sprite = _releasedSprite;
    }

    // This is needed for situations where the Switch is toggled NOT by a Grunt (or other actor)
    protected void ToggleSwitch() {
      IsPressed = !IsPressed;
      SpriteRenderer.sprite = IsPressed ? pressedSprite : _releasedSprite;
    }
  }
}
