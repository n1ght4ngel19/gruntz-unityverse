using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.UI {
public class MainMenuInitializer : MonoBehaviour {
	private void Awake() {
		Application.targetFrameRate = 120;

		Cursor.visible = false;

		FindFirstObjectByType<GameCursor>().enabled = true;
	}
}
}
