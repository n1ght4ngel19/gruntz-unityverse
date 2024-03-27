using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class DisableUITextOnButtonPress : MonoBehaviour {
	public Button button;
	public List<GameObject> enableTargetz;
	public List<GameObject> disableTargetz;

	private void OnValidate() {
		button = GetComponent<Button>();
	}

	private void Start() {
		button.onClick.AddListener(
			() => {
				foreach (GameObject target in enableTargetz) {
					target.GetComponentsInChildren<TMP_Text>().ToList().ForEach(t => t.enabled = true);
					target.GetComponentsInChildren<Image>().ToList().ForEach(i => i.enabled = true);
				}

				foreach (GameObject target in disableTargetz) {
					target.GetComponentsInChildren<TMP_Text>().ToList().ForEach(t => t.enabled = false);
					target.GetComponentsInChildren<Image>().ToList().ForEach(i => i.enabled = false);
				}
			}
		);
	}
}
}
