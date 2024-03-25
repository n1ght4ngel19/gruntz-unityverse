using System.Collections.Generic;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Interfacez;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.Objectz.Interactablez {
/// <summary>
/// A Grunt-sized piece of rock that blocks the path, possibly hiding something under it.
/// </summary>
public class Rock : GridObject, IObjectHolder, IAnimatable {
	public override void Setup() {
		base.Setup();

		node.isBlocked = isObstacle;
	}

	// --------------------------------------------------
	// IObjectHolder
	// --------------------------------------------------

	#region IObjectHolder
	[Header("IObjectHolder")]
	[field: SerializeField] public LevelItem heldItem { get; set; }
	[field: SerializeField] public Hazard hiddenHazard { get; set; }
	[field: SerializeField] public Switch hiddenSwitch { get; set; }

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
		spriteRenderer.sortingOrder = 4;

		Animancer.Play(breakAnimation);

		await UniTask.WaitForSeconds(breakAnimation.length * 0.25f);

		spriteRenderer.sortingLayerName = "AlwaysBottom";
		spriteRenderer.sortingOrder = 4;

		await UniTask.WaitForSeconds(breakAnimation.length * 0.5f);

		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

		RevealHidden(gameObject.scene.isLoaded);

		// This also prevents removing the effect of other possible blocking objects at the same location
		node.isBlocked = isObstacle ? false : node.isBlocked;
	}

	// --------------------------------------------------
	// IAnimatable
	// --------------------------------------------------

	#region IAnimatable
	[Header("IAnimatable")]
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	#endregion

	public AnimationClip breakAnimation;

	private async void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out RollingBall ball)) {
			ball.moveSpeed *= 5;
			await ball.animancer.Play(ball.breakAnim);
			ball.enabled = false;

			ball.GetComponent<SpriteRenderer>().sortingLayerName = "AlwaysBottom";
			ball.GetComponent<SpriteRenderer>().sortingOrder = 6;
		}
	}
}
}
