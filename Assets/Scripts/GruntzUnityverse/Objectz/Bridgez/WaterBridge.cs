using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class WaterBridge : MapObject {
    [field: SerializeField] public bool IsDown { get; set; }

    public void ToggleBridge() {
      Animator.Play(
        IsDown
          ? "WaterBridge_Up"
          : "WaterBridge_Down"
      );

      IsDown = !IsDown;
      LevelManager.Instance.SetBlockedAt(OwnLocation, IsDown);
    }
  }
}
