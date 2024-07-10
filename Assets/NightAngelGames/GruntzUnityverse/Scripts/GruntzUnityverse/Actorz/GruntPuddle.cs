using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
public class GruntPuddle : GridObject {
	public int gooAmount;

	[BoxGroup("Animation")]
	public AnimationClip bubblingAnim;

	[BoxGroup("Animation")]
	public AnimationClip appearAnim;

	[BoxGroup("Animation")]
	public AnimationClip disappearAnim;

	[BoxGroup("Animation")]
	public AnimancerComponent animancer;

	protected override void Start() {
		base.Start();

		animancer = GetComponent<AnimancerComponent>();

		animancer.Play(bubblingAnim);
	}

	public async void Appear() {
		await animancer.Play(appearAnim);

		animancer.Play(bubblingAnim);
	}

	public async void Disappear() {
		await animancer.Play(disappearAnim);

		gameManager.gridObjectz.Remove(this);

		Destroy(gameObject, 0.1f);
	}
}
}
