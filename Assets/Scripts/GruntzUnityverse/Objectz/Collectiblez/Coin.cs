using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Collectiblez {
  public class Coin : MonoBehaviour {
    public Vector2Int OwnLocation { get; set; }

    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }

    private void RemoveFromGame() {
      StatzManager.Instance.acquiredCoinz++;

      Destroy(gameObject);
    }
  }
}
