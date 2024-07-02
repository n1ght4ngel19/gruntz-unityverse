using System.Collections;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class StaticHazard : Hazard {
	[BoxGroup("Hazard Data")]
	[ReadOnly]
	public bool active;

	[BoxGroup("Hazard Data")]
	public float delay;

	[BoxGroup("Hazard Data")]
	public float timeGap;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip hazardAnim;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animTargetAnimancer;

	[BoxGroup("Animation")]
	[DisableIf(nameof(isInstance))]
	public SpriteRenderer animTargetSpriteRenderer;

	protected override IEnumerator Start() {
		yield return new WaitForSeconds(delay);

		StartCoroutine(Damage());
	}

	protected override IEnumerator Damage() {
		animTargetSpriteRenderer.enabled = true;
		animTargetAnimancer.Play(hazardAnim);

		if (gruntOnTop != null && !gruntOnTop.between) {
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
