using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueToggleSwitch : ObjectSwitch {
    [field: SerializeField] public List<SwitchableBridge> Bridgez { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        if (HasBeenPressed) {
          return;
        }

        ToggleBridgez();
        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }

    private void ToggleBridgez() {
      foreach (SwitchableBridge bridge in Bridgez) {
        bridge.ToggleBridge();
      }
    }
  }
}
