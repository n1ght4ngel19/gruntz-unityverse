using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.UI {
public class LevelLoader : MonoBehaviour {
	public string sceneToLoad;
	public TMP_Text levelNameText;

	public void LoadLevel() {
		Addressables.LoadSceneAsync(sceneToLoad);
	}

	public void SetText(string toSet) {
		levelNameText.SetText(toSet);
	}
}
}
