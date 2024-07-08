using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class Bridge : GridObject {
	[BoxGroup("Bridge Data")]
	[DisableIf(nameof(isInstance))]
	public bool isDeathBridge;

	[BoxGroup("Bridge Data")]
	[DisableIf(nameof(isInstance))]
	public bool raised;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip raiseAnim;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip lowerAnim;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animancer;

	public override void Setup() {
		base.Setup();

		animancer ??= GetComponent<AnimancerComponent>();
		node.isWater = !raised;
	}

	public async void Toggle() {
		AnimationClip toPlay = raised ? lowerAnim : raiseAnim;

		animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		raised = !raised;

		switch (node.tileType) {
			case TileType.Water:
				node.isWater = !raised;

				break;
			case TileType.Fire:
				node.isFire = !raised;

				break;
		}

		// Check if a Grunt is standing no the Bridge when it's lowered
		if (node.gruntOnNode != null && !node.gruntOnNode.between) {
			// Kill the Grunt if it doesn't have a Toob or Wingz equipped
			if (node.isWater && node.gruntOnNode.tool is not Wingz or Toob) {
				node.gruntOnNode.Die(AnimationManager.instance.sinkDeathAnimation, false, false);
			}

			if (node.isFire && node.gruntOnNode.tool is not Wingz) {
				node.gruntOnNode.Die(AnimationManager.instance.burnDeathAnimation, false, false);
			}
		}
	}
}
}
