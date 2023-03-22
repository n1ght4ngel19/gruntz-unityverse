using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  /// <summary>
  /// The base for all Objects that can be interacted with on a Level.
  /// </summary>
  public class MapObject : MonoBehaviour {
        [field: SerializeField] public Vector2Int OwnLocation { get; set; }
        [field: SerializeField] public Node OwnNode { get; set; }
    protected SpriteRenderer Renderer { get; set; }
    protected Animator Animator { get; set; }


    protected virtual void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      OwnNode = LevelManager.Instance.NodeAt(OwnLocation);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      Animator = gameObject.GetComponent<Animator>();
    }
  }
}
