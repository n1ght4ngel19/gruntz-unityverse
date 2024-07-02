using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Interactablez;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public abstract class Switch : GridObject {
	[BoxGroup("Switch Data")]
	public bool hideUnderMound;

	[BoxGroup("Switch Data")]
	[DisableIf(nameof(isInstance))]
	public bool isPressed;

	[BoxGroup("Switch Data")]
	[DisableIf(nameof(isInstance))]
	public Sprite pressedSprite;

	[BoxGroup("Switch Data")]
	[DisableIf(nameof(isInstance))]
	public Sprite releasedSprite;

	public override void Setup() {
		base.Setup();

		Rock rock = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (rock != null) {
			rock.hiddenSwitch = this;
			spriteRenderer.enabled = false;
			circleCollider2D.isTrigger = false;
			enabled = false;

			return;
		}

		Hole hole = FindObjectsByType<Hole>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (hole == null || !hideUnderMound) {
			return;
		}

		hole.hiddenSwitch = this;
		spriteRenderer.enabled = false;
		circleCollider2D.isTrigger = false;
		enabled = false;
	}

	public virtual void Toggle() {
		isPressed = !isPressed;
		spriteRenderer.sprite = isPressed ? pressedSprite : releasedSprite;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt _) && !other.TryGetComponent(out RollingBall _)) {
			return;
		}

		if (!isPressed) {
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
