using Animancer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
public abstract class Pyramid : GridObject {
	public AnimationClip raiseAnim;
	public AnimationClip lowerAnim;

	private AnimancerComponent Animancer => GetComponent<AnimancerComponent>();

	public virtual async void Toggle() {
		AnimationClip toPlay = isObstacle ? lowerAnim : raiseAnim;

		Animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		isObstacle = !isObstacle;
		node.isBlocked = isObstacle;
	}
}
}
