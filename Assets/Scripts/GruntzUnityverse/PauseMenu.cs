using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Itemz.Misc;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Itemz.Toyz;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

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

      // _{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}
      string saveName = $"{SceneManager.GetActiveScene().name}";

      QuickSaveWriter writer = QuickSaveWriter.Create(saveName, new QuickSaveSettings());

      List<GruntData> gruntData = new List<GruntData>();

      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz) {
        gruntData.Add(new GruntData(grunt));
      }

      writer.Write("GruntData", gruntData);

      writer.Commit();
    }

    /// <summary>
    /// Loads the game's state from a file.
    /// </summary>
    public void Load() {
      Debug.Log("Load");

      GameManager.Instance.currentLevelManager.playerGruntz.Clear();
      GameManager.Instance.currentLevelManager.enemyGruntz.Clear();
      GameManager.Instance.currentLevelManager.allGruntz.Clear();

      foreach (Grunt grunt in FindObjectsOfType<Grunt>()) {
        Destroy(grunt.gameObject);
      }

      QuickSaveReader reader = QuickSaveReader.Create($"{SceneManager.GetActiveScene().name}");

      reader.Read<List<GruntData>>("GruntData", (r) => {
        foreach (GruntData data in r) {
          Addressables.InstantiateAsync($"P_{data.tool}Grunt.prefab").Completed += handle => {
            Grunt g = handle.Result.GetComponent<Grunt>();
            g.saveData = data;
            g.hasSaveData = true;
          };
        }
      });
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

      Addressables.LoadSceneAsync("Menuz/MainMenu.unity").Completed += handle => {
        GameManager.Instance.hasChangedMusic = false;
      };
    }
  }

}
