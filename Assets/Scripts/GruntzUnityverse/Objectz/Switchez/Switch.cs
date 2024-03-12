using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public abstract class Switch : GridObject {
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

	[field: SerializeField] public bool IsPressed { get; set; }
	[field: SerializeField] public Sprite PressedSprite { get; set; }
	[field: SerializeField] public Sprite ReleasedSprite { get; set; }

	public virtual void Toggle() {
		IsPressed = !IsPressed;
		spriteRenderer.sprite = IsPressed ? PressedSprite : ReleasedSprite;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt _) && !other.TryGetComponent(out RollingBall _)) {
			return;
		}

		if (!IsPressed) {
			Toggle();
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt _) && !other.TryGetComponent(out RollingBall _)) {
			return;
		}

		Toggle();
	}
}
}
