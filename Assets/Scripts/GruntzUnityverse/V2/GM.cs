using System.Collections.Generic;
using System.Linq;
using InfinityCode.Observers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.V2 {
  public class GM : MonoBehaviour {
    /// <summary>
    /// The singleton accessor of the GM.
    /// </summary>
    public static GM Instance { get; private set; }

    /// <summary>
    /// The current level.
    /// </summary>
    public LevelV2 level;

    /// <summary>
    /// All the Gruntz in the current level.
    /// </summary>
    [Header("Gruntz")]
    public List<GruntV2> allGruntz;

    /// <summary>
    /// The Gruntz currently selected by the player, linked to the SelectedGruntz in the InputManager.
    /// </summary>
    public LinkedValue<List<GruntV2>> selectedGruntz;

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

      level = FindFirstObjectByType<LevelV2>();
      level.Initialize();

      allGruntz = FindObjectsByType<GruntV2>(FindObjectsSortMode.None).ToList();
      selectedGruntz = new LinkedValue<List<GruntV2>>(FindFirstObjectByType<InputManager>(), "selectedGruntz");
    }
  }
}
