using System.Linq;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Blocker : GridObject {
	public override void Setup() {
		FindObjectsByType<Node>(FindObjectsSortMode.None)
			.First(n => n.location2D.Equals(Vector2Int.RoundToInt(transform.position)))
			.isBlocked = true;
	}

	private void Awake() {
		GetComponent<SpriteRenderer>().enabled = false;
		Destroy(gameObject);
	}
}
}
