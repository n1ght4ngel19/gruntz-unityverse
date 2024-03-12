using Animancer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class Bridge : GridObject {
	public bool isDeathBridge;
	public bool raised;

	public AnimationClip raiseAnim;
	public AnimationClip lowerAnim;
	public AnimancerComponent animancer;

	public override void Setup() {
		base.Setup();

		animancer = GetComponent<AnimancerComponent>();

		node.isWater = isWater;
	}


	public async void Toggle() {
		Debug.Log("Toggle");
		AnimationClip toPlay = raised ? lowerAnim : raiseAnim;

		animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		raised = !raised;
		node.isWater = !raised;
	}
}
}
