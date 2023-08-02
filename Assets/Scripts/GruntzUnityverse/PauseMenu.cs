using GruntzUnityverse.Objectz.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  public class PauseMenu : MonoBehaviour {
    public static bool IsGamePaused;
    public GameObject pauseMenuUI;

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape) && !HelpBox.IsTextShown) {
        if (IsGamePaused) {
          Resume();
        } else {
          Pause();
        }
      }
    }

    private void Pause() {
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0;
      IsGamePaused = true;
    }

    public void Resume() {
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1f;
      IsGamePaused = false;
    }

    public void Save() {
      Debug.Log("Save");
      // SaveManager.Instance.SaveGame();
    }

    public void Load() {
      Debug.Log("Load");
      // StartCoroutine(SaveManager.Instance.LoadGame("save.json"));
    }

    public void ShowOptions() {
      Debug.Log("Options");
    }

    public void ShowHelp() {
      Debug.Log("Help");
    }

    public void QuitGame() {
      Debug.Log("Save Game");
      Addressables.LoadSceneAsync("MainMenu");
    }
  }
}
