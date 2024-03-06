using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.UI {
public class LevelLoader : MonoBehaviour {
	public SceneAsset sceneToLoad;
	public TMP_Text levelNameText;

	public void LoadLevel() {
		Addressables.LoadSceneAsync(sceneToLoad.name);
	}

	public void SetText(string toSet) {
		levelNameText.SetText(toSet);
	}
}
}
