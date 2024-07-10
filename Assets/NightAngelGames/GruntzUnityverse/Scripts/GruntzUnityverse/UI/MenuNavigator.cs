using GruntzUnityverse.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class MenuNavigator : MonoBehaviour {
	public GameCursor gameCursor;

	public GameObject currentMenu;

	public DisableUITextOnButtonPress currentBackButton;

	public ButtonNavigatable currentActiveButton;

	private void OnToggleBack() {
		if (currentBackButton == null) {
			return;
		}

		currentBackButton.HandleTargetz();
	}

	private void OnActivateNext() {
		gameCursor.enabled = false;

		currentActiveButton.GetComponent<TMP_Text>().color = currentActiveButton.GetComponent<Button>().enabled
			? new Color32(243, 250, 0, 255)
			: new Color32(255, 255, 255, 255);

		currentActiveButton = currentActiveButton.next;

		gameCursor.transform.position = currentActiveButton.GetComponent<RectTransform>().position;

		currentActiveButton.GetComponent<TMP_Text>().color = currentActiveButton.GetComponent<Button>().enabled
			? new Color32(255, 128, 0, 255)
			: new Color32(255, 255, 255, 255);
	}

	private void OnActivatePrevious() {
		gameCursor.enabled = false;

		currentActiveButton.GetComponent<TMP_Text>().color = currentActiveButton.GetComponent<Button>().enabled
			? new Color32(243, 250, 0, 255)
			: new Color32(255, 255, 255, 255);

		currentActiveButton = currentActiveButton.previous;

		gameCursor.transform.position = currentActiveButton.GetComponent<RectTransform>().position;

		currentActiveButton.GetComponent<TMP_Text>().color = currentActiveButton.GetComponent<Button>().enabled
			? new Color32(255, 128, 0, 255)
			: new Color32(255, 255, 255, 255);
	}

	private void OnPressEnter() {
		if (currentActiveButton == null) {
			return;
		}

		currentActiveButton.Activate();

		currentActiveButton.GetComponent<TMP_Text>().color = currentActiveButton.GetComponent<Button>().enabled
			? new Color32(243, 250, 0, 255)
			: new Color32(255, 255, 255, 255);
	}

	private void OnMoveMouse() {
		gameCursor.enabled = true;

		currentActiveButton.GetComponent<TMP_Text>().color = currentActiveButton.GetComponent<Button>().enabled
			? new Color32(255, 128, 0, 255)
			: new Color32(255, 255, 255, 255);
	}
}
}
