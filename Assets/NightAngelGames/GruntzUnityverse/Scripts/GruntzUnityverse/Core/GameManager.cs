using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Hazardz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Pyramidz;
using GruntzUnityverse.UI;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;


namespace GruntzUnityverse.Core {
public class GameManager : MonoBehaviour {
    [Expandable]
    public LevelData levelData;

    [Header("Gruntz")] public List<Grunt> gruntz => playerGruntz.Concat(dizgruntled).ToList();

    public List<Grunt> playerGruntz;

    public List<Grunt> dizgruntled;

    public List<Grunt> selectedGruntz;

    [Header("Cached Objectz")]
    public List<RedPyramid> redPyramidz;

    public List<GridObject> gridObjectz;

    public List<Spikez> spikez;

    public Grunt firstSelected => selectedGruntz.FirstOrDefault();

    [Header("User Interface")]
    public GooWell gooWell;

    public GameObject helpboxUI;

    /// <summary>
    /// The selector responsible for selecting and highlighting objects.
    /// </summary>
    public Selector selector;

    [Header("Level Transformz")]
    public GameObject actorz;

    public GameObject itemz;

    public GameObject objectz;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() {
        FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include).canvas.worldCamera =
            FindFirstObjectByType<CameraMovement>(FindObjectsInactive.Include).gameObject.GetComponent<Camera>();

        GameObject.Find("Visualizer")?.SetActive(false);

        InitializeLevel();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name is Namez.MainMenuName or Namez.LoadMenuName) {
            return;
        }

        Application.targetFrameRate = 120;
        Cursor.visible = false;

        FindFirstObjectByType<GameCursor>().enabled = true;

        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include);

        if (pauseMenu != null) {
            pauseMenu.canvas.enabled = false;
        }
    }

    public void InitializeLevel() {
        // --------------------------------------------------
        // UI setup
        // --------------------------------------------------
        Cursor.visible = false;
        FindFirstObjectByType<GameCursor>().enabled = true;

        // GameObject.Find("SidebarUI").GetComponent<Canvas>().worldCamera =
        // 	FindObjectsByType<Camera>(FindObjectsSortMode.None).First(cam => cam.gameObject.name == "InterfaceCamera");

        GameObject.Find(Namez.SidebarUIName).GetComponent<Canvas>().enabled = true;

        gooWell = FindFirstObjectByType<GooWell>();

        FindFirstObjectByType<PauseMenu>().canvas.worldCamera = FindFirstObjectByType<CameraMovement>().gameObject.GetComponent<Camera>();

        Time.timeScale = 0f;

        // --------------------------------------------------
        // Collecting Actorz
        // --------------------------------------------------

        // --------------------------------------------------
        // Collecting Objectz
        // --------------------------------------------------
        redPyramidz = FindObjectsByType<RedPyramid>(FindObjectsSortMode.None).ToList();

        gridObjectz = FindObjectsByType<GridObject>(FindObjectsSortMode.None).Where(go => go is not Blocker or Brick).ToList();

        spikez = FindObjectsByType<Spikez>(FindObjectsSortMode.None).ToList();
    }

    private void OnSwitchLocale() {
        LocalizationSettings.SelectedLocale = LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1]
            ? LocalizationSettings.AvailableLocales.Locales[0]
            : LocalizationSettings.AvailableLocales.Locales[1];
    }

    private void OnChangeGameSpeed() {
        Time.timeScale = Time.timeScale switch {
            0 => 0,
            2f => 1f,
            _ => 2f,
        };
    }

    private void OnFreezeGame() {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }

    public void LocalizeLevelName(string newLevelName) {
        levelData.levelName = newLevelName;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GameManager gameManager = (GameManager)target;
        Level level = FindFirstObjectByType<Level>();

        if (GUILayout.Button("Initialize Level")) {
            DateTime start = DateTime.Now;

            level.Initialize();

            gameManager.InitializeLevel();

            // Set the sorting order of all EyeCandy objects so they render properly behind or in front of each other
            // FindObjectsByType<EyeCandy>(FindObjectsSortMode.None)
            // 	.Where(ec => ec.gameObject.CompareTag("HighEyeCandy"))
            // 	.ToList()
            // 	.ForEach(ec1 => ec1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(ec1.transform.position.y) * -1);

            List<Checkpoint> checkpointz = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).ToList();

            List<LevelItem> levelItemz = FindObjectsByType<LevelItem>(FindObjectsSortMode.None).ToList();

            Debug.Log($"Initialization took {DateTime.Now - start}");
        }
    }
}
#endif
}
