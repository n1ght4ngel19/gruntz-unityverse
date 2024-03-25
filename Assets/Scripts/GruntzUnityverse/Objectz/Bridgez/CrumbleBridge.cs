using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class CrumbleBridge : GridObject {
	public bool isDeathBridge;

	public int crumbleDelay;

	public AnimationClip crumbleAnim;

	public AnimancerComponent animancer;

	public override void Setup() {
		base.Setup();

		animancer ??= GetComponent<AnimancerComponent>();
		node.isWater = false;
	}

	private async void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt _) && !other.TryGetComponent(out RollingBall _)) {
			return;
		}

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

		// Check if a Grunt is standing no the Bridge when it's lowered
		if (node.gruntOnNode != null) {
			// Kill the Grunt if it doesn't have a Toob or Wingz equipped
			if (node.isWater && node.gruntOnNode.equippedTool is not Wingz or Toob) {
				node.gruntOnNode.Die(AnimationManager.instance.sinkDeathAnimation, false, false);
			}

			if (node.isFire && node.gruntOnNode.equippedTool is not Wingz) {
				node.gruntOnNode.Die(AnimationManager.instance.burnDeathAnimation, false, false);
			}
		}

		Destroy(gameObject, 0.5f);
	}
}
}
