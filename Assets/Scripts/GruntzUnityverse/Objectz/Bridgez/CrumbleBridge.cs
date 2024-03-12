using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class CrumbleBridge : GridObject {
	public bool isDeathBridge;

	public int crumbleDelay;

	public AnimationClip crumbleAnim;

	private AnimancerComponent Animancer => GetComponent<AnimancerComponent>();

	public override void Setup() {
		base.Setup();

		node.isWater = false;
	}

	private async void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt _) && !other.TryGetComponent(out RollingBall _)) {
			return;
		}

		await UniTask.WaitForSeconds(crumbleDelay);

		Animancer.Play(crumbleAnim);
		await UniTask.WaitForSeconds(crumbleAnim.length);

		node.isWater = true;

		// ? Allow some time for node to change state  
		await UniTask.WaitForSeconds(0.5f);

		Destroy(gameObject);
	}
}
}
