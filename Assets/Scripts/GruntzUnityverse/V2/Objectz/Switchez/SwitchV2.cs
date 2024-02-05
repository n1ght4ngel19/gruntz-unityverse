using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz.Switchez {
  public abstract class SwitchV2 : GridObject, IBinaryToggleable {
    protected override void Start() {
      base.Start();

      Setup();
    }

    protected virtual void Setup() { }

    #region IBinaryToggleable
    // --------------------------------------------------
    // IBinaryToggleable
    // --------------------------------------------------
    [field: SerializeField] public bool IsOn { get; set; }
    [field: SerializeField] public Sprite OnSprite { get; set; }
    [field: SerializeField] public Sprite OffSprite { get; set; }

    public virtual void Toggle() {
      if (IsOn) {
        ToggleOff();
      } else {
        ToggleOn();
      }
    }

    public virtual void ToggleOn() {
      IsOn = true;
      spriteRenderer.sprite = OnSprite;
    }

    public virtual void ToggleOff() {
      IsOn = false;
      spriteRenderer.sprite = OffSprite;
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
