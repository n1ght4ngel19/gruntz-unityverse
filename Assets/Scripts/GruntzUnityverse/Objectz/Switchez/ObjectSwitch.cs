using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class ObjectSwitch : MapObject {
    [field: SerializeField] public Sprite PressedSprite { get; set; }
    [field: SerializeField] public Sprite ReleasedSprite { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }

    protected void PressSwitch() {
      IsPressed = true;
      HasBeenPressed = true;
      Renderer.sprite = PressedSprite;
    }

    protected void ReleaseSwitch() {
      IsPressed = false;
      HasBeenPressed = false;
      Renderer.sprite = ReleasedSprite;
    }

    protected void ToggleSwitch() {
      IsPressed = !IsPressed;

      Renderer.sprite = IsPressed
        ? PressedSprite
        : ReleasedSprite;
    }
  }
}
