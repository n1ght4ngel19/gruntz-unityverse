using System.Linq;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
public abstract class LevelItem : GridObject {
	[BoxGroup("Item Data")]
	public string displayName;

	[BoxGroup("Item Data")]
	[HideIf(nameof(isInstance))]
	public string codename;

	[BoxGroup("Item Data")]
	public bool hideInObject;

	[Foldout("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip rotatingAnim;

	[Foldout("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimationClip pickupAnim;

	[Foldout("Animation")]
	[DisableIf(nameof(isInstance))]
	public AnimancerComponent animancer;

	private void SetupRockOnTop() {
		Rock rockOnTop = FindObjectsByType<Rock>(FindObjectsSortMode.None)
			.FirstOrDefault(r => r.location2D == location2D);

		if (rockOnTop == null) {
			return;
		}

		rockOnTop.hiddenItem = this;

		spriteRenderer.enabled = false;
		enabled = false;

		Deactivate();
	}

	private void SetupHole() {
		Hole hole = FindObjectsByType<Hole>(FindObjectsSortMode.None)
			.FirstOrDefault(h => h.location2D == location2D);

		if (hole == null) {
			return;
		}

		if (hole.open) {
			Debug.LogError("An open hole cannot have a hidden item!");

			return;
		}

		hole.heldItem = this;

		spriteRenderer.enabled = false;
		enabled = false;

		Deactivate();
	}

	protected virtual async void Pickup(Grunt targetGrunt) {
		targetGrunt.GoToState(StateHandler.State.Committed);
		targetGrunt.enabled = false;

		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void Start() {
		base.Start();

		animancer = GetComponent<AnimancerComponent>();

		SetupRockOnTop();

		SetupHole();

		animancer.Play(rotatingAnim);
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		spriteRenderer.enabled = false;

		Pickup(node.grunt);
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		spriteRenderer.enabled = true;
	}

	public void LocalizeName(string newDisplayName) {
		displayName = newDisplayName;
	}
}
}
