using GruntzUnityverse.Enumz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  /// <summary>
  /// Provides various ways of loading Scenes.
  /// </summary>
  public class SceneLoader : MonoBehaviour {
    public Area area;

    /// <summary>
    /// Loads a Scene based on its Addressable address.
    /// </summary>
    /// <param name="sceneName">The address of the Scene to be loaded.</param>
    public void LoadSceneOfName(string sceneName) {
      Addressables.LoadSceneAsync(sceneName).Completed += handle => {
        GameManager.Instance.hasChangedMusic = false;
      };
    }
  }
}
