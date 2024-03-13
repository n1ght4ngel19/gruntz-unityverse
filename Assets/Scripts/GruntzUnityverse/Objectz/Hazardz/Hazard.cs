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

	public Grunt GruntOnTop => GameManager.Instance.allGruntz.FirstOrDefault(gr => gr.node == node);

	public override void Setup() {
		base.Setup();

		Rock rock = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (rock != null) {
			rock.HiddenHazard = this;
			spriteRenderer.enabled = false;
			enabled = false;
			circleCollider2D.isTrigger = false;

			return;
		}

		Hole hole = FindObjectsByType<Hole>(FindObjectsSortMode.None)
			.FirstOrDefault(r => Vector2Int.RoundToInt(r.transform.position) == Vector2Int.RoundToInt(transform.position));

		if (hole != null && hideUnderMound) {
			hole.HiddenHazard = this;
			spriteRenderer.enabled = false;
			circleCollider2D.isTrigger = false;
			enabled = false;

			return;
		}
	}

	protected virtual void Start() {
		StopAllCoroutines();

		StartCoroutine(Damage());
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
