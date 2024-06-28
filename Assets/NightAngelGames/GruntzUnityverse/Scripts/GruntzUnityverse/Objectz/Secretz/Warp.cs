using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class Warp : GridObject {
	[BoxGroup("Warp Data")]
	public WarpType warpType;

	[BoxGroup("Warp Data")]
	[Required]
	public Transform warpDestination;

	[BoxGroup("Warp Data")]
	public int duration;

	[BoxGroup("Animation")]
	[Required]
	public Animator animator;

	[BoxGroup("Animation")]
	[Required]
	public AnimancerComponent animancer;

	[BoxGroup("Animation")]
	[Required]
	public AnimationClip appearAnim;

	[BoxGroup("Animation")]
	[Required]
	public AnimationClip disappearAnim;

	[BoxGroup("Animation")]
	[Required]
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

		Deactivate();
	}

	public async void Deactivate() {
		circleCollider2D.isTrigger = false;

		await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpEnterAnim.length);

		animancer.Play(disappearAnim);
		await UniTask.WaitForSeconds(disappearAnim.length);

		spriteRenderer.enabled = false;

		Destroy(gameObject, 1f);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!circleCollider2D.isTrigger || !other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		grunt.Teleport(warpDestination.transform);

		Deactivate();
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
