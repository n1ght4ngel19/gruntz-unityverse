using UnityEngine;

namespace GruntzUnityverse.Objectz.Misc {
public class EyeCandy : MonoBehaviour {
    private void OnValidate() {
        // GetComponent<SpriteRenderer>().sor
    }

    private void Start() {
		gameObject.hideFlags = HideFlags.HideInHierarchy;
	}

	private void OnDrawGizmosSelected() {
		transform.hideFlags = HideFlags.None;

		GetComponent<SpriteRenderer>().hideFlags = HideFlags.None;
	}
}
}
