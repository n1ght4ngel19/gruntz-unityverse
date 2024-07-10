using Animancer;
using Cysharp.Threading.Tasks;
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
	[BoxGroup("Animation Data")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip breakAnimation;

	[BoxGroup("Animation Data")]
	public AnimancerComponent animancer;

	[BoxGroup("Rock Objectz")]
	[ReadOnly]
	public LevelItem hiddenItem;

	[BoxGroup("Rock Objectz")]
	[ReadOnly]
	public Hazard hiddenHazard;

	[BoxGroup("Rock Objectz")]
	[ReadOnly]
	public Switch hiddenSwitch;

	public void RevealHidden() {
		if (hiddenItem != null) {
			hiddenItem.GetComponent<SpriteRenderer>().enabled = true;
			hiddenItem.GetComponent<CircleCollider2D>().isTrigger = true;
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

		RevealHidden();

		node.isBlocked = false;
		node.hardCorner = false;
	}

	protected override void Start() {
		base.Start();

		animancer = GetComponent<AnimancerComponent>();
	}
}
}
