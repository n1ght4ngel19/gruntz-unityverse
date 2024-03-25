using Animancer;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Misc {
public class Flag : MonoBehaviour {
	public AnimationClip wavingAnim;
	private AnimancerComponent animancer => GetComponent<AnimancerComponent>();

	public void PlayAnim() {
		animancer.Play(wavingAnim);
	}
}
}
