using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class Hazard : GridObject {
	public int damage;
	public bool hideUnderMound;

	public Grunt gruntOnTop => GameManager.instance.allGruntz.FirstOrDefault(gr => gr.node == node);

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
}
}
