using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
public class Settings : MonoBehaviour {
	public static Settings instance;

	[Expandable]
	public GameSettings gameSettings;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);

			return;
		}

		instance = this;

		Addressables.LoadAssetAsync<GameSettings>("GameSettings").Completed += handle => gameSettings = handle.Result;
	}
}
}
