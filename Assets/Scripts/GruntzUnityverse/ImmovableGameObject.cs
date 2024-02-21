using UnityEngine;

namespace GruntzUnityverse {
/// <summary>
/// Editor-only helper script for making certain Transforms unchangeable in the Inspector.
/// </summary>
public class ImmovableGameObject : MonoBehaviour {
	private void OnValidate() {
		transform.hideFlags = HideFlags.NotEditable;
	}
}
}
