using System.Linq;
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

	private void SetupRockOnTop() {
		Rock rockOnTop = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => r.location2D == location2D);

		if (rockOnTop == null) {
			return;
		}

		rockOnTop.hiddenSwitch = this;

		spriteRenderer.enabled = false;
		enabled = false;

		Deactivate();
	}

	private void SetupHole() {
		Hole hole = FindObjectsByType<Hole>(FindObjectsSortMode.None)
			.FirstOrDefault(h => h.location2D == location2D);

		if (hole == null) {
			return;
		}

		if (hole.open) {
			Debug.LogError("An open hole cannot have a hidden switch!");

			return;
		}

		hole.hiddenSwitch = this;

		spriteRenderer.enabled = false;
		enabled = false;

		Deactivate();
	}

	// --------------------------------------------------
	// Overrides
	// --------------------------------------------------

	protected virtual void Toggle(bool checkPressed = false) {
		if (isPressed && checkPressed) {
			return;
		}

		isPressed = !isPressed;
		spriteRenderer.sprite = isPressed ? pressedSprite : releasedSprite;
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void Reset() {
		GetComponent<CircleCollider2D>().excludeLayers = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
		GetComponent<CircleCollider2D>().includeLayers = LayerMask.GetMask("RollingBall", "Grunt");
	}

	protected override void OnValidate() {
		base.OnValidate();

		GetComponent<CircleCollider2D>().excludeLayers = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
		GetComponent<CircleCollider2D>().includeLayers = LayerMask.GetMask("RollingBall", "Grunt");
	}

	protected override void Start() {
		base.Start();

		SetupRockOnTop();

		SetupHole();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		Toggle(checkPressed: true);
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		Toggle();
	}
}
}
