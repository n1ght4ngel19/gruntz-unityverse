using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class GreenToggleSwitch : ObjectSwitch {
    [field: SerializeField] public List<GreenPyramid> Pyramidz { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        if (!HasBeenPressed) {
          TogglePyramidz();
          PressSwitch();
        }
      } else {
        ReleaseSwitch();
      }
    }

    private void TogglePyramidz() {
      foreach (GreenPyramid pyramid in Pyramidz) {
        pyramid.TogglePyramid();
      }
    }
  }
}
