using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.UI {
public class PauseMenuQuitButton : MonoBehaviour {
	public void QuitToMainMenu() {
		Addressables.LoadSceneAsync("MainMenu", LoadSceneMode.Additive, false).Completed += handle => {
			FindFirstObjectByType<PauseMenu>().canvas.enabled = false;

			handle.Result.ActivateAsync().completed += _ => {
				Time.timeScale = 1f;
				SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
			};
		};
	}
}
}
