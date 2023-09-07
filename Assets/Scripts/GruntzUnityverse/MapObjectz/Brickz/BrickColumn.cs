using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Brickz {
  public class BrickColumn : MonoBehaviour {
    private Vector2Int Location { get; set; }

    private void Start() {
      Location = Vector2Int.FloorToInt(transform.position);

      GameManager.Instance.currentLevelManager.BrickColumnz.Add(this);
      GameManager.Instance.currentLevelManager.SetBlockedAt(Location, true);
      GameManager.Instance.currentLevelManager.NodeAt(Location).isHardTurn = true;
    }
  }
}
