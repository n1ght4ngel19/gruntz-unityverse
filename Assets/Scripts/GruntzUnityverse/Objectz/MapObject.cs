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
    
    // Todo: Move initialization needed for all MapObjectz here
    // Set OwnNode so features that need the Object's Node are much easier to handle
  }
}
