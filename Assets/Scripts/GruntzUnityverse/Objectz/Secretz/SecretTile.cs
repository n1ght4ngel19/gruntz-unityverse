using System.Collections;
using System.Linq;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class SecretTile : GridObject {
	public float delay;
	public float duration;

	private bool _initiallyBlocked;
	private bool _initiallyWater;
	private bool _initiallyFire;
	private bool _initiallyVoid;

	public override void Setup() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		circleCollider2D = GetComponent<CircleCollider2D>();
		location2D = Vector2Int.RoundToInt(transform.position);

		node = FindObjectsByType<Node>(FindObjectsSortMode.None)
			.First(n => Vector2Int.RoundToInt(n.transform.position) == Vector2Int.RoundToInt(transform.position));

		_initiallyBlocked = node.isBlocked;
		_initiallyWater = node.isWater;
		_initiallyFire = node.isFire;
		_initiallyVoid = node.isVoid;
	}

	public IEnumerator Reveal() {
		Setup();

		yield return new WaitForSeconds(delay);

		gameObject.SetActive(true);
		node.isBlocked = actAsObstacle;
		node.isWater = actAsWater;
		node.isFire = actAsFire;
		node.isVoid = actAsVoid;

		yield return new WaitForSeconds(duration);

		node.isBlocked = _initiallyBlocked;
		node.isWater = _initiallyWater;
		node.isFire = _initiallyFire;
		node.isVoid = _initiallyVoid;
		gameObject.SetActive(false);
	}
}
}
