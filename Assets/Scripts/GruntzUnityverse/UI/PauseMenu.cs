using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace GruntzUnityverse.UI {
public class PauseMenu : MonoBehaviour {
	public Canvas canvas;

	private void Awake() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		canvas.enabled = false;
		canvas.worldCamera = Camera.main;

		GameObject.Find("ResumeButton")
			.GetComponent<Button>()
			.onClick.AddListener(
				() => {
					canvas.enabled = false;
					Time.timeScale = 1f;
				}
			);
	}

	private void OnSaveGame() {
		Debug.Log("Save game");
	}

	private void OnLoadGame() {
		Debug.Log("Load game");
	}

	private void OnEscape() {
		if (!GameManager.instance.helpboxUI.GetComponent<Canvas>().enabled) {
			Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
			canvas.enabled = Time.timeScale == 0f;
		} else {
			Time.timeScale = 1f;
		}

		GameManager.instance.selector.enabled = Time.timeScale is not 0;
		FindFirstObjectByType<CameraMovement>().enabled = Time.timeScale is not 0;
	}

	// private void OnApplicationFocus(bool hasFocus) {
	// 	if (!hasFocus) {
	// 		OnEscape();
	// 	}
	// }
}
}
