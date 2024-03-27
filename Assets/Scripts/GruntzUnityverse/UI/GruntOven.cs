﻿using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.UI {
public class GruntOven : MonoBehaviour {
	public bool filled;
	public AnimationClip bakeAnim;
	public Sprite emptySprite;
	public Sprite filledSprite;

	public AnimancerComponent animancer;

	public async void Bake() {
		animancer.enabled = true;
		await animancer.Play(bakeAnim);

		filled = true;
		enabled = true;
	}

	public void RemoveOrPlaceBack() {
		animancer.enabled = false;

		// When not empty and can remove, remove
		if ((filled && enabled) && !GameManager.instance.selector.placingGrunt) {
			Debug.Log("Remove");

			filled = false;
			enabled = false;
			GetComponent<SpriteRenderer>().sprite = emptySprite;
			// GetComponent<SpriteRenderer>().enabled = false;

			GameCursor.instance.SwapCursor(AnimationManager.instance.cursorFlailingGrunt);
			GameManager.instance.selector.placingGrunt = true;

			return;
		}

		// When empty and can place back, place back
		if ((!filled && !enabled) && GameManager.instance.selector.placingGrunt) {
			Debug.Log("Put back");

			filled = true;
			enabled = true;

			GetComponent<SpriteRenderer>().sprite = filledSprite;
			// GetComponent<SpriteRenderer>().enabled = true;

			GameCursor.instance.SwapCursor(AnimationManager.instance.cursorDefault);
			GameManager.instance.selector.placingGrunt = false;
		}
	}
}
}