using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class ContinueToLevel : MonoBehaviour {
	public TMP_Text sceneIsLoadedText;

	public Slider sceneLoadProgressBar;

	private SceneInstance _currentlyLoadedLevel;

	private bool _completedLoad;

	private AsyncOperationHandle<SceneInstance> _loadHandle;

	private void Update() {
		if (_loadHandle.IsValid()) {
			sceneLoadProgressBar.value = _loadHandle.GetDownloadStatus().Percent;
		}
	}

	public void LoadLevel(string levelToLoad) {
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		// SceneManager.UnloadSceneAsync("MainMenu");

		_loadHandle = Addressables.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive, false);

		_loadHandle.Completed += handle => {
			_currentlyLoadedLevel = handle.Result;
			_completedLoad = true;

			sceneIsLoadedText = GameObject.Find("SceneIsLoadedText").GetComponent<TMP_Text>();

			InvokeRepeating(nameof(ToggleLoadedMessage), 0, 0.5f);
		};
	}

	private void ToggleLoadedMessage() {
		sceneIsLoadedText.enabled = !sceneIsLoadedText.enabled;
	}

	private void OnContinueToLevel() {
		if (_completedLoad) {
			_currentlyLoadedLevel.ActivateAsync().completed += _ => {
				SceneManager.UnloadSceneAsync("LoadMenu");
			};
		}
	}
}
}
