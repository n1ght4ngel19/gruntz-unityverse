using Animancer;
using GruntzUnityverse.Core;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse.UI {
public class StatzMenu : MonoBehaviour {
	public Canvas canvas;
	public TMP_Text timeValue;
	public TMP_Text survivorzValue;
	public TMP_Text deathzValue;
	public TMP_Text toolzValue;
	public TMP_Text toyzValue;
	public TMP_Text powerupzValue;
	public TMP_Text coinzValue;
	public TMP_Text secretzValue;

	public static StatzMenu instance { get; private set; }

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(gameObject);
		} else {
			instance = this;
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
		Application.targetFrameRate = 120;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (canvas != null) {
			canvas.enabled = false;
		}
	}

	public void Activate() {
		canvas.worldCamera = FindFirstObjectByType<CameraMovement>().gameObject.GetComponent<Camera>();
		canvas.enabled = true;

		timeValue.text = Level.instance.levelStatz.secondzTaken.ToString();

		Addressables.LoadAssetAsync<AnimationClip>("Helpbox_Rotating").Completed += handle => {
			GameObject.Find("TimeIcon").GetComponent<AnimancerComponent>().Play(handle.Result);
		};

		survivorzValue.text = FindFirstObjectByType<GameManager>().playerGruntz.Count.ToString();

		Addressables.LoadAssetAsync<AnimationClip>("Grunt_Exit_01_Loop").Completed += handle => {
			GameObject.Find("SurvivorzIcon").GetComponent<AnimancerComponent>().Play(handle.Result);
		};

		deathzValue.text = Level.instance.levelStatz.deathz.ToString();

		Addressables.LoadAssetAsync<AnimationClip>("GruntPuddle_Bubbling").Completed += handle => {
			GameObject.Find("DeathzIcon").GetComponent<AnimancerComponent>().Play(handle.Result);
		};

		toolzValue.text = $"{Level.instance.levelStatz.toolzCollected.ToString()} OF {Level.instance.levelStatz.maxToolz.ToString()}";

		Addressables.LoadAssetAsync<AnimationClip>("Gauntletz_Rotating").Completed += handle => {
			GameObject.Find("ToolzIcon").GetComponent<AnimancerComponent>().Play(handle.Result);
		};

		toyzValue.text = $"{Level.instance.levelStatz.toyzCollected.ToString()} OF {Level.instance.levelStatz.maxToyz.ToString()}";

		Addressables.LoadAssetAsync<AnimationClip>("SqueakToy_Rotating").Completed += handle => {
			GameObject.Find("ToyzIcon").GetComponent<AnimancerComponent>().Play(handle.Result);
		};

		powerupzValue.text = $"{Level.instance.levelStatz.powerupzCollected.ToString()} OF {Level.instance.levelStatz.maxPowerupz.ToString()}";

		Addressables.LoadAssetAsync<AnimationClip>("Roidz_Rotating").Completed += handle => {
			GameObject.Find("PowerupzIcon").GetComponent<AnimancerComponent>().Play(handle.Result);
		};

		coinzValue.text = $"{Level.instance.levelStatz.coinzCollected.ToString()} OF {Level.instance.levelStatz.maxCoinz.ToString()}";

		Addressables.LoadAssetAsync<AnimationClip>("Coin_Rotating").Completed += handle => {
			GameObject.Find("CoinzIcon").GetComponent<AnimancerComponent>().Play(handle.Result);
		};

		secretzValue.text = $"{Level.instance.levelStatz.discoveredSecretz.ToString()} OF {Level.instance.levelStatz.maxSecretz.ToString()}";
	}

	private void OnEscape() {
		if (GetComponent<Canvas>().enabled) {
			Addressables.LoadSceneAsync("MainMenu");
		}
	}
}
}
