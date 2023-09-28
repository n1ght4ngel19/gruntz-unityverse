using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse {
  public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance {
      get => _instance;
    }

    public LevelManager currentLevelManager;
    public AnimationManager currentAnimationManager;
    public SelectorCircle selectorCircle;
    public AudioListener audioListener;
    public AudioSource audioSource;
    public bool hasChangedMusic;

    /// <summary>
    /// The threshold for the frequency of the voice clip played when a Grunt is selected.
    /// The higher the number, the more often a clip will play. A value of 10 means that a
    /// clip is played every time a Grunt is selected.
    /// </summary>
    [Range(0, 10)]
    public int selectVoicePlayFrequency;

    private void Awake() {
      if (_instance is not null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      DontDestroyOnLoad(gameObject);
    }

    private void Start() {
      audioListener = gameObject.GetComponent<AudioListener>();
      audioSource = gameObject.GetComponent<AudioSource>();
      hasChangedMusic = true;
    }

    private void Update() {
      SceneManager.sceneLoaded += (scene, mode) => {
        Time.timeScale = 1f;
        if (scene.name != "StatzMenu" || scene.name != "MainMenu") {
          currentLevelManager = FindObjectOfType<LevelManager>();
          currentLevelManager.enabled = true;
          currentLevelManager.gameObject.GetComponent<Controller>().enabled = true;

          currentAnimationManager = FindObjectOfType<AnimationManager>();
          currentAnimationManager.enabled = true;

          selectorCircle = FindObjectOfType<SelectorCircle>();

          StatzManager.Clean();
        }
      };

      if (!hasChangedMusic) {
        if (SceneManager.GetActiveScene().name == "StatzMenu") {
          Addressables.LoadAssetAsync<AudioClip>("StatzMenuLoop.wav").Completed += handle => {
            audioSource.clip = handle.Result;
            audioSource.loop = true;
            audioSource.Play();
          };
        } else if (SceneManager.GetActiveScene().name == "MainMenu") {
          Addressables.LoadAssetAsync<AudioClip>("MenuLoop.wav").Completed += handle => {
            audioSource.clip = handle.Result;
            audioSource.loop = true;
            audioSource.Play();
          };
        } else {
          audioSource.loop = false;
          audioSource.Stop();
        }

        hasChangedMusic = true;
      }

      if (Input.GetKeyDown(KeyCode.Escape)) {
        if (SceneManager.GetActiveScene().name == "StatzMenu") {
          Addressables.LoadSceneAsync("Menuz/MainMenu.unity").Completed += handle => {
            hasChangedMusic = false;
          };
        }
      }

      bool isComboPressed =
        Input.GetKeyDown(KeyCode.LeftControl)
        && Input.GetKeyDown(KeyCode.LeftShift)
        && Input.GetKeyDown(KeyCode.T);

      if (isComboPressed) {
        Addressables.LoadSceneAsync("TestLevel.unity");
      }
    }

    public static void QuitGame() {
      Application.Quit();
    }
  }
}