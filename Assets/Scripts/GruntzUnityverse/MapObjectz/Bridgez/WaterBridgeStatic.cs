using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Bridgez {
  public class WaterBridgeStatic : MonoBehaviour, IMapObject {
    public Vector2Int GridLocation {get; set;}

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }
  }
}
