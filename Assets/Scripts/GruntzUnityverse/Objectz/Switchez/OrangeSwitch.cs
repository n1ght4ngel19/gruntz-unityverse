using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class OrangeSwitch : ObjectSwitch {
    [field: SerializeField] public List<OrangeSwitch> OtherSwitchez { get; set; }
    [field: SerializeField] public List<OrangePyramid> Pyramidz { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        if (IsPressed) {
          return;
        }

        if (!HasBeenPressed) {
          PressSwitch();
          TogglePyramidz();
          ToggleOtherSwitchez();
        }
      } else {
        HasBeenPressed = false;
      }
    }

    private void ToggleOtherSwitchez() {
      foreach (OrangeSwitch orangeSwitch in OtherSwitchez) {
        orangeSwitch.ToggleSwitch();
      }
    }

    private void TogglePyramidz() {
      foreach (OrangePyramid pyramid in Pyramidz) {
        pyramid.TogglePyramid();
      }
    }
  }
}
