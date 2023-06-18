using GruntzUnityverse.Enumz;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse {
  public class SceneLoader : MonoBehaviour {
    public Area area;
    public SceneAsset scene;
    public string sceneName;

    public void LoadScene() {
      // SceneManager.LoadScene(Resources.Load<SceneAsset>($"Levelz/{area}/{sceneName}").name);
      SceneManager.LoadScene(scene.name);
    }
  }
}
