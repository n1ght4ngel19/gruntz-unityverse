using GruntzUnityverse.Utils;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Misc {
public class EyeCandy : MonoBehaviour {
	private void OnDrawGizmosSelected() {
		transform.hideFlags = HideFlags.HideInInspector;

		GetComponent<SpriteRenderer>().hideFlags = HideFlags.HideInInspector;
		GetComponent<TrimName>().hideFlags = HideFlags.HideInInspector;
	}
}
}
