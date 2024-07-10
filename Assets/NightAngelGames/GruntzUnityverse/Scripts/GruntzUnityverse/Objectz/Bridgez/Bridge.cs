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
	public bool raised;

	[BoxGroup("Bridge Data")]
	[DisableIf(nameof(isInstance))]
	public bool isDeathBridge;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip raiseAnim;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip lowerAnim;

	private AnimationClip animToPlay => raised ? lowerAnim : raiseAnim;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animancer;

	public async void Toggle() {
		animancer.Play(animToPlay);
		await UniTask.WaitForSeconds(animToPlay.length);

		raised = !raised;

		switch (node.tileType) {
			case TileType.Water:
				node.isWater = !raised;

				break;
			case TileType.Fire:
				node.isFire = !raised;

				break;
		}

		if (node.grunt == null || node.grunt.between) {
			return;
		}

		if (node.isWater && node.grunt.tool is not Wingz or Toob) {
			node.grunt.Die(AnimationManager.instance.sinkDeathAnimation, false, false);
		}

		if (node.isFire && node.grunt.tool is not Wingz) {
			node.grunt.Die(AnimationManager.instance.burnDeathAnimation, false, false);
		}
	}

	protected override void AssignNodeValues() {
		node.isWater = !raised;
	}

	protected override void Start() {
		base.Start();

		animancer = GetComponent<AnimancerComponent>();
	}
}
}
