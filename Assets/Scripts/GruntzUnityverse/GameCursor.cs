using UnityEngine;

namespace GruntzUnityverse {
  public class GameCursor : MonoBehaviour {
    private int Counter { get; set; }

    private void Awake() {
      Cursor.visible = false;
    }
    // ------------------------------------------------------------ //

    private void Update() {
      Counter = Counter % 10 + 1;

      transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 15;
    }
  }
}
