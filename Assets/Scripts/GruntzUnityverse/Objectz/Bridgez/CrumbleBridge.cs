using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
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

		node.isWater = true;

		Destroy(gameObject, 0.5f);
	}
}
}
