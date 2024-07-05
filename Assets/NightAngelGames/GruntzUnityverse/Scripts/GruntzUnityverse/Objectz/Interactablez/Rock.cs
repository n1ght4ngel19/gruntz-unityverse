using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Switchez;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.Objectz.Interactablez {
/// <summary>
/// A Grunt-sized piece of rock that blocks the path, possibly hiding something under it.
/// </summary>
public class Rock : GridObject {
	[DisableIf(nameof(isInstance))]
	public AnimationClip breakAnimation;

	private AnimancerComponent animancer => GetComponent<AnimancerComponent>();

	public override void Setup() {
		base.Setup();

		node.isBlocked = isObstacle;
		node.hardCorner = isObstacle;
	}

	// --------------------------------------------------
	// Held Objectz
	// --------------------------------------------------

	#region Held Objectz
	[BoxGroup("Held Objectz")]
	[ReadOnly]
	public LevelItem heldItem;

	[BoxGroup("Held Objectz")]
	[ReadOnly]
	public Hazard hiddenHazard;

	[BoxGroup("Held Objectz")]
	[ReadOnly]
	public Switch hiddenSwitch;

	public void RevealHidden(bool isSceneLoaded) {
		if (heldItem != null) {
			heldItem.GetComponent<SpriteRenderer>().enabled = true;
			heldItem.GetComponent<CircleCollider2D>().isTrigger = true;
		}

		if (hiddenHazard != null) {
			hiddenHazard.spriteRenderer.enabled = true;
			hiddenHazard.circleCollider2D.isTrigger = true;
			hiddenHazard.enabled = true;

			hiddenHazard.OnRevealed();
		}

		if (hiddenSwitch != null) {
			hiddenSwitch.spriteRenderer.enabled = true;
			hiddenSwitch.circleCollider2D.isTrigger = true;
			hiddenSwitch.enabled = true;
		}
	}
	#endregion

	public async void Break() {
		enabled = false;

		transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		spriteRenderer.sortingLayerName = "Default";
		spriteRenderer.sortingOrder = 5;

		animancer.Play(breakAnimation);
		await UniTask.WaitForSeconds(breakAnimation.length * 0.25f);

		spriteRenderer.sortingLayerName = "AlwaysBottom";
		spriteRenderer.sortingOrder = 5;

		await UniTask.WaitForSeconds(breakAnimation.length * 0.5f);

		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

		RevealHidden(gameObject.scene.isLoaded);

		node.isBlocked = false;
		node.hardCorner = false;
	}
}
}
