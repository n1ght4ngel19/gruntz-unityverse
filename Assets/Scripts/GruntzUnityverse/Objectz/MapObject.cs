using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class MapObject : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    protected SpriteRenderer Renderer { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }

    protected virtual void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      Animator = gameObject.GetComponent<Animator>();
    }
  }
}
