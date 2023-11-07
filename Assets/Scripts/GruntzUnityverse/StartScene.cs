using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  /// <summary>
  /// The starting point of the game. The Scene containing the GameObject that has this Component
  /// is the only Scene to be included in the Build Settings.
  /// </summary>
  public class StartScene : MonoBehaviour {
    private void Awake() {
      Addressables.LoadSceneAsync("Menuz/MainMenu.unity");
    }
  }
}
