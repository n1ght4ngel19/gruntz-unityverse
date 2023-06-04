using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Bridgez;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueHoldSwitch : ObjectSwitch {
    private List<SwitchableBridge> Bridgez { get; set; }


    protected override void Start() {
      base.Start();

      Bridgez = transform.parent.GetComponentsInChildren<SwitchableBridge>().ToList();
    }

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
      foreach (SwitchableBridge bridge in Bridgez) {
        bridge.ToggleBridge();
      }
    }
  }
}
