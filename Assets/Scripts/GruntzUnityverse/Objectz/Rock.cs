using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Rock : MapObject {
    [field: SerializeField] public bool IsInitialized { get; set; }
    public bool isBlockedHere;

    private void Update() {
      if (!IsInitialized) {
        InitializeNodeAtOwnLocation();
      }
    }

    protected void InitializeNodeAtOwnLocation() {
      IsInitialized = true;
      LevelManager.Instance.SetBlockedAt(OwnLocation, true);
    }
  }
}
