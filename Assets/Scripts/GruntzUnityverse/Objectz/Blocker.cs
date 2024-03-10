using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Blocker : MonoBehaviour {
	private void Awake() {
		GetComponent<SpriteRenderer>().enabled = false;
	}
}
}
