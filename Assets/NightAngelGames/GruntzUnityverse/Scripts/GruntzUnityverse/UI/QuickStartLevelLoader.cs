using GruntzUnityverse.Core;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class QuickStartLevelLoader : LevelLoader {
	public override void LoadLevel() {
		Addressables.LoadSceneAsync("LoadMenu", LoadSceneMode.Additive, false).Completed += handle => {
			FindFirstObjectByType<GameCursor>().spriteRenderer.enabled = false;

			handle.Result.ActivateAsync().completed += _ => {
				GameObject.Find("LevelNameDisplay").GetComponent<TMP_Text>().SetText(levelData.levelName);

				GameObject.Find("AreaNameDisplay").GetComponent<TMP_Text>().SetText(levelData.areaName);

				GameObject.Find("LoadMenuBackground").GetComponent<Image>().sprite = levelData.loadMenuBackground;

				Addressables.LoadAssetAsync<GameSettings>("GameSettings").Completed += settingsHandle => {
					FindFirstObjectByType<ContinueToLevel>()
						.LoadLevel(settingsHandle.Result.quickStartLevel.levelKey);
				};
			};
		};
	}
}
}
