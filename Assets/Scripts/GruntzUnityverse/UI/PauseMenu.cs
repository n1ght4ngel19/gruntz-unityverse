using System;
using System.Linq;
using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace GruntzUnityverse.UI {
public class PauseMenu : MonoBehaviour {
	public Canvas canvas;

	private void Awake() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		canvas.enabled = false;
		canvas.worldCamera = Camera.main;

		GameObject.Find("ResumeButton")
			.GetComponent<Button>()
			.onClick.AddListener(
				() => {
					canvas.enabled = false;
					Time.timeScale = 1f;
				}
			);
	}

	public void Restart() {
		LevelLoader loader = GameObject.Find("RestartButton").GetComponent<LevelLoader>();

		loader.levelKey = SceneManager.GetActiveScene().name;

		loader.levelName = GameManager.instance.levelName;

		loader.areaName = GameManager.instance.area switch {
			Area.RockyRoadz => "ROCKY ROADZ",
			Area.Gruntziclez => "GRUNTZICLEZ",
			Area.TroubleInTheTropicz => "TROUBLE IN THE TROPICZ",
			Area.HighOnSweetz => "HIGH ON SWEETZ",
			Area.HighRollerz => "HIGH ROLLERZ",
			Area.HoneyIShrunkTheGruntz => "HONEY, SHRUNK THE GRUNTZ!",
			Area.TheMiniatureMasterz => "THE MINIATURE MASTERZ",
			Area.GruntzInSpace => "GRUNTZ IN SPACE",
			Area.VirtualReality => "VIRTUAL REALITY",
			_ => throw new ArgumentOutOfRangeException(),
		};

		Addressables.LoadAssetAsync<Sprite>($"AreaBackground-{loader.levelKey.Split("-").First()}").Completed += handle => {
			loader.loadMenuBackground = handle.Result;

			loader.LoadLevel();
		};
	}

	private void OnSaveGame() {
		Debug.Log("Save game");
	}

	private void OnLoadGame() {
		Debug.Log("Load game");
	}

	public void OnEscape() {
		if (!GameManager.instance.helpboxUI.GetComponent<Canvas>().enabled) {
			Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
			canvas.enabled = Time.timeScale == 0f;
		} else {
			Time.timeScale = 1f;
		}

		GameManager.instance.selector.enabled = Time.timeScale is not 0;
		FindFirstObjectByType<CameraMovement>().enabled = Time.timeScale is not 0;
	}

	// private void OnApplicationFocus(bool hasFocus) {
	// 	if (!hasFocus) {
	// 		OnEscape();
	// 	}
	// }
}
}
