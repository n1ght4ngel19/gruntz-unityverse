using System.Collections;
using Animancer;
using Cysharp.Threading.Tasks;
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

	public bool recurse;

	protected override async void Start() {
		base.Start();

		await UniTask.WaitForSeconds(delay);

		Damage();
	}

	protected override async void Damage() {
		animTargetSpriteRenderer.enabled = true;
		animTargetAnimancer.Play(hazardAnim);

		if (gruntOnTop != null && !gruntOnTop.between) {
			gruntOnTop.Die(AnimationManager.instance.burnDeathAnimation, false, false);
		}

		active = true;

		await UniTask.WaitForSeconds(hazardAnim.length);

		active = false;

		animTargetSpriteRenderer.enabled = false;
		animTargetAnimancer.Stop();

		await UniTask.WaitForSeconds(timeGap);

		if (recurse) {
			Damage();
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		if (active) {
			gruntOnTop.Die(AnimationManager.instance.burnDeathAnimation, leavePuddle: false, playBelow: false);
		}
	}
}
}
