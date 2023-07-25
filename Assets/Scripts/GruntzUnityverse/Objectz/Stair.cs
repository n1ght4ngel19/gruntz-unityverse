using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Stair : MapObject {
    public bool isBlocked;


    private void Update() {
      LevelManager.Instance.SetBlockedAt(location, isBlocked);
      enabled = false;
    }
  }
}
