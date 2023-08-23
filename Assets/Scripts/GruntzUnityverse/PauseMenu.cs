using GruntzUnityverse.MapObjectz.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  public class PauseMenu : MonoBehaviour {
    public static bool isGamePaused; // Todo: Move to GameManager
    public GameObject pauseMenuUI;

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape) && !Helpbox.isTextShown) {
        if (isGamePaused) {
          Resume();
        } else {
          Pause();
        }
      }
    }
    // ------------------------------------------------------------ //

    /// <summary>
    /// Activates the pause menu and pauses the game.
    /// </summary>
    private void Pause() {
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0;
      isGamePaused = true;
    }

    /// <summary>
    /// Deactivates the pause menu and resumes the game.
    /// </summary>
    public void Resume() {
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1f;
      isGamePaused = false;
    }

    /// <summary>
    /// Saves the game's current state and stores it in a file.
    /// </summary>
    public void Save() {
      Debug.Log("Save");
      // SaveManager.Instance.SaveGame();
    }

    /// <summary>
    /// Loads the game's state from a file.
    /// </summary>
    public void Load() {
      Debug.Log("Load");
      // StartCoroutine(SaveManager.Instance.LoadGame("save.json"));
    }

    /// <summary>
    /// Displays the options menu.
    /// </summary>
    public void ShowOptions() {
      Debug.Log("Options");
    }

    /// <summary>
    /// Displays the help menu.
    /// </summary>
    public void ShowHelp() {
      Debug.Log("Help");
    }

    /// <summary>
    /// Quits the game and returns to the main menu.
    /// </summary>
    public void QuitGame() {
      Debug.Log("Save Game");
      Addressables.LoadSceneAsync("MainMenu");
    }
  }
}
