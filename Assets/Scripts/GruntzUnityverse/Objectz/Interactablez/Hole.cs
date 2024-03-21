using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Interfacez;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
public class Hole : GridObject, IObjectHolder {
	public bool open;
	public Sprite openSprite;
	public Sprite filledSprite;
	public GameObject dirt;

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

			node.isBlocked = false;
			node.isWater = false;
			node.isFire = false;
			node.isVoid = false;

			circleCollider2D.isTrigger = false;
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
			Debug.Log("Grunt collision");

			if (open && grunt.equippedTool is not Wingz) {
				grunt.Die(AnimationManager.instance.holeDeathAnimation, leavePuddle: false, playBelow: false);
			}
		}
	}
}
}
