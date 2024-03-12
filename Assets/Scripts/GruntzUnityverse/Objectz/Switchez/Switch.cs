using System.Collections;
using System.Linq;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public abstract class Switch : GridObject, IBinaryToggleable {
	public override void Setup() {
		base.Setup();

		Rock rock = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (rock != null) {
			rock.HiddenSwitch = this;
			spriteRenderer.enabled = false;
			circleCollider2D.isTrigger = false;
			enabled = false;

			return;
		}

		Hole hole = FindObjectsByType<Hole>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (hole != null) {
			hole.HiddenSwitch = this;
			spriteRenderer.enabled = false;
			circleCollider2D.isTrigger = false;
			enabled = false;

			return;
		}
	}

	#region IBinaryToggleable
	// --------------------------------------------------
	// IBinaryToggleable
	// --------------------------------------------------
	[field: SerializeField] public bool IsPressed { get; set; }
	[field: SerializeField] public Sprite PressedSprite { get; set; }
	[field: SerializeField] public Sprite ReleasedSprite { get; set; }

	public virtual void Toggle() {
		IsPressed = !IsPressed;
		spriteRenderer.sprite = IsPressed ? PressedSprite : ReleasedSprite;
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

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (!circleCollider2D.isTrigger) {
			return;
		}

		Toggle();
	}

	protected virtual IEnumerator OnTriggerExit2D(Collider2D other) {
		if (!circleCollider2D.isTrigger) {
			yield break;
		}

		ToggleOff();
	}
}
}
