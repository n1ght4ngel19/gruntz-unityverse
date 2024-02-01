using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.V2.Core {
  public class GM : MonoBehaviour {
    /// <summary>
    /// The singleton accessor of the GM.
    /// </summary>
    public static GM Instance { get; private set; }

    /// <summary>
    /// The selector responsible for selecting and highlighting objects.
    /// </summary>
    public Selector selector;

    /// <summary>
    /// All the Gruntz in the current level.
    /// </summary>
    [Header("Gruntz")]
    public List<GruntV2> allGruntz;


    /// <summary>
    /// The Gruntz currently selected by the player.
    /// </summary>
    public HashSet<GruntV2> selectedGruntz;

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }

      SceneManager.sceneLoaded += OnSceneLoaded;
      Application.targetFrameRate = 60;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      if (scene.name is "MainMenu" or "StatzMenu") {
        return;
      }

      Debug.Log($"Loaded {scene.name}");

      allGruntz = FindObjectsByType<GruntV2>(FindObjectsSortMode.None).ToList();
      selectedGruntz = new HashSet<GruntV2>(allGruntz.Where(grunt => grunt.flagz.selected));

      FindObjectsByType<EyeCandy>(FindObjectsSortMode.None)
        .ToList()
        .ForEach(go => go.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(go.transform.position.y) * -1);
    }
  }
}
