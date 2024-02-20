using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Itemz;
using GruntzUnityverse.V2.Itemz.Collectiblez;
using GruntzUnityverse.V2.Objectz;
using GruntzUnityverse.V2.Objectz.Switchez;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tool = GruntzUnityverse.V2.Itemz.Tool;

namespace GruntzUnityverse.V2.Core {
public class GM : MonoBehaviour {
	/// <summary>
	/// The singleton accessor of the GM.
	/// </summary>
	public static GM Instance { get; private set; }

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
	public List<GruntFSM> allGruntz;

	/// <summary>
	/// The Gruntz currently selected by the player.
	/// </summary>
	public List<GruntV2> selectedGruntz;

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

		Debug.Log($"Loaded {scene.name}");

		allGruntz = FindObjectsByType<GruntFSM>(FindObjectsSortMode.None).ToList();

		// Set the sorting order of all EyeCandy objects so they render properly behind or in front of each other
		FindObjectsByType<EyeCandy>(FindObjectsSortMode.None)
			.ToList()
			.ForEach(go => go.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(go.transform.position.y) * -1);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(GM))]
public class GMEditor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		GM gm = (GM)target;
		LevelV2 level = FindFirstObjectByType<LevelV2>();

		if (GUILayout.Button("Initialize")) {
			DateTime start = DateTime.Now;

			level.Initialize();

			gm.allGruntz = FindObjectsByType<GruntFSM>(FindObjectsSortMode.None).ToList();

			// Set the sorting order of all EyeCandy objects so they render properly behind or in front of each other
			FindObjectsByType<EyeCandy>(FindObjectsSortMode.None)
				.ToList()
				.ForEach(go => go.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(go.transform.position.y) * -1);

			List<CheckpointV2> checkpointz = FindObjectsByType<CheckpointV2>(FindObjectsSortMode.None).ToList();

			foreach (CheckpointV2 cp in checkpointz) {
				cp.Setup();
				EditorUtility.SetDirty(cp);
			}

			List<GridObject> gridObjects = FindObjectsByType<GridObject>(FindObjectsSortMode.None).ToList();

			foreach (GridObject go in gridObjects) {
				go.Setup();
				EditorUtility.SetDirty(go);
			}

			gm.levelStatz.maxToolz = FindObjectsByType<Tool>(FindObjectsSortMode.None).Length;
			gm.levelStatz.maxToyz = FindObjectsByType<Toy>(FindObjectsSortMode.None).Length;
			gm.levelStatz.maxPowerupz = FindObjectsByType<Powerup>(FindObjectsSortMode.None).Length;
			gm.levelStatz.maxCoinz = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;
			gm.levelStatz.maxSecretz = FindObjectsByType<SecretSwitchV2>(FindObjectsSortMode.None).Length;

			EditorUtility.SetDirty(gm);

			Debug.Log($"Initialization took {DateTime.Now - start}");
		}
	}
}
#endif
}
