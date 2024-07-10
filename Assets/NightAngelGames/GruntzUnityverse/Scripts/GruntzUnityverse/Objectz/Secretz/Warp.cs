using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class Warp : GridObject {
	[BoxGroup("Warp Data")]
	[DisableIf(nameof(isInstance))]
	public WarpType warpType;

	[BoxGroup("Warp Data")]
	[Required]
	public Transform warpDestination;

	[BoxGroup("Warp Data")]
	public int duration;

	[Foldout("Animation")]
	[Required]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animancer;

	[Foldout("Animation")]
	[Required]
	[DisableIf(nameof(isInstance))]
	public AnimationClip appearAnim;

	[Foldout("Animation")]
	[Required]
	[DisableIf(nameof(isInstance))]
	public AnimationClip disappearAnim;

	[Foldout("Animation")]
	[Required]
	[DisableIf(nameof(isInstance))]
	public AnimationClip swirlingAnim;

	protected override void Awake() {
		base.Awake();

		animancer = GetComponent<AnimancerComponent>();

		spriteRenderer.enabled = warpType is not WarpType.Red;
		circleCollider2D.isTrigger = warpType is not WarpType.Red;
		enabled = warpType is not WarpType.Red;

		if (warpType is not WarpType.Red) {
			animancer.Play(swirlingAnim);
		}
	}

	public override async void Activate() {
		spriteRenderer.enabled = true;

		await animancer.Play(appearAnim);

		circleCollider2D.isTrigger = true;
		enabled = true;

		animancer.Play(swirlingAnim);

		// If the duration is not positive,
		// the warp will be active indefinitely (or until a Grunt steps into it)
		if (duration <= 0) {
			return;
		}

		await UniTask.WaitForSeconds(duration);

		animancer.Play(disappearAnim);
		await UniTask.WaitForSeconds(disappearAnim.length);

		circleCollider2D.isTrigger = false;
		spriteRenderer.enabled = false;
	}

	public override async void Deactivate() {
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpEnterAnim.length);

		animancer.Play(disappearAnim);
		await UniTask.WaitForSeconds(disappearAnim.length);

		circleCollider2D.isTrigger = false;
		spriteRenderer.enabled = false;
	}

	protected override async void OnTriggerEnter2D(Collider2D other) {
		node.grunt.Teleport(warpDestination.transform);

		if (warpType is WarpType.Red) {
			Deactivate();
		}

		if (warpType is WarpType.Blue or WarpType.Green) {
			animancer.Play(disappearAnim);
			await UniTask.WaitForSeconds(disappearAnim.length);

			circleCollider2D.isTrigger = false;
			spriteRenderer.enabled = false;
		}
	}
}

public enum WarpType {
	Red,
	Blue,
	Green,
	Yellow,
}
}
