using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace GruntzUnityverse.UI {
[RequireComponent(typeof(LocalizeStringEvent))]
public class SimpleLocalize : MonoBehaviour {
	public TMP_Text toLocalize;

	private void OnValidate() {
		if (toLocalize == null) {
			toLocalize = GetComponent<TMP_Text>();
		}
	}

	public void SetText(string toSet) {
		toLocalize.SetText(toSet);
	}
}
}
