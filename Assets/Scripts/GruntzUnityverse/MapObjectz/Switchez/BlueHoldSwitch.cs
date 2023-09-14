using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.Bridgez;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class BlueHoldSwitch : ObjectSwitch {
    private List<Bridge> _bridgez;


    protected override void Start() {
      base.Start();

      _bridgez = parent.GetComponentsInChildren<Bridge>().ToList();

      if (_bridgez.Count.Equals(0)) {
        DisableWithError("There is no Bridge assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        ToggleBridgez();
        PressSwitch();
      } else if (!hasBeenReleased) {
        ToggleBridgez();
        ReleaseSwitch();
      }
    }

    private void ToggleBridgez() {
      foreach (Bridge bridge in _bridgez) {
        bridge.Toggle();
      }
    }
  }
}
