using System.Collections;
using Animancer;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class ToggleBridge : GridObject {
	public bool isDeathBridge;
	public bool isRaised;

	public float delay;
	public float duration;

	public AnimationClip raiseAnim;
	public AnimationClip lowerAnim;
	public AnimancerComponent animancer;

	public override void Setup() {
		base.Setup();

		animancer ??= GetComponent<AnimancerComponent>();
		node.isWater = isWater;
	}

	private IEnumerator Start() {
		yield return new WaitForSeconds(delay);

		StartCoroutine(Toggle());
	}

	public IEnumerator Toggle() {
		AnimationClip toPlay = isRaised ? lowerAnim : raiseAnim;

		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length);

		isRaised = !isRaised;
		node.isWater = !isRaised;

		yield return new WaitForSeconds(duration);

		StartCoroutine(Toggle());
	}
}
}
