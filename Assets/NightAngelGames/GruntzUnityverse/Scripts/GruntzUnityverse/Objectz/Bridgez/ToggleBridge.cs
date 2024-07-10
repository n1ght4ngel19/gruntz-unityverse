using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class ToggleBridge : GridObject {
	[BoxGroup("Bridge Data")]
	public float delay;

	[BoxGroup("Bridge Data")]
	public float duration;

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

	private bool _recurse;

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

		// Check if a Grunt is standing no the Bridge when it's lowered
		if (node.grunt != null && !node.grunt.between) {
			// Kill the Grunt if it doesn't have a Toob or Wingz equipped
			if (node.isWater && node.grunt.tool is not Wingz or Toob) {
				node.grunt.Die(AnimationManager.instance.sinkDeathAnimation, false, false);
			}

			if (node.isFire && node.grunt.tool is not Wingz) {
				node.grunt.Die(AnimationManager.instance.burnDeathAnimation, false, false);
			}
		}

		await UniTask.WaitForSeconds(duration);

		if (_recurse) {
			Toggle();
		}
	}

	// --------------------------------------------------
	// Overrides
	// --------------------------------------------------

	public override void Activate() {
		_recurse = true;
	}

	public override void Deactivate() {
		_recurse = false;
	}

	protected override void AssignNodeValues() {
		node.isWater = !raised;
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override async void Start() {
		base.Start();

		animancer = GetComponent<AnimancerComponent>();

		await UniTask.WaitForSeconds(delay);

		Toggle();
	}
}
}
