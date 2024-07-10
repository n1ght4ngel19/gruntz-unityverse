using System;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;
using NaughtyAttributes;

namespace GruntzUnityverse.Objectz.Secretz {
public class SecretObject : MonoBehaviour {
	public float delay;

	public float duration;

	[ReadOnly]
	public GridObject controlledObject;

	public async void Toggle() {
		if (!gameObject.activeSelf) {
			await UniTask.WaitForSeconds(delay);

			bool initiallyBlocked = controlledObject.node.isBlocked;
			bool initiallyWater = controlledObject.node.isWater;
			bool initiallyFire = controlledObject.node.isFire;
			bool initiallyVoid = controlledObject.node.isVoid;
			gameObject.SetActive(true);

			TryKillGruntOnTop();

			await UniTask.WaitForSeconds(duration);

			controlledObject.node.isBlocked = initiallyBlocked;
			controlledObject.node.isWater = initiallyWater;
			controlledObject.node.isFire = initiallyFire;
			controlledObject.node.isVoid = initiallyVoid;
			gameObject.SetActive(false);

			TryKillGruntOnTop();
		} else {
			await UniTask.WaitForSeconds(delay);

			gameObject.SetActive(false);

			TryKillGruntOnTop();

			await UniTask.WaitForSeconds(duration);

			gameObject.SetActive(true);

			TryKillGruntOnTop();
		}
	}

	private void TryKillGruntOnTop() {
		Grunt gruntOnTop = controlledObject.node.grunt;

		if (gruntOnTop == null || gruntOnTop.between) {
			return;
		}

		if (controlledObject.node.isFire) {
			gruntOnTop.Die(AnimationManager.instance.burnDeathAnimation, false, false);

			return;
		}

		if (controlledObject.node.isVoid) {
			gruntOnTop.Die(AnimationManager.instance.fallDeathAnimation, false, false);

			return;
		}

		if (controlledObject.node.isWater) {
			gruntOnTop.Die(AnimationManager.instance.sinkDeathAnimation, false, false);

			return;
		}

		if (controlledObject.node.isBlocked) {
			gruntOnTop.Die(AnimationManager.instance.explodeDeathAnimation, false, false);
		}
	}

	private void Start() { }

	private void Update() {
		if (controlledObject == null) {
			controlledObject = GetComponent<GridObject>();

			enabled = false;
		}
	}
}
}
