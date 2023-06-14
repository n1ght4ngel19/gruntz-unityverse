using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Stair : MonoBehaviour {
    [field: SerializeField] public bool IsColliding { get; set; }
    private Vector2Int OwnLocation { get; set; }

    private void Awake() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Start() {
      LevelManager.Instance.NodeAt(OwnLocation).isInaccessible = IsColliding;
    }
  }
}
