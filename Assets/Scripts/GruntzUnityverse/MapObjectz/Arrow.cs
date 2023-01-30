using GruntzUnityverse.Utilitiez;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Arrow : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public CompassDirection direction;

    private void Start()
    {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }
  }
}
