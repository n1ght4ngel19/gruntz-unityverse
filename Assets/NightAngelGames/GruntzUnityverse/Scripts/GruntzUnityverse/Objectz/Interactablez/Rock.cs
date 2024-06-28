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
public class Rock : GridObject, IObjectHolder {
	public AnimationClip breakAnimation;

	private AnimancerComponent animancer => GetComponent<AnimancerComponent>();

	public override void Setup() {
		base.Setup();

		node.isBlocked = isObstacle;
		node.hardCorner = isObstacle;
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
