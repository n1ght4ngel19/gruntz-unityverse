using UnityEngine;

namespace GruntzUnityverse.V2.Core {
  public class PauseMenu : MonoBehaviour {

    private void OnSaveGame() {
      Debug.Log("Save game");
    }

    private void OnLoadGame() {
      Debug.Log("Load game");
    }

    private void OnEscape() {
      Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
      
      if (Time.timeScale == 0f) {
        Debug.Log("Show pause menu");
      } else {
        Debug.Log("Resume the game (do nothing)");
      }
    }
  }
}
