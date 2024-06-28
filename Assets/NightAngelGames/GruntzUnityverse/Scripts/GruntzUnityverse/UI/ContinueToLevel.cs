using GruntzUnityverse.Core;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class ContinueToLevel : MonoBehaviour {
	private SceneInstance _levelLoaded;
	public TMP_Text sceneIsLoadedText;
	private bool _completedLoad;
	public Slider loadingBar;

	private AsyncOperationHandle<SceneInstance> _loadHandle;

	private void Update() {
		if (_loadHandle.IsValid()) {
			loadingBar.value = _loadHandle.GetDownloadStatus().Percent;
		}
	}

	public void LoadLevel(string levelToLoad) {
		SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		// SceneManager.UnloadSceneAsync("MainMenu");

		_loadHandle = Addressables.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive, false);

		_loadHandle.Completed += handle => {
			_levelLoaded = handle.Result;
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
			_levelLoaded.ActivateAsync().completed += _ => {
				SceneManager.UnloadSceneAsync("LoadMenu");

				// GameManager.instance.Init();
			};
		}
	}
}
}
