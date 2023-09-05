using System;
using GruntzUnityverse.MapObjectz.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse {
  public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    public static GameManager Instance {
      get => _instance;
    }

    public bool isDebugMode;

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Escape)) {
        Debug.Log(SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == "StatzMenu") {
          Addressables.LoadSceneAsync("Menuz/MainMenu.unity");
        }
      }
    }
  }
}
