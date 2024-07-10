using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class CrumbleBridge : GridObject {
	[BoxGroup("Bridge Data")]
	public int crumbleDelay;

	[BoxGroup("Bridge Data")]
	[DisableIf(nameof(isInstance))]
	public bool isDeathBridge;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip crumbleAnim;

	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animancer;

	private async void Crumble() {
		await UniTask.WaitForSeconds(crumbleDelay);

		animancer.Play(crumbleAnim);
		await UniTask.WaitForSeconds(crumbleAnim.length);

		switch (node.tileType) {
			case TileType.Water:
				node.isWater = true;

				break;
			case TileType.Fire:
				node.isFire = true;

				break;
		}

		if (node.grunt != null && !node.grunt.between) {
			if (node.isWater && node.grunt.tool is not Wingz or Toob) {
				node.grunt.Die(AnimationManager.instance.sinkDeathAnimation, leavePuddle: false, playBelow: false);
			}

			if (node.isFire && node.grunt.tool is not Wingz) {
				node.grunt.Die(AnimationManager.instance.burnDeathAnimation, leavePuddle: false, playBelow: false);
			}
		}

		gameManager.gridObjectz.Remove(this);

		Destroy(gameObject);
	}

	protected override void AssignNodeValues() {
		node.isWater = false;
	}

	protected override void UnAssignNodeValues() { }

	protected override void Start() {
		base.Start();

		animancer = GetComponent<AnimancerComponent>();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		Deactivate();

		Crumble();
	}
}
}
