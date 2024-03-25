using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Objectz {
public class CreatorPad : GridObject {
	public AnimationClip padAnim;
	private AnimancerComponent animancer => GetComponent<AnimancerComponent>();

	private void Start() {
		animancer.Play(padAnim);
	}

	private void OnTryPlaceGrunt() {
		if (GameManager.instance.selector.placingGrunt && GameManager.instance.selector.node == node) {
			GameCursor.instance.SwapCursor(AnimationManager.instance.cursorDefault);
			GameManager.instance.selector.placingGrunt = false;
			GameCursor.instance.spriteRenderer.material = GameCursor.instance.defaultMaterial;

			Addressables.InstantiateAsync("PG_BareHandz").Completed += handle => {
				handle.Result.transform.position = new Vector3(location2D.x, location2D.y, 0);

				GameManager.instance.allGruntz.Add(handle.Result.GetComponent<Grunt>());

				if (handle.Result.CompareTag("PlayerGrunt")) {
					GameManager.instance.playerGruntz.Add(handle.Result.GetComponent<Grunt>());
				} else {
					GameManager.instance.dizgruntled.Add(handle.Result.GetComponent<Grunt>());
				}
			};
		}
	}
}
}
