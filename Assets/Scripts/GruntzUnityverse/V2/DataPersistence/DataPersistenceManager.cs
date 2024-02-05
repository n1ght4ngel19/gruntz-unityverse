using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.V2.DataPersistence {
  public class DataPersistenceManager : MonoBehaviour {
    public static DataPersistenceManager Instance { get; private set; }

    [Header("Transforms")]
    public Transform gruntzTransform;

    [Header("Prefabs")]
    public GruntV2 baseGrunt;

    [Header("Game Data")]
    [SerializeField]
    private GameData currentGameData;

    public List<IDataPersistence> dataPersistenceObjects;

    [Header("File Storage Settings")]
    [SerializeField]
    private string dataFileName;

    private FileDataHandler _fileDataHandler;

    private void Awake() {
      if (Instance == null) {
        Instance = this;
      } else {
        Destroy(gameObject);
      }
    }

    private void Start() {
      currentGameData = new GameData();
      _fileDataHandler = new FileDataHandler(Application.persistentDataPath, dataFileName);
      gruntzTransform = GameObject.Find("Gruntz").transform;

      // Todo: Find out why this doesn't work
      // Addressables.LoadAssetAsync<GruntV2>("BaseGrunt.prefab").Completed += handle => {
      //   baseGrunt = handle.Result;
      // };
    }

    /// <summary>
    /// Todo
    /// </summary>
    public void NewGame() {
      // _gameData = new GameData();
      Debug.Log("New Game");
    }

    /// <summary>
    /// Saves the game data to a file.
    /// </summary>
    public void SaveGame() {
      dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IDataPersistence>()
        .ToList();

      // currentGameData.gruntData = new List<GruntDataV2>();

      // Iterate through all the data persistence objects (dynamic objects) and make them save their data
      foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects) {
        dataPersistenceObject.Save(ref currentGameData);
      }

      // Save the game data to disk
      _fileDataHandler.SaveGameData(currentGameData);
    }

    /// <summary>
    /// Loads the game data from a file.
    /// </summary>
    public void LoadGame() {
      // Todo: See if this is necessary
      if (currentGameData == null) {
        NewGame();

        return;
      }

      try {
        DestroyEverything();
      } catch (Exception e) {
        Debug.LogException(e);

        throw;
      } finally {
        // Load the game data from disk
        currentGameData = _fileDataHandler.LoadGameData();

        Debug.Log(currentGameData.gruntData.Count);

        // Instantiate Gruntz from the loaded data
        for (int i = 0; i < currentGameData.gruntData.Count; i++) {
          Instantiate(baseGrunt, currentGameData.gruntData[i].position, Quaternion.identity, gruntzTransform)
            .Load(currentGameData.gruntData[i]);
        }
      }

      // Reset game data after finishing loading
      currentGameData = new GameData();
    }

    /// <summary>
    /// Destroys all the dynamic objects in the scene. Used for cleaning up the scene before loading.
    /// </summary>
    private static void DestroyEverything() {
      List<MonoBehaviour> toDestroy = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IDataPersistence>()
        .Select(dpo => dpo as MonoBehaviour)
        .ToList();

      toDestroy.ForEach(mb => Destroy(mb.gameObject));
    }

    [ContextMenu("Generate GUIDs")]
    public void GenerateGuids() {
      List<IDataPersistence> list = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
        .OfType<IDataPersistence>()
        .ToList();

      list.ForEach(
        dpo => {
          dpo.Guid = Guid.NewGuid().ToString();
          Debug.Log($"Generated GUID {dpo.Guid} for {dpo}");
        }
      );
    }

    // --------------------------------------------------
    // Input Actions
    // --------------------------------------------------
    private void OnSaveGame() {
      Instance.SaveGame();
    }

    private void OnLoadGame() {
      Instance.LoadGame();
    }
  }
}
