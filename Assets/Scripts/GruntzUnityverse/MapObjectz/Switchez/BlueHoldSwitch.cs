using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Bridgez;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class BlueHoldSwitch : ObjectSwitch {
    private List<Bridge> Bridgez { get; set; }


    protected override void Start() {
      base.Start();

      Bridgez = transform.parent.GetComponentsInChildren<Bridge>().ToList();
    }

    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        ToggleBridgez();
        PressSwitch();
      } else if (hasBeenPressed) {
        ToggleBridgez();
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
