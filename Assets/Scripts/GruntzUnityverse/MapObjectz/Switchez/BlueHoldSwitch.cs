using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Bridgez;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class BlueHoldSwitch : ObjectSwitch {
    private List<Bridge> _bridgez;

    protected override void Start() {
      base.Start();

      _bridgez = parent.GetComponentsInChildren<Bridge>().ToList();

      if (_bridgez.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Bridge assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (IsBeingPressed()) {
        if (hasBeenPressed) {
          return;
        }

        PressSwitch();
        ToggleBridgez();
      } else if (!hasBeenReleased) {
        ReleaseSwitch();
        ToggleBridgez();
      }
    }

    private void ToggleBridgez() {
      _bridgez.ForEach(bridge => bridge.Toggle());
    }
  }
}
