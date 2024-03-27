using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
public abstract class Pyramid : GridObject {
	public AnimationClip raiseAnim;
	public AnimationClip lowerAnim;
	public AnimancerComponent animancer;

	protected Grunt gruntOnTop => GameManager.instance.allGruntz.FirstOrDefault(gr => gr.node == node);

	public override void Setup() {
		base.Setup();

		animancer = GetComponent<AnimancerComponent>();

		node.isBlocked = isObstacle;
		node.hardCorner = isObstacle;
	}

	public virtual async void Toggle() {
		AnimationClip toPlay = isObstacle ? lowerAnim : raiseAnim;

		animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		isObstacle = !isObstacle;
		node.isBlocked = isObstacle;
		node.hardCorner = isObstacle;
		spriteRenderer.sortingLayerName = isObstacle ? "HighObjectz" : "AlwaysBottom";

		if (gruntOnTop != null && node.isBlocked) {
			gruntOnTop.Die(AnimationManager.instance.explodeDeathAnimation, false, false);
		}
	}
}
}
