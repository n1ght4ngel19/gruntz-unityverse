using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
public class SilverPyramid : Pyramid {
	public float delay;
	public float duration;

	public override async void Toggle() {
		if (duration <= 0) {
			return;
		}

		await UniTask.WaitForSeconds(delay);

		AnimationClip toPlay1 = isObstacle ? lowerAnim : raiseAnim;
		animancer.Play(toPlay1);

		await UniTask.WaitForSeconds(toPlay1.length);

		isObstacle = !isObstacle;
		node.isBlocked = !node.isBlocked;
		spriteRenderer.sortingLayerName = isObstacle ? "HighObjectz" : "AlwaysBottom";

		if (gruntOnTop != null && node.isBlocked && !gruntOnTop.between) {
			gruntOnTop.Die(AnimationManager.instance.explodeDeathAnimation, false, false);
		}

		await UniTask.WaitForSeconds(duration);

		AnimationClip toPlay2 = isObstacle ? lowerAnim : raiseAnim;
		animancer.Play(toPlay2);

		await UniTask.WaitForSeconds(toPlay2.length);

		isObstacle = !isObstacle;
		node.isBlocked = !node.isBlocked;
		spriteRenderer.sortingLayerName = isObstacle ? "HighObjectz" : "AlwaysBottom";

		if (gruntOnTop != null && node.isBlocked && !gruntOnTop.between) {
			gruntOnTop.Die(AnimationManager.instance.explodeDeathAnimation, false, false);
		}
	}
}
}
