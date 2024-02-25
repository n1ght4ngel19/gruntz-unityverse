using System.Collections.Generic;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Objectz.Interactablez {
public class Hole : GridObject, IObjectHolder, IInteractable, IAnimatable {
	public bool open;

	// --------------------------------------------------
	// IObjectHolder
	// --------------------------------------------------

	#region IObjectHolder
	[Header("IObjectHolder")]
	[field: SerializeField] public LevelItem HeldItem { get; set; }
	[field: SerializeField] public Hazard HiddenHazard { get; set; }

	public void RevealHidden(bool isSceneLoaded) {
		if (HeldItem != null) {
			Addressables.InstantiateAsync(HeldItem, GameObject.Find("Itemz").transform).Completed += _ => {
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
			"Shovel",
		};
	}

	public async void Interact() {
		Animancer.Play(dirtAnimation);

		await UniTask.WaitForSeconds(0.25f);

		open = !open;
		node.isBlocked = open;
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

	public AnimationClip dirtAnimation;
}
}
