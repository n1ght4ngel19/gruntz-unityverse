using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GruntzUnityverse {
  public class SceneLoader : MonoBehaviour {
    [field: SerializeField] public string AreaName { get; set; }
    [field: SerializeField] public string SceneName { get; set; }

    public void LoadScene() {
      SceneManager.LoadScene(Resources.Load<SceneAsset>($"Levelz/{AreaName}/{SceneName}").name);
    }
  }
}
