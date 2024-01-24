using UnityEngine;

namespace GruntzUnityverse.V2 {
  public class Selector : MonoBehaviour {
    public Vector2Int location2D;

    private void Update() {
      transform.position = Input.mousePosition.FromCamera().RoundedToIntWithCustomZ(15f);
      location2D = Vector2Int.RoundToInt(transform.position);
    }
  }
}
