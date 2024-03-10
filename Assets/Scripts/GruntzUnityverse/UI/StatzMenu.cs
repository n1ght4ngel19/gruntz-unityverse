using GruntzUnityverse.Core;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.UI {
public class StatzMenu : MonoBehaviour {
	public Canvas canvas;
	public TMP_Text timeValue;
	public TMP_Text survivorzValue;
	public TMP_Text deathzValue;
	public TMP_Text toolzValue;
	public TMP_Text toyzValue;
	public TMP_Text powerupzValue;
	public TMP_Text coinzValue;
	public TMP_Text secretzValue;

	public static StatzMenu Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
		Application.targetFrameRate = 60;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		canvas.enabled = false;
	}

	public void Activate() {
		canvas.enabled = true;

		timeValue.text = Level.Instance.levelStatz.secondzTaken.ToString();
		survivorzValue.text = GameManager.Instance.allGruntz.Count.ToString();
		deathzValue.text = Level.Instance.levelStatz.deathz.ToString();
		toolzValue.text = $"{Level.Instance.levelStatz.toolzCollected.ToString()} OF {Level.Instance.levelStatz.maxToolz.ToString()}";
		toyzValue.text = $"{Level.Instance.levelStatz.toyzCollected.ToString()} OF {Level.Instance.levelStatz.maxToyz.ToString()}";
		powerupzValue.text = $"{Level.Instance.levelStatz.powerupzCollected.ToString()} OF {Level.Instance.levelStatz.maxPowerupz.ToString()}";
		coinzValue.text = $"{Level.Instance.levelStatz.coinzCollected.ToString()} OF {Level.Instance.levelStatz.maxCoinz.ToString()}";
		secretzValue.text = $"{Level.Instance.levelStatz.discoveredSecretz.ToString()} OF {Level.Instance.levelStatz.maxSecretz.ToString()}";
	}

	private void OnEscape() {
		if (GetComponent<Canvas>().enabled) {
			Addressables.LoadSceneAsync("MainMenu");
		}
	}
}
}
