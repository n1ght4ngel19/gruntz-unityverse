﻿using GruntzUnityverse.Core;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class LevelLoader : MonoBehaviour {
	[Expandable]
	public LevelData levelData;

	public TMP_Text buttonText;

	[BoxGroup("Old")]
	public string levelKey;

	[BoxGroup("Old")]
	public string levelName;

	[BoxGroup("Old")]
	public string areaName;

	[BoxGroup("Old")]
	public Sprite loadMenuBackground;

	public virtual void LoadLevel() {
		Addressables.LoadSceneAsync("LoadMenu", LoadSceneMode.Additive, false).Completed += handle => {
			FindFirstObjectByType<GameCursor>().spriteRenderer.enabled = false;

			handle.Result.ActivateAsync().completed += _ => {
				GameObject.Find("LevelNameDisplay").GetComponent<TMP_Text>().SetText(levelData.levelName);

				GameObject.Find("AreaNameDisplay").GetComponent<TMP_Text>().SetText(levelData.areaName);

				GameObject.Find("LoadMenuBackground").GetComponent<Image>().sprite = levelData.loadMenuBackground;

				FindFirstObjectByType<ContinueToLevel>().LoadLevel(levelData.levelKey);
			};
		};
	}

	public void LocalizeLevelName(string toSet) {
		buttonText.SetText(toSet);
		levelName = toSet;
	}

	public void LocalizeAreaName(string toSet) {
		areaName = toSet;
	}
}
}