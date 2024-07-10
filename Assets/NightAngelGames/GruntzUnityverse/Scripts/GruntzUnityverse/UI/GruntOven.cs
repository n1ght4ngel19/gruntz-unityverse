using System;
using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.UI {
public class GruntOven : MonoBehaviour {
	public GameManager gameManager;

	public bool filled;

	public AnimationClip bakeAnim;

	public Sprite emptySprite;

	public Sprite filledSprite;

	public AnimancerComponent animancer;

	private void Start() {
		gameManager = FindFirstObjectByType<GameManager>();
	}

	public async void Bake() {
		animancer.enabled = true;
		await animancer.Play(bakeAnim);

		filled = true;
		enabled = true;
	}

	public void RemoveOrPlaceBack() {
		animancer.enabled = false;

		// When not empty and can remove, remove
		if ((filled && enabled) && !gameManager.selector.placingGrunt) {
			filled = false;
			enabled = false;
			GetComponent<SpriteRenderer>().sprite = emptySprite;

			GameCursor.instance.SwapCursor(AnimationManager.instance.cursorFlailingGrunt);
			gameManager.selector.placingGrunt = true;

			return;
		}

		// When empty and can place back, place back
		if ((!filled && !enabled) && gameManager.selector.placingGrunt) {
			filled = true;
			enabled = true;

			GetComponent<SpriteRenderer>().sprite = filledSprite;

			GameCursor.instance.SwapCursor(AnimationManager.instance.cursorDefault);
			gameManager.selector.placingGrunt = false;
		}
	}
}
}
