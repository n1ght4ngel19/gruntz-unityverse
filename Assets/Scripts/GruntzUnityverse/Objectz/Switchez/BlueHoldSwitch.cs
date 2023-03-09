using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueHoldSwitch : ObjectSwitch {
    [field: SerializeField] public List<WaterBridge> Bridgez { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        if (HasBeenPressed) {
          return;
        }

        ToggleBridgez();
        PressSwitch();
      } else if (HasBeenPressed) {
        ToggleBridgez();
        ReleaseSwitch();
      }
    }

    private void ToggleBridgez() {
      foreach (WaterBridge bridge in Bridgez) {
        bridge.ToggleBridge();
      }
    }
  }
}
