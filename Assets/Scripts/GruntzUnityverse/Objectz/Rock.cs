using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Rock : MonoBehaviour {
    public Vector2Int OwnLocation { get; set; }

    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }
  }
}
