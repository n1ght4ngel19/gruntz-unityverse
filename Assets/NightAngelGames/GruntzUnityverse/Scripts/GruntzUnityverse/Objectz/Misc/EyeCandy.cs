using UnityEngine;

namespace GruntzUnityverse.Objectz.Misc {
public class EyeCandy : MonoBehaviour {
	private void Start() {
		gameObject.hideFlags = HideFlags.HideInHierarchy;
	}

	private void OnDrawGizmosSelected() {
		transform.hideFlags = HideFlags.HideInInspector;

		GetComponent<SpriteRenderer>().hideFlags = HideFlags.HideInInspector;
	}
}
}
