using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Brickz {
  public class BrickColumn : MonoBehaviour {
    private Vector2Int Location { get; set; }

    private void Start() {
      Location = Vector2Int.FloorToInt(transform.position);

      LevelManager.Instance.BrickColumnz.Add(this);
      LevelManager.Instance.SetInaccessibleAt(Location, true);
      LevelManager.Instance.NodeAt(Location).isHardTurn = true;
    }
  }
}
