using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace GruntzUnityverse.UI {
public class PauseMenu : MonoBehaviour {
	public Canvas canvas;

	private void Awake() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		canvas.enabled = false;
	}

	private void OnSaveGame() {
		Debug.Log("Save game");
	}

	private void OnLoadGame() {
		Debug.Log("Load game");
	}

	private void OnEscape() {
		Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
		canvas.enabled = Time.timeScale == 0f;

		Debug.Log(Time.timeScale == 0f ? "Show pause menu" : "Resume the game (do nothing)");
	}
}
}
