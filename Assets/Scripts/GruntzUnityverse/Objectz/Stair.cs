using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Stair : MonoBehaviour {
    public bool isBlocked;
    private Vector2Int _ownLocation;


    private void Start() {
      _ownLocation = Vector2Int.FloorToInt(transform.position);
      LevelManager.Instance.SetBlockedAt(_ownLocation, isBlocked);
    }
  }
}
