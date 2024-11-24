using System;
using System.Linq;
using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


namespace GruntzUnityverse.UI {
public class PauseMenu : MonoBehaviour {
    public GameManager gameManager;

    public Canvas canvas;

    private void Awake() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (canvas != null) {
            canvas.enabled = false;
            canvas.worldCamera = Camera.main;
        }

        GameObject.Find(Namez.ResumeButtonName)?.GetComponent<Button>().onClick.AddListener(Resume);
    }

    private void Resume() {
        canvas.enabled = false;
        Time.timeScale = 1f;
    }

    public void Restart() {
        LevelLoader loader = GameObject.Find(Namez.RestartButtonName).GetComponent<LevelLoader>();
        loader.levelData = gameManager.levelData;

        // loader.levelKey = SceneManager.GetActiveScene().name;
        // loader.levelName = gameManager.levelData.levelName;
        //
        // loader.areaName = gameManager.levelData.area switch {
        //     Area.RockyRoadz => "ROCKY ROADZ",
        //     Area.Gruntziclez => "GRUNTZICLEZ",
        //     Area.TroubleInTheTropicz => "TROUBLE IN THE TROPICZ",
        //     Area.HighOnSweetz => "HIGH ON SWEETZ",
        //     Area.HighRollerz => "HIGH ROLLERZ",
        //     Area.HoneyIShrunkTheGruntz => "HONEY, SHRUNK THE GRUNTZ!",
        //     Area.TheMiniatureMasterz => "THE MINIATURE MASTERZ",
        //     Area.GruntzInSpace => "GRUNTZ IN SPACE",
        //     Area.VirtualReality => "VIRTUAL REALITY",
        //     _ => throw new ArgumentOutOfRangeException(),
        // };

        // Addressables.LoadAssetAsync<Sprite>($"AreaBackground-{loader.levelData.levelKey.Split("-").First()}").Completed += handle => {
        //     loader.loadMenuBackground = handle.Result;
        //     loader.LoadLevel();
        // };

        loader.LoadLevel();
    }

    private void OnSaveGame() {
        Debug.Log("Save game");
    }

    private void OnLoadGame() {
        Debug.Log("Load game");
    }

    public void OnEscape() {
        // The game is paused because of something else (e.g. level load)
        if (!canvas.enabled && Time.timeScale == 0f) {
            return;
        }

        // The game is paused to display the helpbox UI
        if (gameManager.helpboxUI.GetComponent<Canvas>().enabled) {
            Time.timeScale = 1f;

            return;
        }

        // if (!gameManager.helpboxUI.GetComponent<Canvas>().enabled) {
        canvas.enabled = !canvas.enabled;
        Time.timeScale = canvas.enabled ? 0f : 1f;
        GameCursor.instance.gameObject.SetActive(!canvas.enabled);

        transform.GetComponentsInChildren<RectTransform>(true).First(rt => rt.name == "Main").gameObject.SetActive(true);
        transform.GetComponentsInChildren<RectTransform>(true).First(rt => rt.name == "OptionzMenu").gameObject.SetActive(false);
        transform.GetComponentsInChildren<RectTransform>(true).First(rt => rt.name == "Help").gameObject.SetActive(false);

        // }

        gameManager.selector.enabled = Time.timeScale != 0f;
        FindFirstObjectByType<CameraMovement>().enabled = Time.timeScale != 0f;
    }

    // private void OnApplicationFocus(bool hasFocus) {
    // 	if (!hasFocus) {
    // 		OnEscape();
    // 	}
    // }
}
}
