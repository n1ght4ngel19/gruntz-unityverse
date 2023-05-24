using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
  public class SwitchableBridge : MapObject {
    [field: SerializeField] public bool IsDown { get; set; }

    public void ToggleBridge() {
      OwnAnimator.Play(IsDown ? "WaterBridge_Up" : "WaterBridge_Down");

      IsDown = !IsDown;
      LevelManager.Instance.SetBlockedAt(OwnLocation, IsDown);
    }
  }
}
