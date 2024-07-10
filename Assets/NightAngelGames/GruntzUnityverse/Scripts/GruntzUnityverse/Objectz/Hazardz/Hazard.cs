using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Interactablez;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class Hazard : GridObject {
	[BoxGroup("Hazard Data")]
	public float damage;

	[BoxGroup("Hazard Data")]
	public bool hideUnderMound;

	public Grunt gruntOnTop => node.grunt;

	public virtual async void OnRevealed() { }

	protected virtual async void Damage() { }

	private void SetupRockOnTop() {
		Rock rockOnTop = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => r.location2D == location2D);

		if (rockOnTop == null) {
			return;
		}

		rockOnTop.hiddenHazard = this;

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
			Debug.LogError("An open hole cannot have a hidden item!");

			return;
		}

		hole.hiddenHazard = this;

		spriteRenderer.enabled = false;
		enabled = false;

		Deactivate();
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void OnEnable() {
		Damage();
	}

	protected override void Start() {
		base.Start();

		SetupRockOnTop();

		SetupHole();
	}
}
}
