using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.DataPersistence;
using InfinityCode.Observers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.V2 {
  public class InputManager : MonoBehaviour {
    /// <summary>
    /// The singleton accessor of the InputManager.
    /// </summary>
    public static InputManager Instance { get; private set; }

    /// <summary>
    /// The selector responsible for selecting and highlighting objects.
    /// </summary>
    public Selector selector;

    /// <summary>
    /// All the nodes of the current level.
    /// </summary>
    public LinkedValue<HashSet<NodeV2>> levelNodes;

    /// <summary>
    /// All the Gruntz in the current level, linked to the Gruntz in the GM.
    /// </summary>
    [Header("Gruntz")]
    public LinkedValue<List<GruntV2>> allGruntz;

    /// <summary>
    /// The Gruntz currently selected by the player.
    /// </summary>
    public List<GruntV2> selectedGruntz;

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }

      SceneManager.sceneLoaded += OnSceneLoaded;
      Application.targetFrameRate = 60;

      DontDestroyOnLoad(this);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (scene.name == "MainMenu") {
        return;
      }

      Debug.Log($"Loaded {scene.name}");

      selector = FindFirstObjectByType<Selector>();
      levelNodes = new LinkedValue<HashSet<NodeV2>>(FindFirstObjectByType<LevelV2>(), "levelNodes");
      allGruntz = new LinkedValue<List<GruntV2>>(FindFirstObjectByType<GM>(), "allGruntz");
    }

    #region Input Actions
    // --------------------------------------------------
    // Input Actions
    // --------------------------------------------------
    private void OnSelect() {
      selectedGruntz.Clear();

      if (allGruntz.Value.Any(grunt => grunt.location2D == selector.location2D)) {
        Debug.Log("Selecting");
      }

      allGruntz.Value.ForEach(
        grunt => {
          grunt.SetSelected(grunt.location2D == selector.location2D);

          if (grunt.flagz.selected) {
            selectedGruntz.Add(grunt);
          }
        }
      );
    }

    // private void OnMove() {
    //   Vector2Int target = Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    //
    //   gruntz.Value
    //     .Where(grunt => grunt.flagz.selected)
    //     .ToList()
    //     .ForEach(grunt => grunt.Move(target));
    // }

    private void OnAction() {
      // gruntz.Value
      //   .Where(grunt => grunt.flagz.selected)
      //   .ToList()
      //   .ForEach(grunt => grunt.Act());
    }

    private void OnGive() {
      // gruntz.Value
      //   .Where(grunt => grunt.flagz.selected)
      //   .ToList()
      //   .ForEach(grunt => grunt.Give());
    }

    private void OnSaveGame() {
      DataPersistenceManager.Instance.SaveGame();
    }

    private void OnLoadGame() {
      DataPersistenceManager.Instance.LoadGame();
    }
    #endregion

    private bool CompareVector2Int(Vector3 a, Vector3 b) {
      return Vector2Int.RoundToInt(a) == Vector2Int.FloorToInt(b);
    }
  }

}
