using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.UI {
public class ToggleUIText : MonoBehaviour {
	public List<GameObject> targetz;

	public void ToggleTargetz() {
		foreach (GameObject target in targetz) {
			target.SetActive(!target.activeSelf);
		}
	}
}
}
