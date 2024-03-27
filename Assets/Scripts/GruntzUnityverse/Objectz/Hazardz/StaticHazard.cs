﻿using System.Collections;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class StaticHazard : Hazard {
	public bool active;
	public float delay;
	public float timeGap;

	public AnimationClip hazardAnim;
	public AnimancerComponent animTargetAnimancer;
	public SpriteRenderer animTargetSpriteRenderer;

	protected override IEnumerator Start() {
		yield return new WaitForSeconds(delay);

		StartCoroutine(Damage());
	}

	protected override IEnumerator Damage() {
		animTargetSpriteRenderer.enabled = true;
		animTargetAnimancer.Play(hazardAnim);

		if (gruntOnTop != null) {
			gruntOnTop.Die(AnimationManager.instance.burnDeathAnimation, false, false);
		}

		active = true;

		yield return new WaitForSeconds(hazardAnim.length);

		active = false;

		animTargetSpriteRenderer.enabled = false;
		animTargetAnimancer.Stop();

		yield return new WaitForSeconds(timeGap);

		StartCoroutine(Damage());
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			gruntOnTop = grunt;

			if (active) {
				gruntOnTop.Die(AnimationManager.instance.burnDeathAnimation, false, false);

				gruntOnTop = null;
			}
		}
	}
}
}