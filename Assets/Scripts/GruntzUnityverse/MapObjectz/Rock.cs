using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Rock : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }
  }
}
