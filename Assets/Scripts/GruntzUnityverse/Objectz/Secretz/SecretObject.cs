using System.Collections;
using System.Linq;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

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
			Node node = Level.Instance.levelNodes.First(n => n.location2D == Vector2Int.RoundToInt(transform.position));
			bool initiallyBlocked = node.isBlocked;
			bool initiallyWater = node.isWater;
			bool initiallyFire = node.isFire;
			bool initiallyVoid = node.isVoid;

			yield return new WaitForSeconds(delay);

			gameObject.SetActive(true);
			gridObject.Setup();

			yield return new WaitForSeconds(duration);

			gridObject.node.isBlocked = initiallyBlocked;
			gridObject.node.isWater = initiallyWater;
			gridObject.node.isFire = initiallyFire;
			gridObject.node.isVoid = initiallyVoid;

			gameObject.SetActive(false);
		} else {
			bool initiallyBlocked = gridObject.node.isBlocked;
			bool initiallyWater = gridObject.node.isWater;
			bool initiallyFire = gridObject.node.isFire;
			bool initiallyVoid = gridObject.node.isVoid;

			yield return new WaitForSeconds(delay);

			gameObject.SetActive(false);

			yield return new WaitForSeconds(duration);

			gridObject.node.isBlocked = initiallyBlocked;
			gridObject.node.isWater = initiallyWater;
			gridObject.node.isFire = initiallyFire;
			gridObject.node.isVoid = initiallyVoid;

			gameObject.SetActive(true);
		}
	}
}
}
