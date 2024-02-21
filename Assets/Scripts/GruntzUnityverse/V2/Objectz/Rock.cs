using System.Collections.Generic;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.V2.Itemz;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
/// <summary>
/// A Grunt-sized piece of rock that blocks the path and possibly holds an <see cref="LevelItem"/>.
/// </summary>
public class Rock : GridObject, IObjectHolder, IInteractable, IAnimatable {
	[field: SerializeField] public LevelItem HeldItem { get; set; }
	[field: SerializeField] public Animator Animator { get; set; }
	[field: SerializeField] public AnimancerComponent Animancer { get; set; }
	public AnimationClip breakAnimation;

	public void DropItem(bool isSceneLoaded) {
		if (!isSceneLoaded || HeldItem == null) {
			return;
		}

		Instantiate(HeldItem, transform.position, Quaternion.identity, GameObject.Find("Itemz").transform);
	}

	protected override void OnDestroy() {
		base.OnDestroy();

		DropItem(gameObject.scene.isLoaded);
	}

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

		Animancer.Play(breakAnimation);

		await UniTask.WaitForSeconds(0.5f);

		enabled = false;
		// This also prevents removing the effect of other possible blocking objects at the same location
		node.isBlocked = actAsObstacle ? false : node.isBlocked;
		// Destroy(gameObject);
	}
	#endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(Rock))]
[CanEditMultipleObjects]
public class RockV2Editor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		MonoBehaviour objectHolder = (Rock)target;

		if (GUILayout.Button("Destroy Rock")) {
			Destroy(objectHolder.gameObject);
		}
	}
}
#endif
}
