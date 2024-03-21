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

	public override void Setup() {
		base.Setup();

		animancer = GetComponent<AnimancerComponent>();
	}

	public virtual async void Toggle() {
		AnimationClip toPlay = isObstacle ? lowerAnim : raiseAnim;

		animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		isObstacle = !isObstacle;
		node.isBlocked = isObstacle;
		spriteRenderer.sortingLayerName = isObstacle ? "HighObjectz" : "AlwaysBottom";

		Grunt grunt = FindObjectsByType<Grunt>(FindObjectsSortMode.None).FirstOrDefault(gr => gr.node == node);

		if (grunt != null && node.isBlocked) {
			grunt.Die(AnimationManager.instance.explodeDeathAnimation, false, false);
		}
	}
}
}
