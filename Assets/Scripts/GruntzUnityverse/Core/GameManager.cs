using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Misc;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Misc;
using GruntzUnityverse.Objectz.Switchez;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.Core {
public class GameManager : MonoBehaviour {
	/// <summary>
	/// The singleton accessor of the GM.
	/// </summary>
	public static GameManager Instance { get; private set; }

	/// <summary>
	/// The name of the current Level (not the scene name/address).
	/// </summary>
	public string levelName;

	/// <summary>
	/// The statz of the current level (completion).
	/// </summary>
	public LevelStatz levelStatz;

	/// <summary>
	/// The selector responsible for selecting and highlighting objects.
	/// </summary>
	public Selector selector;

	/// <summary>
	/// All the Gruntz in the current level.
	/// </summary>
	[Header("Gruntz")]
	public List<Grunt> allGruntz;

	/// <summary>
	/// The Gruntz currently selected by the player.
	/// </summary>
	public List<Grunt> selectedGruntz;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Debug.Log("Destroying self, GM already exists.");
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
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		GameManager gameManager = (GameManager)target;
		Level level = FindFirstObjectByType<Level>();

		if (GUILayout.Button("Initialize")) {
			DateTime start = DateTime.Now;

			level.Initialize();

			gameManager.allGruntz = FindObjectsByType<Grunt>(FindObjectsSortMode.None).ToList();

			// Set the sorting order of all EyeCandy objects so they render properly behind or in front of each other
			FindObjectsByType<EyeCandy>(FindObjectsSortMode.None)
				.Where(ec => ec.gameObject.CompareTag("HighEyeCandy"))
				.ToList()
				.ForEach(ec1 => ec1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(ec1.transform.position.y) * -1);

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

			gameManager.levelStatz.maxToolz = FindObjectsByType<LevelTool>(FindObjectsSortMode.None).Length;
			gameManager.levelStatz.maxToyz = FindObjectsByType<LevelToy>(FindObjectsSortMode.None).Length;
			gameManager.levelStatz.maxPowerupz = FindObjectsByType<LevelPowerup>(FindObjectsSortMode.None).Length;
			gameManager.levelStatz.maxCoinz = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;
			gameManager.levelStatz.maxSecretz = FindObjectsByType<SecretSwitch>(FindObjectsSortMode.None).Length;
			gameManager.levelStatz.maxWarpletterz = FindObjectsByType<Warpletter>(FindObjectsSortMode.None).Length;

			EditorUtility.SetDirty(gameManager);

			Debug.Log($"Initialization took {DateTime.Now - start}");
		}
	}
}
#endif
}
