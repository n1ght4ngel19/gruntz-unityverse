using GruntzUnityverse.Core;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class LevelLoader : MonoBehaviour {
	public TMP_Text buttonText;
	public string levelKey;
	public string levelName;
	public string areaName;
	public Sprite loadMenuBackground;

	public void LoadLevel() {
		Addressables.LoadSceneAsync("LoadMenu", LoadSceneMode.Additive, false).Completed += handle => {
			FindFirstObjectByType<GameCursor>().spriteRenderer.enabled = false;

			handle.Result.ActivateAsync().completed += _ => {
				GameObject.Find("LoadMenuBackground").GetComponent<Image>().sprite = loadMenuBackground;
				GameObject.Find("LevelNameDisplay").GetComponent<TMP_Text>().SetText(levelName);
				GameObject.Find("AreaNameDisplay").GetComponent<TMP_Text>().SetText(areaName);
				FindFirstObjectByType<ContinueToLevel>().LoadLevel(levelKey);
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
