using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Landing : MonoBehaviour {
    private Vector2Int _ownLocation;
    // ------------------------------------------------------------ //

    private void Start() {
      _ownLocation = Vector2Int.FloorToInt(transform.position);
      LevelManager.Instance.SetBlockedAt(_ownLocation, false);
      LevelManager.Instance.SetWaterAt(_ownLocation, false);
      enabled = false;
    }
  }
}
