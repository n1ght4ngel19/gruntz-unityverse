using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;
using GruntzUnityverse.Pathfinding;

namespace GruntzUnityverse.Objectz.Secretz {
public class SecretObject : MonoBehaviour {
	public float delay;
	public float duration;

	public IEnumerator Toggle() {
		if (!TryGetComponent(out GridObject gridObject)) {
			Debug.LogError($"SecretObject is not assigned to a GridObject: {name}!");

			yield break;
		}

		if (!gameObject.activeSelf) {
			Node node = Level.instance.levelNodes.First(n => n.location2D == Vector2Int.RoundToInt(transform.position));
			bool initiallyBlocked = node.isBlocked;
			bool initiallyWater = node.isWater;
			bool initiallyFire = node.isFire;
			bool initiallyVoid = node.isVoid;

			yield return new WaitForSeconds(delay);

			gameObject.SetActive(true);
			gridObject.Setup();

			TryKillGruntOnTop();

			yield return new WaitForSeconds(duration);

			gridObject.node.isBlocked = initiallyBlocked;
			gridObject.node.isWater = initiallyWater;
			gridObject.node.isFire = initiallyFire;
			gridObject.node.isVoid = initiallyVoid;

			gameObject.SetActive(false);

			TryKillGruntOnTop();
		} else {
			yield return new WaitForSeconds(delay);

			gameObject.SetActive(false);

			TryKillGruntOnTop();


			yield return new WaitForSeconds(duration);

			gameObject.SetActive(true);

			TryKillGruntOnTop();
		}

		// bool initiallyBlocked = gridObject.node.isBlocked;
		// bool initiallyWater = gridObject.node.isWater;
		// bool initiallyFire = gridObject.node.isFire;
		// bool initiallyVoid = gridObject.node.isVoid;
		//
		// yield return new WaitForSeconds(delay);
		//
		// gameObject.SetActive(false);
		//
		// yield return new WaitForSeconds(duration);
		//
		// gridObject.node.isBlocked = initiallyBlocked;
		// gridObject.node.isWater = initiallyWater;
		// gridObject.node.isFire = initiallyFire;
		// gridObject.node.isVoid = initiallyVoid;
		//
		// gameObject.SetActive(true);
	}

	private void TryKillGruntOnTop() {
		Grunt gruntOnTop = GameManager.instance.allGruntz.FirstOrDefault(
			gr => gr.node == Level.instance.levelNodes.First(n => n.location2D == Vector2Int.RoundToInt(transform.position) && !gr.between)
		);

		if (gruntOnTop == null || gruntOnTop.between) {
			return;
		}

		if (!TryGetComponent(out GridObject gridObject)) {
			return;
		}

		if (gridObject.node.isFire) {
			gruntOnTop.Die(AnimationManager.instance.burnDeathAnimation, false, false);

			return;
		}

		if (gridObject.node.isVoid) {
			gruntOnTop.Die(AnimationManager.instance.fallDeathAnimation, false, false);

			return;
		}

		if (gridObject.node.isWater) {
			gruntOnTop.Die(AnimationManager.instance.sinkDeathAnimation, false, false);

			return;
		}

		if (gridObject.node.isBlocked) {
			gruntOnTop.Die(AnimationManager.instance.explodeDeathAnimation, false, false);
		}
	}
}
}
