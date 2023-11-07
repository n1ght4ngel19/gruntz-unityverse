using UnityEngine;

namespace GruntzUnityverse {
  public class GameCursor : MonoBehaviour {
    private void Awake() {
      Cursor.visible = false;
    }

    private void Update() {
      transform.position =
        Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 15;
    }
  }
}
