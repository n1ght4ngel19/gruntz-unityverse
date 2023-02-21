using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class MapObject : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] protected SpriteRenderer Renderer { get; set; }

    protected virtual void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
    }
  }
}
