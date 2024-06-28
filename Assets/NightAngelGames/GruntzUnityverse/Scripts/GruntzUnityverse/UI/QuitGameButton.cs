using UnityEngine;
using Application = UnityEngine.Application;

namespace GruntzUnityverse.UI {
public class QuitGameButton : MonoBehaviour {
	public void QuitGame() {
		Application.Quit();
	}
}
}
