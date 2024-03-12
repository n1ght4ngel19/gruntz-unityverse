using Animancer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class Bridge : GridObject {
	public bool isDeathBridge;
	public bool raised;

	public AnimationClip raiseAnim;
	public AnimationClip lowerAnim;

	private AnimancerComponent Animancer => GetComponent<AnimancerComponent>();

	public override void Setup() {
		base.Setup();

		node.isWater = isWater;
	}

	public async void Toggle() {
		AnimationClip toPlay = raised ? lowerAnim : raiseAnim;

		Animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		raised = !raised;
		node.isWater = !raised;
	}
}
}
