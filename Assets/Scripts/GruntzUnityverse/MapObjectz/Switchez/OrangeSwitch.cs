using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class OrangeSwitch : ObjectSwitch {
    public List<OrangeSwitch> otherSwitchez;
    private List<OrangePyramid> _pyramidz;

    protected override void Start() {
      base.Start();

      _pyramidz = parent.GetComponentsInChildren<OrangePyramid>().ToList();
      otherSwitchez = parent.parent.GetComponentsInChildren<OrangeSwitch>().Where(sw => sw != this).ToList();

      if (_pyramidz.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Orange Pyramid assigned to this Switch, this way the Switch won't work properly!");
      }

      if (otherSwitchez.Count.Equals(0)) {
        WarnWithSpriteChange("There is no other Orange Switch assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (IsBeingPressed()) {
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

    protected override void ReleaseSwitch() {
      base.ReleaseSwitch();

      TogglePyramidz();
    }

    private void ToggleOtherSwitchez() {
      foreach (OrangeSwitch orangeSwitch in otherSwitchez.Where(sw => sw.isPressed)) {
        orangeSwitch.ReleaseSwitch();
      }
    }

    private void TogglePyramidz() {
      foreach (OrangePyramid pyramid in _pyramidz) {
        pyramid.Toggle();
      }
    }
  }
}
