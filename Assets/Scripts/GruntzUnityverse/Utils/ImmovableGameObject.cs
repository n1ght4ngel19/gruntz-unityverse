#if UNITY_EDITOR
using UnityEngine;

namespace GruntzUnityverse.Utils {
/// <summary>
/// Editor-only helper script for making certain Transforms unchangeable in the Inspector.
/// </summary>
public class ImmovableGameObject : MonoBehaviour {
	private void OnDrawGizmosSelected() {
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	private void OnValidate() {
		transform.hideFlags = HideFlags.HideInInspector;
	}
}
}
#endif
