using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse {
  public class Modifier : MonoBehaviour {
    [field: SerializeField] public bool IsBurning { get; set; }
    [field: SerializeField] public bool IsDrowning { get; set; }
    [field: SerializeField] public bool IsElectric { get; set; }
    [field: SerializeField] public bool IsInaccessible { get; set; }
    private Vector2Int OwnLocation { get; set; }

    private void Awake() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Start() {
      gameObject.GetComponent<SpriteRenderer>().enabled = false;
      LevelManager.Instance.NodeAt(OwnLocation).isBurning = IsBurning;
      LevelManager.Instance.NodeAt(OwnLocation).isDrowning = IsDrowning;
      LevelManager.Instance.NodeAt(OwnLocation).isElectric = IsElectric;
      LevelManager.Instance.NodeAt(OwnLocation).isColliding = IsInaccessible;
    }
  }
}
