using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class OrangeSwitch : ObjectSwitch {
    public List<OrangeSwitch> otherSwitchez;
    private List<OrangePyramid> _pyramidz;


    protected override void Start() {
      base.Start();

      _pyramidz = transform.parent.GetComponentsInChildren<OrangePyramid>().ToList();
    }

    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
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
