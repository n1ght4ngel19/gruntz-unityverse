using System.Collections;
using System.Linq;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class SecretTile : GridObject {
	public float delay;
	public float duration;

	private bool _initiallyBlocked;
	private bool _initiallyWater;
	private bool _initiallyFire;
	private bool _initiallyVoid;

	protected override void Start() {
		node = Level.Instance.levelNodes.First(n => n.location2D == location2D);
		_initiallyBlocked = node.isBlocked;
		_initiallyWater = node.isWater;
		_initiallyFire = node.isFire;
		_initiallyVoid = node.isVoid;
	}

	public IEnumerator Reveal() {
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
