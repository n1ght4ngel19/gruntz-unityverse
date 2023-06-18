using GruntzUnityverse.Objectz.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void SaveGame() {
      // SaverLoader.CreateSaveFile();
      Debug.Log("Save");
    }

    public void LoadGame() {
      // SaverLoader.LoadSaveFile();
      Debug.Log("Load");
    }

    public void ShowOptions() {
      Debug.Log("Options");
    }

    public void ShowHelp() {
      Debug.Log("Help");
    }

    public void QuitGame() {
      Debug.Log("Save Game");
      SceneManager.LoadScene(Resources.Load<SceneAsset>("Menuz/MainMenu").name);
    }
  }
}
