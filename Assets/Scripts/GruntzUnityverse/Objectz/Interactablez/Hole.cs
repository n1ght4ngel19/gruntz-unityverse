using System.Collections.Generic;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Interfacez;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Objectz.Interactablez {
public class Hole : GridObject, IObjectHolder, IInteractable, IAnimatable {
	public bool open;
	public Sprite openSprite;
	public Sprite filledSprite;

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
			"Shovel",
		};
	}

	public async void Interact() {
		// Animancer.Play(dirtAnimation);

		await UniTask.WaitForSeconds(0.5f);

		open = !open;
		spriteRenderer.sprite = open ? openSprite : filledSprite;

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

}
}
