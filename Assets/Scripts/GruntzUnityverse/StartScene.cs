using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  public class StartScene : MonoBehaviour {
    private void Awake() {
      Addressables.LoadSceneAsync("Menuz/MainMenu.unity");
    }
  }
}
