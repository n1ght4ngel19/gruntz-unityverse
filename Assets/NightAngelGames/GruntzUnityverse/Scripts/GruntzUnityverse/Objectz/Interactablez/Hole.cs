using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Switchez;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
public class Hole : GridObject {
	// --------------------------------------------------
	// Hole Data
	// --------------------------------------------------

	#region Hole Data
	[BoxGroup("Hole Data")]
	[DisableIf(nameof(isInstance))]
	public bool open;

	[BoxGroup("Hole Data")]
	[DisableIf(nameof(isInstance))]
	public Sprite openSprite;

	[BoxGroup("Hole Data")]
	[DisableIf(nameof(isInstance))]
	public Sprite filledSprite;

	[BoxGroup("Hole Data")]
	[DisableIf(nameof(isInstance))]
	public GameObject dirt;
	#endregion

	// --------------------------------------------------
	// Hidden Objectz
	// --------------------------------------------------

	#region Hidden Objectz
	[BoxGroup("Hidden Objectz")]
	[ReadOnly]
	public LevelItem heldItem;

	[BoxGroup("Hidden Objectz")]
	[ReadOnly]
	public Hazard hiddenHazard;

	[BoxGroup("Hidden Objectz")]
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

			node.isBlocked = false;
			node.isWater = false;
			node.isFire = false;
			node.isVoid = false;

			circleCollider2D.isTrigger = false;
			gameManager.gridObjectz.Remove(this);

			Destroy(gameObject);
		}
	}
	#endregion

	public void Dig() {
		open = !open;
		spriteRenderer.sprite = spriteRenderer.sprite == openSprite ? filledSprite : openSprite;
		dirt.GetComponent<AnimancerComponent>().Stop();

		RevealHidden(gameObject.scene.isLoaded);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			if (open && grunt.tool is not Wingz) {
				grunt.Die(AnimationManager.instance.holeDeathAnimation, leavePuddle: false, playBelow: false);
			}
		}
	}
}
}
