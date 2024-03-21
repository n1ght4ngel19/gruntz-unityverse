using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Misc;
using GruntzUnityverse.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.Core {
public class GameManager : MonoBehaviour {
	/// <summary>
	/// The singleton accessor of the GM.
	/// </summary>
	public static GameManager instance { get; private set; }

	/// <summary>
	/// The name of the current Level (not the scene name/address).
	/// </summary>
	public string levelName;

	/// <summary>
	/// The selector responsible for selecting and highlighting objects.
	/// </summary>
	public Selector selector;

	/// <summary>
	/// All the Gruntz in the current level.
	/// </summary>
	[Header("Gruntz")]
	public List<Grunt> allGruntz;

	public Grunt[] playerGruntz;
	public Grunt[] dizgruntled;

	/// <summary>
	/// The Gruntz currently selected by the player.
	/// </summary>
	public List<Grunt> selectedGruntz;

	[Header("Level Transformz")]
	public GameObject actorz;

	public GameObject itemz;
	public GameObject objectz;

	public GameObject helpboxUI;

	private void Awake() {
		instance = this;

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Application.targetFrameRate = 120;
		Time.timeScale = 0f;

		Cursor.visible = false;

		FindFirstObjectByType<GameCursor>().enabled = true;

		FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include).canvas.worldCamera =
			FindFirstObjectByType<CameraMovement>(FindObjectsInactive.Include).gameObject.GetComponent<Camera>();
	}

	public void InitUI() {
		Time.timeScale = 0f;
		Cursor.visible = false;

		FindFirstObjectByType<GameCursor>().enabled = true;

		GameObject.Find("SidebarUI").GetComponent<Canvas>().worldCamera =
			FindFirstObjectByType<CameraMovement>(FindObjectsInactive.Include).gameObject.GetComponent<Camera>();

		GameObject.Find("SidebarUI").GetComponent<Canvas>().enabled = true;
	}

	private void OnLoadStatzMenu() {
		Level.instance.gameObject.SetActive(false);
		actorz.SetActive(false);
		itemz.SetActive(false);
		objectz.SetActive(false);

		StatzMenu.Instance.Activate();
	}

	private void OnSwitchLocale() {
		if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1]) {
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
		} else {
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
		}
	}

	private void OnChangeGameSpeed() {
		// ReSharper disable once CompareOfFloatsByEqualityOperator
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
		levelName = newLevelName;
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

			FindFirstObjectByType<PauseMenu>().canvas.worldCamera =
				FindFirstObjectByType<CameraMovement>().gameObject.GetComponent<Camera>();

			gameManager.allGruntz = FindObjectsByType<Grunt>(FindObjectsSortMode.None).ToList();

			gameManager.playerGruntz = gameManager.allGruntz
				.Where(grunt => grunt.CompareTag("PlayerGrunt"))
				.ToArray();

			gameManager.dizgruntled = gameManager.allGruntz
				.Where(grunt => grunt.CompareTag("Dizgruntled"))
				.ToArray();

			// Set the sorting order of all EyeCandy objects so they render properly behind or in front of each other
			FindObjectsByType<EyeCandy>(FindObjectsSortMode.None)
				.Where(ec => ec.gameObject.CompareTag("HighEyeCandy"))
				.ToList()
				.ForEach(ec1 => ec1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(ec1.transform.position.y) * -1);

			FindObjectsByType<RollingBall>(FindObjectsSortMode.None)
				.ToList()
				.ForEach(rb => rb.Setup());

			List<Checkpoint> checkpointz = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).ToList();

			foreach (Checkpoint cp in checkpointz) {
				cp.Setup();
				EditorUtility.SetDirty(cp);
			}

			List<GridObject> gridObjects = FindObjectsByType<GridObject>(FindObjectsSortMode.None).ToList();

			foreach (GridObject go in gridObjects) {
				go.Setup();
				EditorUtility.SetDirty(go);
			}

			List<LevelItem> levelItems = FindObjectsByType<LevelItem>(FindObjectsSortMode.None).ToList();

			foreach (LevelItem li in levelItems) {
				li.Setup();
				EditorUtility.SetDirty(li);
			}

			EditorUtility.SetDirty(gameManager);

			Debug.Log($"Initialization took {DateTime.Now - start}");
		}
	}
}
#endif
}
