using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Bridgez;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class BlueToggleSwitch : ObjectSwitch {
    private List<Bridge> _bridgez;


    protected override void Start() {
      base.Start();

      _bridgez = transform.parent.GetComponentsInChildren<Bridge>().ToList();
    }

    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (!hasBeenPressed) {
          PressSwitch();
          ToggleBridgez();
        }
      } else {
        ReleaseSwitch();
      }
    }

    public void ToggleBridgez() {
      foreach (Bridge bridge in _bridgez) {
        bridge.Toggle();
      }
    }
  }
}
