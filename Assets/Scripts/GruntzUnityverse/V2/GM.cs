using System.Collections.Generic;
using System.Linq;
using InfinityCode.Observers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.V2 {
  public class GM : MonoBehaviour {
    [Header("Gruntz")]
    public List<GruntV2> gruntz;
    public LinkedValue<List<GruntV2>> selectedGruntz;

    public LevelV2 level;

    private void Awake() {
      SceneManager.sceneLoaded += OnSceneLoaded;
      Application.targetFrameRate = 60;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
      Debug.Log($"Loaded {scene.name}");

      gruntz = FindObjectsByType<GruntV2>(FindObjectsSortMode.None).ToList();
      
      level.Initialize();
    }
  }
}
