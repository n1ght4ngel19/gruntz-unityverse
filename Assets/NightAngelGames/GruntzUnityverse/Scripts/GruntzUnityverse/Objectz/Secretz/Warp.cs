using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
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
	public Animator animator;

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

	private void Awake() {
		spriteRenderer.enabled = warpType is not WarpType.Red;
	}

	public async void Activate() {
		circleCollider2D.isTrigger = true;
		spriteRenderer.enabled = true;

		await animancer.Play(appearAnim);

		circleCollider2D.isTrigger = true;

		animancer.Play(swirlingAnim);

		// If the duration is not positive, the warp will be active indefinitely (or until a Grunt steps on it).
		if (duration <= 0) {
			return;
		}

		await UniTask.WaitForSeconds(duration);

		animancer.Play(disappearAnim);
		await UniTask.WaitForSeconds(disappearAnim.length);

		spriteRenderer.enabled = false;

		Destroy(gameObject);
	}

	public async void Deactivate() {
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpEnterAnim.length);

		animancer.Play(disappearAnim);
		await UniTask.WaitForSeconds(disappearAnim.length);

		spriteRenderer.enabled = false;

		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!circleCollider2D.isTrigger || !other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		grunt.Teleport(warpDestination.transform);

		if (warpType is WarpType.Red) {
			Deactivate();
		}
	}

	protected override void OnDestroy() {
		base.OnDestroy();

		GameManager.instance.gridObjectz.Remove(this);
	}
}

public enum WarpType {
	Red,
	Blue,
	Green,
	Yellow,
}
}
