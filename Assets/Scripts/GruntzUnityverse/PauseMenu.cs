using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Itemz.MiscItemz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Arrowz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Bridgez;
using GruntzUnityverse.MapObjectz.Hazardz;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.Saving;
using GruntzUnityverse.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse {
  /// <summary>
  /// Handler for the Pause Menu UI and functionalities.
  /// </summary>
  public class PauseMenu : MonoBehaviour {
    public static bool isGamePaused; // Todo: Move to GameManager
    public GameObject pauseMenuUI;

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape) && !Helpbox.isTextShown && !GameManager.Instance.isPausedOnLevelLoad) {
        if (isGamePaused) {
          Resume();
        } else {
          Pause();
        }
      }
    }

    /// <summary>
    /// Activates the Pause Menu and stops the game.
    /// </summary>
    private void Pause() {
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0;
      isGamePaused = true;
    }

    /// <summary>
    /// Deactivates the Pause Menu and resumes the game.
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
      Debug.Log("Saving");

      List<GruntData> gruntData = new List<GruntData>();

      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz) {
        gruntData.Add(new GruntData(grunt));
      }

      List<ObjectData> objectData = new List<ObjectData>();

      foreach (MapObject mapObject in GameManager.Instance.currentLevelManager.mapObjectz) {
        objectData.Add(new ObjectData(mapObject));
      }

      SaveData saveData = new SaveData(gruntData, objectData);

      // _{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}
      string saveName = $"{SceneManager.GetActiveScene().name}";
      // QuickSaveWriter writer = QuickSaveWriter.Create(saveName, new QuickSaveSettings());

      // writer.Write("SaveData", saveData);
      // writer.Commit();
    }

    /// <summary>
    /// Loads a game state from a file.
    /// </summary>
    public void Load() {
      Debug.Log("Loading");

      GameManager.Instance.currentLevelManager.player1Gruntz.Clear();
      GameManager.Instance.currentLevelManager.ai1Gruntz.Clear();
      GameManager.Instance.currentLevelManager.allGruntz.Clear();

      GameManager.Instance.currentLevelManager.rollingBallz.Clear();

      foreach (Grunt grunt in FindObjectsOfType<Grunt>()) {
        Destroy(grunt.gameObject);
      }

      // QuickSaveReader reader = QuickSaveReader.Create($"{SceneManager.GetActiveScene().name}");

      // reader.Read<SaveData>("SaveData", result => {
      //   foreach (GruntData data in result.gruntData) {
      //     Addressables.InstantiateAsync($"P_{data.tool}Grunt.prefab").Completed += handle => {
      //       Grunt g = handle.Result.GetComponent<Grunt>();
      //       g.saveData = data;
      //       g.hasSaveData = true;
      //
      //       g.transform.parent = g.team == Team.Player1
      //         ? GameManager.Instance.currentLevelManager.playerGruntzParent
      //         : GameManager.Instance.currentLevelManager.dizgruntledParent;
      //     };
      //   }
      //
      //   foreach (MapObject mapObject in GameManager.Instance.currentLevelManager.mapObjectz) {
      //     Destroy(mapObject.gameObject);
      //   }
      //
      //   foreach (ObjectData data in result.objectData) {
      //     switch (data.type) {
      //       case nameof(Rock):
      //         Addressables.InstantiateAsync($"{nameof(Rock)}.prefab").Completed += handle => {
      //           Rock rock = handle.Result.GetComponent<Rock>();
      //           rock.transform.SetParent(GameManager.Instance.currentLevelManager.rockzParent);
      //           rock.objectId = data.objectId;
      //           rock.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite>($"{data.spriteName}.png").Completed += handle1 => {
      //             rock.spriteRenderer.sprite = handle1.Result;
      //             rock.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(GiantRock):
      //         Addressables.InstantiateAsync($"{nameof(GiantRock)}.prefab").Completed += handle => {
      //           GiantRock rock = handle.Result.GetComponent<GiantRock>();
      //           rock.transform.SetParent(GameManager.Instance.currentLevelManager.rockzParent);
      //           rock.objectId = data.objectId;
      //           rock.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite>($"{data.spriteName}.png").Completed += handle1 => {
      //             rock.spriteRenderer.sprite = handle1.Result;
      //             rock.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(GiantRockEdge):
      //         Addressables.InstantiateAsync($"{nameof(GiantRockEdge)}.prefab").Completed += handle => {
      //           GiantRockEdge edge = handle.Result.GetComponent<GiantRockEdge>();
      //           // Todo: Move to GiantRock parent instead
      //           edge.transform.SetParent(GameManager.Instance.currentLevelManager.rockzParent);
      //           edge.objectId = data.objectId;
      //           edge.transform.position = data.position;
      //           edge.spriteRenderer.enabled = false;
      //
      //           Addressables.LoadAssetAsync<Sprite>($"{data.spriteName}.png").Completed += handle1 => {
      //             edge.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(OneWayArrow):
      //         Addressables.InstantiateAsync($"{nameof(Arrow)}_1W_{data.optionalDirection}.prefab").Completed += handle => {
      //           Arrow oneWayArrow = handle.Result.GetComponent<Arrow>();
      //           oneWayArrow.transform.SetParent(GameManager.Instance.currentLevelManager.arrowzParent);
      //           oneWayArrow.objectId = data.objectId;
      //           oneWayArrow.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite>($"{data.spriteName}.png").Completed += handle1 => {
      //             oneWayArrow.spriteRenderer.sprite = handle1.Result;
      //             oneWayArrow.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(TwoWayArrow):
      //         Addressables.InstantiateAsync($"{nameof(Arrow)}_2W_{data.optionalDirection}.prefab").Completed += handle => {
      //           TwoWayArrow twoWayArrow = handle.Result.GetComponent<TwoWayArrow>();
      //           // Todo: Move to Puzzle parent instead
      //           twoWayArrow.transform.SetParent(GameManager.Instance.currentLevelManager.arrowzParent);
      //           twoWayArrow.objectId = data.objectId;
      //           twoWayArrow.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite>($"{data.spriteName}.png").Completed += handle1 => {
      //             twoWayArrow.spriteRenderer.sprite = handle1.Result;
      //             twoWayArrow.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(Spikez):
      //         Addressables.InstantiateAsync($"{nameof(Spikez)}.prefab").Completed += handle => {
      //           Spikez spikez = handle.Result.GetComponent<Spikez>();
      //           spikez.transform.SetParent(GameManager.Instance.currentLevelManager.spikezParent);
      //           spikez.objectId = data.objectId;
      //           spikez.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite>($"{data.spriteName}.png").Completed += handle1 => {
      //             spikez.spriteRenderer.sprite = handle1.Result;
      //             spikez.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(Bridge):
      //         Addressables.InstantiateAsync($"{nameof(Bridge)}.prefab").Completed += handle => {
      //           Bridge bridge = handle.Result.GetComponent<Bridge>();
      //           bridge.transform.SetParent(GameManager.Instance.currentLevelManager.bridgezParent);
      //           bridge.objectId = data.objectId;
      //           bridge.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite[]>($"{data.area}_{nameof(Bridge)}.png").Completed += handle1 => {
      //             bridge.spriteRenderer.sprite = handle1.Result[0];
      //             bridge.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(ToggleBridge):
      //         Addressables.InstantiateAsync($"{nameof(ToggleBridge)}.prefab").Completed += handle => {
      //           ToggleBridge bridge = handle.Result.GetComponent<ToggleBridge>();
      //           bridge.transform.SetParent(GameManager.Instance.currentLevelManager.bridgezParent);
      //           bridge.objectId = data.objectId;
      //           bridge.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite[]>($"{data.area}_{nameof(ToggleBridge)}.png").Completed += handle1 => {
      //             bridge.spriteRenderer.sprite = handle1.Result[0];
      //             bridge.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(CrumbleBridge):
      //         Addressables.InstantiateAsync($"{nameof(CrumbleBridge)}.prefab").Completed += handle => {
      //           CrumbleBridge bridge = handle.Result.GetComponent<CrumbleBridge>();
      //           bridge.transform.SetParent(GameManager.Instance.currentLevelManager.bridgezParent);
      //           bridge.objectId = data.objectId;
      //           bridge.transform.position = data.position;
      //
      //           int idx = int.Parse(data.spriteName.Split("_").Last());
      //
      //           Addressables.LoadAssetAsync<Sprite[]>($"{data.area}_{nameof(CrumbleBridge)}.png").Completed += handle1 => {
      //             bridge.spriteRenderer.sprite = handle1.Result[idx];
      //             bridge.Setup();
      //           };
      //         };
      //
      //         break;
      //       case nameof(RollingBall):
      //         Addressables.InstantiateAsync($"{nameof(RollingBall)}.prefab").Completed += handle => {
      //           RollingBall ball = handle.Result.GetComponent<RollingBall>();
      //           ball.transform.SetParent(GameManager.Instance.currentLevelManager.rockzParent);
      //           ball.objectId = data.objectId;
      //           ball.transform.position = data.position;
      //           ball.moveDirection = DirectionUtility.StringDirectionAsDirection(data.optionalBallDirection);
      //
      //           int idx = int.Parse(data.spriteName.Split("_").Last());
      //
      //           Addressables.LoadAssetAsync<Sprite[]>($"{data.area}_{nameof(RollingBall)}_{data.optionalBallDirection}.png").Completed +=
      //             handle1 => {
      //               ball.spriteRenderer.sprite = handle1.Result[idx];
      //               ball.Setup();
      //             };
      //         };
      //
      //         break;
      //       case nameof(SelectorCircle):
      //         Addressables.InstantiateAsync($"{nameof(SelectorCircle)}.prefab").Completed += handle => {
      //           SelectorCircle circle = handle.Result.GetComponent<SelectorCircle>();
      //           circle.transform.SetParent(GameManager.Instance.currentLevelManager.utilityparent);
      //           circle.objectId = data.objectId;
      //           circle.transform.position = data.position;
      //
      //           Addressables.LoadAssetAsync<Sprite>($"{nameof(SelectorCircle)}.png").Completed += handle1 => {
      //             circle.spriteRenderer.sprite = handle1.Result;
      //             circle.Setup();
      //           };
      //         };
      //
      //         break;
      //     }
      //   }
      // });
    }

    /// <summary>
    /// Displays the Options Menu.
    /// </summary>
    public void ShowOptions() {
      Debug.Log("Options");
    }

    /// <summary>
    /// Displays the Help Menu.
    /// </summary>
    public void ShowHelp() {
      Debug.Log("Help");
    }

    /// <summary>
    /// Quits the current level and loads the Main Menu.
    /// </summary>
    public void QuitGame() {
      Debug.Log("Save Game");

      Addressables.LoadSceneAsync("Menuz/MainMenu.unity").Completed += handle => {
        GameManager.Instance.hasChangedMusic = false;
      };
    }
  }
}
