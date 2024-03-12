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
public class Rock : GridObject, IObjectHolder, IInteractable, IAnimatable {
	// --------------------------------------------------
	// IObjectHolder
	// --------------------------------------------------

	#region IObjectHolder
	[Header("IObjectHolder")]
	[field: SerializeField] public LevelItem HeldItem { get; set; }
	[field: SerializeField] public Hazard HiddenHazard { get; set; }
	[field: SerializeField] public Switch HiddenSwitch { get; set; }

	public void RevealHidden(bool isSceneLoaded) {
		if (HeldItem != null) {
			HeldItem.GetComponent<SpriteRenderer>().enabled = true;
			HeldItem.GetComponent<CircleCollider2D>().isTrigger = true;
		}

		if (HiddenHazard != null) {
			HiddenHazard.spriteRenderer.enabled = true;
			HiddenHazard.circleCollider2D.isTrigger = true;
			HiddenHazard.enabled = true;

			HiddenHazard.OnRevealed();
		}

		if (HiddenSwitch != null) {
			HiddenSwitch.spriteRenderer.enabled = true;
			HiddenSwitch.circleCollider2D.isTrigger = true;
			HiddenSwitch.enabled = true;
		}
	}
	#endregion

	// --------------------------------------------------
	// IInteractable
	// --------------------------------------------------

	#region IInteractable
	public List<string> CompatibleItemz {
		get => new List<string> {
			"Gauntletz",
		};
	}

	public async void Interact() {
		transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		spriteRenderer.sortingLayerName = "Default";
		spriteRenderer.sortingOrder = 4;

		await Animancer.Play(breakAnimation);

		// await UniTask.WaitForSeconds(0.5f);

		transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		spriteRenderer.sortingLayerName = "AlwaysBottom";
		spriteRenderer.sortingOrder = 4;
		enabled = false;

		// This also prevents removing the effect of other possible blocking objects at the same location
		node.isBlocked = isObstacle ? false : node.isBlocked;

		RevealHidden(gameObject.scene.isLoaded);
	}
	#endregion

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
			ball.enabled = false;
			await ball.Animancer.Play(ball.breakAnim);

			Destroy(ball.gameObject);
		}
	}
}
}
