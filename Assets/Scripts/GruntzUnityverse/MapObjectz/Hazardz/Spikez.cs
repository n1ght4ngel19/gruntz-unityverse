using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Hazardz {
  public class Spikez : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public const int Dps = 2;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }
  }
}
