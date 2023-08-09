using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Bridgez;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueToggleSwitch : ObjectSwitch {
    private List<Bridge> _bridgez;


    protected override void Start() {
      base.Start();

      _bridgez = transform.parent.GetComponentsInChildren<Bridge>().ToList();
    }

    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (!HasBeenPressed) {
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
