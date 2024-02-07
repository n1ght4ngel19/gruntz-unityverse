using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Switchez {
  public abstract class SwitchV2 : GridObject, IBinaryToggleable {

    #region IBinaryToggleable
    // --------------------------------------------------
    // IBinaryToggleable
    // --------------------------------------------------
    [field: SerializeField] public bool IsPressed { get; set; }
    [field: SerializeField] public Sprite PressedSprite { get; set; }
    [field: SerializeField] public Sprite ReleasedSprite { get; set; }

    public virtual void Toggle() {
      if (IsPressed) {
        ToggleOff();
      } else {
        ToggleOn();
      }
    }

    public virtual void ToggleOn() {
      IsPressed = true;
      spriteRenderer.sprite = PressedSprite;
    }

    public virtual void ToggleOff() {
      IsPressed = false;
      spriteRenderer.sprite = ReleasedSprite;
    }
    #endregion

    protected virtual IEnumerator OnTriggerEnter2D(Collider2D other) {
      if (!circleCollider2D.isTrigger) {
        yield break;
      }

      ToggleOn();
    }

    protected virtual IEnumerator OnTriggerExit2D(Collider2D other) {
      if (!circleCollider2D.isTrigger) {
        yield break;
      }

      ToggleOff();
    }
  }
}
