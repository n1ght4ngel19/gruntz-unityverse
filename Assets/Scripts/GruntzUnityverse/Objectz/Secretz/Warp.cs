using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class Warp : GridObject, IAnimatable {
	public Transform warpDestination;
	public int duration;

	public AnimationClip appearAnim;
	public AnimationClip disappearAnim;
	public AnimationClip swirlingAnim;

	public async void Activate() {
		node.circleCollider2D.isTrigger = false;

		circleCollider2D.isTrigger = true;
		spriteRenderer.enabled = true;

		await Animancer.Play(appearAnim);

		circleCollider2D.isTrigger = true;

		Animancer.Play(swirlingAnim);

		// If the duration is not positive, the warp will be active indefinitely (or until a Grunt steps on it).
		if (duration <= 0) {
			return;
		}

		await UniTask.WaitForSeconds(duration);

		Deactivate();
	}

	public async void Deactivate() {
		node.circleCollider2D.isTrigger = true;

		circleCollider2D.isTrigger = false;

		Animancer.Play(disappearAnim);
		await UniTask.WaitForSeconds(disappearAnim.length);

		spriteRenderer.enabled = false;
		// Destroy(gameObject);
	}

	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }

	private void OnTriggerEnter2D(Collider2D other) {
		if (circleCollider2D.isTrigger && other.TryGetComponent(out Grunt grunt)) {
			StartCoroutine(grunt.Teleport(warpDestination.transform, this));
		}
	}
}
}
