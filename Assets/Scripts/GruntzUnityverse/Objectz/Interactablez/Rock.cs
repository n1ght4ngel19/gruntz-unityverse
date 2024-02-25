using System.Collections.Generic;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Misc;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

	public void RevealHidden(bool isSceneLoaded) {
		if (HeldItem != null) {
			string parentName = HeldItem switch {
				LevelTool => "Toolz",
				LevelToy => "Toyz",
				LevelPowerup => "Powerupz",
				Coin => "Coinz",
				Helpbox => "Helpboxez",
				_ => "Misc",
			};

			Addressables.InstantiateAsync(HeldItem, GameObject.Find(parentName).transform).Completed += _ => {
				HeldItem.transform.position = transform.position;
			};
		}

		if (HiddenHazard != null) {
			Addressables.InstantiateAsync(HiddenHazard, GameObject.Find("Hazardz").transform).Completed += _ => {
				HiddenHazard.transform.position = transform.position;
			};
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
		transform.localScale *= 0.75f;
		transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		spriteRenderer.sortingLayerName = "AlwaysBottom";
		spriteRenderer.sortingOrder = 4;

		
		Animancer.Play(breakAnimation);

		await UniTask.WaitForSeconds(0.5f);

		enabled = false;
		// This also prevents removing the effect of other possible blocking objects at the same location
		node.isBlocked = actAsObstacle ? false : node.isBlocked;
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
}
}
