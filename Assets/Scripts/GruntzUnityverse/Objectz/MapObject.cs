using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class MapObject : MonoBehaviour {
    #region Fieldz

    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Node OwnNode { get; set; }
    protected SpriteRenderer Renderer { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsInitialized { get; set; }

    #endregion


    protected virtual void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      OwnNode = LevelManager.Instance.nodeList.First(node => node.GridLocation.Equals(OwnLocation));
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      Animator = gameObject.GetComponent<Animator>();
    }
  }
}
