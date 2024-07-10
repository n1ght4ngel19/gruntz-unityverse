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

	private MenuNavigator menuNavigator => FindFirstObjectByType<MenuNavigator>();

	private void OnValidate() {
		button = GetComponent<Button>();
	}

	public void HandleTargetz() {
		foreach (GameObject target in enableTargetz) {
			target.GetComponentsInChildren<TMP_Text>().ToList().ForEach(t => t.enabled = true);
			target.GetComponentsInChildren<Image>().ToList().ForEach(i => i.enabled = true);
		}

		foreach (GameObject target in disableTargetz) {
			target.GetComponentsInChildren<TMP_Text>().ToList().ForEach(t => t.enabled = false);
			target.GetComponentsInChildren<Image>().ToList().ForEach(i => i.enabled = false);
		}

		menuNavigator.currentMenu = enableTargetz.First();
		menuNavigator.currentActiveButton = menuNavigator.currentMenu.GetComponentsInChildren<ButtonNavigatable>().First();

		menuNavigator.currentBackButton =
			menuNavigator.currentMenu.GetComponentsInChildren<DisableUITextOnButtonPress>().FirstOrDefault(du => du.CompareTag("BackButton"));

		menuNavigator.gameCursor.transform.position = menuNavigator.currentActiveButton.transform.position;

		menuNavigator.currentBackButton.GetComponent<TMP_Text>().color = menuNavigator.currentBackButton.GetComponent<Button>().enabled
			? new Color32(255, 128, 0, 255)
			: new Color32(255, 255, 255, 255);
	}

	private void Start() {
		button.onClick.AddListener(HandleTargetz);
	}
}
}
