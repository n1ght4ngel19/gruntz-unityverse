using System;
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
	public TMP_Text levelNameText;
	public TMP_Text sceneIsLoadedText;
	public string localizedLoadedMessage;
	private bool _completedLoad;
	public Slider loadingBar;

	private AsyncOperationHandle<SceneInstance> _loadHandle;

	private void Update() {
		if (_loadHandle.IsValid()) {
			loadingBar.value = _loadHandle.GetDownloadStatus().Percent;
		}
	}

	public void LoadLevel(string levelToLoad) {
		SceneManager.UnloadSceneAsync("MainMenu");

		_loadHandle = Addressables.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive, false);

		_loadHandle.Completed += handle => {
			_levelLoaded = handle.Result;
			_completedLoad = true;

			sceneIsLoadedText.SetText(localizedLoadedMessage);
			InvokeRepeating(nameof(ToggleLoadedMessage), 0, 1f);
		};
	}

	private void ToggleLoadedMessage() {
		sceneIsLoadedText.enabled = !sceneIsLoadedText.enabled;
	}

	private void OnContinueToLevel() {
		if (_completedLoad) {
			_levelLoaded.ActivateAsync().completed += _ => {
				GameManager.instance.InitUI();

				SceneManager.UnloadSceneAsync("LoadMenu");
			};
		}
	}

	public void LocalizeLoadedMessage(string message) {
		localizedLoadedMessage = message;
	}
}
}
