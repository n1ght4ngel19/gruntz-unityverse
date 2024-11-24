using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
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

    private bool wasContinueKeyPressed => Keyboard.current.enterKey.wasPressedThisFrame
                                           || Keyboard.current.numpadEnterKey.wasPressedThisFrame
                                           || Keyboard.current.spaceKey.wasPressedThisFrame;

    private void Update() {
        if (_loadHandle.IsValid()) {
            sceneLoadProgressBar.value = _loadHandle.GetDownloadStatus().Percent * 100;
        }

        if (wasContinueKeyPressed) {
            Continue();
        }
    }

    public void LoadLevel(string levelToLoad) {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        _loadHandle = Addressables.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive, false);

        _loadHandle.Completed += handle => {
            _currentlyLoadedLevel = handle.Result;
            _completedLoad = true;

            sceneLoadProgressBar.value = 100;

            sceneIsLoadedText = GameObject.Find(Namez.SceneIsLoadedTextName).GetComponent<TMP_Text>();

            if (gameObject.TryGetComponent(out UnscaledTimeInvoker invoker)) {
                invoker.InvokeRepeatingUnscaled(nameof(ToggleLoadedMessage), 0, 0.5f);
            }
            else {
                gameObject.AddComponent<UnscaledTimeInvoker>().InvokeRepeatingUnscaled(nameof(ToggleLoadedMessage), 0, 0.5f);
            }
        };
    }

    private void ToggleLoadedMessage() {
        sceneIsLoadedText.enabled = !sceneIsLoadedText.enabled;
    }

    private void Continue() {
        if (_completedLoad) {
            _currentlyLoadedLevel.ActivateAsync().completed += _ => {
                SceneManager.UnloadSceneAsync(Namez.LoadMenuName);
            };
        }
    }
}
}
