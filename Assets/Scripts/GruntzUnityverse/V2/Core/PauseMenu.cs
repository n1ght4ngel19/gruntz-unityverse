using UnityEngine;

namespace GruntzUnityverse.V2.Core {
  public class PauseMenu : MonoBehaviour {


    private void OnEscape() {
      Time.timeScale = Time.timeScale == 0f ? 1f : 0f;

      Debug.Log("Pause Menu Toggled");
    }
  }
}
