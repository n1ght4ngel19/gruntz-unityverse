using System.Collections;
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

	[BoxGroup("Hazard Data")]
	[ReadOnly]
	public Grunt gruntOnTop;

	public override void Setup() {
		base.Setup();

		Rock rock = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (rock != null) {
			rock.hiddenHazard = this;
			spriteRenderer.enabled = false;
			enabled = false;
			circleCollider2D.isTrigger = false;

			return;
		}

		Hole hole = FindObjectsByType<Hole>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (hole != null && hideUnderMound) {
			hole.hiddenHazard = this;
			spriteRenderer.enabled = false;
			circleCollider2D.isTrigger = false;
			enabled = false;

			return;
		}
	}

	protected virtual IEnumerator Start() {
		StopAllCoroutines();

		StartCoroutine(Damage());

		yield break;
	}

	protected virtual void OnEnable() {
		StopAllCoroutines();

		StartCoroutine(Damage());
	}

	public virtual async void OnRevealed() { }

	protected virtual IEnumerator Damage() {
		yield return null;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			gruntOnTop = grunt;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt _)) {
			gruntOnTop = null;
		}
	}
}
}
