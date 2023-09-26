using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class GreenHoldSwitch : ObjectSwitch {
    private List<GreenPyramid> _pyramidz;


    protected override void Start() {
      base.Start();

      _pyramidz = parent.GetComponentsInChildren<GreenPyramid>().ToList();

      if (_pyramidz.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Green Pyramid assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (IsBeingPressed()) {
        if (hasBeenPressed) {
          return;
        }

        TogglePyramidz();
        PressSwitch();
      } else if (hasBeenPressed) {
        TogglePyramidz();
        ReleaseSwitch();
      }
    }

    private void TogglePyramidz() {
      foreach (GreenPyramid pyramid in _pyramidz) {
        pyramid.Toggle();
      }
    }
  }
}
