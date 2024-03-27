using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class MainMenuInitializer : MonoBehaviour {
	private void Awake() {
		Application.targetFrameRate = 120;

		Cursor.visible = false;

		FindFirstObjectByType<GameCursor>().enabled = true;

		GameObject.Find("ShowHelp_Value").GetComponent<Toggle>().isOn = Settings.instance.gameSettings.showHelpboxez;
	}
}
}
