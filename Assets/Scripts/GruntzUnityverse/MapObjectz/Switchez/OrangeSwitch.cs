using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class OrangeSwitch : ObjectSwitch {
    public List<OrangeSwitch> otherSwitchez;
    private List<OrangePyramid> _pyramidz;

    protected override void Start() {
      base.Start();

      _pyramidz = parent.GetComponentsInChildren<OrangePyramid>().ToList();

      if (_pyramidz.Count.Equals(0)) {
        DisableWithError("There is no Orange Pyramid assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (isPressed || hasBeenPressed) {
          return;
        }

        PressSwitch();
        TogglePyramidz();
        ToggleOtherSwitchez();
      } else {
        hasBeenPressed = false;
      }
    }

    private void ToggleOtherSwitchez() {
      foreach (OrangeSwitch orangeSwitch in otherSwitchez) {
        orangeSwitch.ToggleSwitch();
      }
    }

    private void TogglePyramidz() {
      foreach (OrangePyramid pyramid in _pyramidz) {
        pyramid.Toggle();
      }
    }
  }
}
