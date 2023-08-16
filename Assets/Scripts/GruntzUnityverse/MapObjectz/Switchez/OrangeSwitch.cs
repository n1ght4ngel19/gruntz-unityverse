using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class OrangeSwitch : ObjectSwitch {
    [field: SerializeField] public List<OrangeSwitch> OtherSwitchez { get; set; }
    private List<OrangePyramid> Pyramidz { get; set; }


    protected override void Start() {
      base.Start();

      Pyramidz = transform.parent.GetComponentsInChildren<OrangePyramid>().ToList();
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
      foreach (OrangeSwitch orangeSwitch in OtherSwitchez) {
        orangeSwitch.ToggleSwitch();
      }
    }

    private void TogglePyramidz() {
      foreach (OrangePyramid pyramid in Pyramidz) {
        pyramid.Toggle();
      }
    }
  }
}
