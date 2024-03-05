using System.Collections;
using System.Linq;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class SecretObject : MonoBehaviour {
	public float delay;
	public float duration;

	public IEnumerator ToggleOn() {
		if (!TryGetComponent(out GridObject go)) {
			Debug.LogError($"No GridObject assigned to SecretObject: {name}!");

			yield break;
		}

		Node node = Level.Instance.levelNodes.First(n => n.location2D == Vector2Int.RoundToInt(transform.position));
		bool initiallyBlocked = node.isBlocked;
		bool initiallyWater = node.isWater;
		bool initiallyFire = node.isFire;
		bool initiallyVoid = node.isVoid;

		yield return new WaitForSeconds(delay);

		gameObject.SetActive(true);
		go.Setup();

		yield return new WaitForSeconds(duration);

		go.node.isBlocked = initiallyBlocked;
		go.node.isWater = initiallyWater;
		go.node.isFire = initiallyFire;
		go.node.isVoid = initiallyVoid;

		gameObject.SetActive(false);
	}
}
}
