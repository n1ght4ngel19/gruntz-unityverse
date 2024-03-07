using System.Security.Cryptography;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class CrumbleBridge : GridObject, IAnimatable {
	public bool isDeathBridge;
	public int crumbleDelay;
	[field: SerializeField] public AnimationClip CrumbleAnim { get; set; }

	public override void Setup() {
		base.Setup();

		node.isWater = false;
	}

	// --------------------------------------------------
	// IAnimatable
	// --------------------------------------------------
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }

	private async void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		await UniTask.WaitForSeconds(crumbleDelay);

		await Animancer.Play(CrumbleAnim);

		node.isWater = true;

		// Allow some time for node to change state  
		await UniTask.WaitForSeconds(0.5f);

		Destroy(gameObject);
	}
}
}
