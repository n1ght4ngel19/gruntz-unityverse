﻿using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Bridgez;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueToggleSwitch : ObjectSwitch {
    private List<Bridge> Bridgez { get; set; }


    protected override void Start() {
      base.Start();

      Bridgez = transform.parent.GetComponentsInChildren<Bridge>().ToList();
    }

    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtLocation(location))) {
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
      foreach (Bridge bridge in Bridgez) {
        bridge.Toggle();
      }
    }
  }
}
