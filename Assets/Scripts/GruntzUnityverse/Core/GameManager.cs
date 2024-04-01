using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Objectz.Pyramidz;
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
	[Header("Level Data")]
	public string levelName;

	public Area area;

	/// <summary>
	/// All the Gruntz in the current level.
	/// </summary>
	[Header("Gruntz")]
	public List<Grunt> allGruntz;

	public List<Grunt> playerGruntz;

	public List<Grunt> dizgruntled;

	/// <summary>
	/// The Gruntz currently selected by the player.
	/// </summary>
	public List<Grunt> selectedGruntz;

	[Header("Cached Objectz")]
	public List<RedPyramid> redPyramidz;

	public List<GridObject> gridObjectz;

	public Grunt firstSelected => selectedGruntz.FirstOrDefault();

	[Header("User Interface")]
	public GooWell gooWell;

	/// <summary>
	/// The selector responsible for selecting and highlighting objects.
	/// </summary>
	public Selector selector;

	[Header("Level Transformz")]
	public GameObject actorz;

	public GameObject itemz;
	public GameObject objectz;

	public GameObject helpboxUI;

	private void Awake() {
		instance = this;

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void Start() {
		FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include).canvas.worldCamera =
			FindFirstObjectByType<CameraMovement>(FindObjectsInactive.Include).gameObject.GetComponent<Camera>();
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Application.targetFrameRate = 120;
		// Time.timeScale = 0f;

		Cursor.visible = false;

		FindFirstObjectByType<GameCursor>().enabled = true;

		Debug.Log("Disable Pause Menu Canvas");
		FindFirstObjectByType<PauseMenu>(FindObjectsInactive.Include).canvas.enabled = false;
	}

	public void Init() {
		Cursor.visible = false;

		FindFirstObjectByType<GameCursor>().enabled = true;

		GameObject.Find("SidebarUI").GetComponent<Canvas>().worldCamera =
			FindFirstObjectByType<CameraMovement>(FindObjectsInactive.Include).gameObject.GetComponent<Camera>();

		GameObject.Find("SidebarUI").GetComponent<Canvas>().enabled = true;

		Time.timeScale = 0f;

		allGruntz = FindObjectsByType<Grunt>(FindObjectsSortMode.None).ToList();

		playerGruntz = allGruntz
			.Where(grunt => grunt.CompareTag("PlayerGrunt"))
			.ToList();

		dizgruntled = allGruntz
			.Where(grunt => grunt.CompareTag("Dizgruntled"))
			.ToList();

		redPyramidz = FindObjectsByType<RedPyramid>(FindObjectsSortMode.None).ToList();

		gridObjectz = FindObjectsByType<GridObject>(FindObjectsSortMode.None).Where(go => go is not Blocker).ToList();

		gridObjectz = gridObjectz.Where(go => go is not Brick).ToList();

		FindObjectsByType<Blocker>(FindObjectsSortMode.None).ToList().ForEach(bl => bl.Setup());

		foreach (GridObject go in gridObjectz) {
			go.Setup();
		}
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

			gameManager.gooWell = FindFirstObjectByType<GooWell>();

			FindFirstObjectByType<PauseMenu>().canvas.worldCamera =
				FindFirstObjectByType<CameraMovement>().gameObject.GetComponent<Camera>();

			// Set the sorting order of all EyeCandy objects so they render properly behind or in front of each other
			// FindObjectsByType<EyeCandy>(FindObjectsSortMode.None)
			// 	.Where(ec => ec.gameObject.CompareTag("HighEyeCandy"))
			// 	.ToList()
			// 	.ForEach(ec1 => ec1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(ec1.transform.position.y) * -1);

			FindObjectsByType<RollingBall>(FindObjectsSortMode.None)
				.ToList()
				.ForEach(rb => rb.Setup());

			List<Checkpoint> checkpointz = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).ToList();

			foreach (Checkpoint cp in checkpointz) {
				cp.Setup();
				EditorUtility.SetDirty(cp);
			}

			List<LevelItem> levelItemz = FindObjectsByType<LevelItem>(FindObjectsSortMode.None).ToList();

			foreach (LevelItem li in levelItemz) {
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
