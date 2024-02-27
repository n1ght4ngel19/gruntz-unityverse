using Animancer;
using Cysharp.Threading.Tasks;
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
		circleCollider2D.isTrigger = false;

		Animancer.Play(disappearAnim);
		await UniTask.WaitForSeconds(disappearAnim.length);

		Destroy(gameObject);
	}

	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
}
}
