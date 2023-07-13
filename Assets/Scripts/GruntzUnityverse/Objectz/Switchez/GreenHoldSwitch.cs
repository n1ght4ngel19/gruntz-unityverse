using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class GreenHoldSwitch : ObjectSwitch {
    private List<GreenPyramid> Pyramidz { get; set; }


    protected override void Start() {
      base.Start();

      Pyramidz = transform.parent.GetComponentsInChildren<GreenPyramid>().ToList();
    }


    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtLocation(location))) {
        if (HasBeenPressed) {
          return;
        }

        TogglePyramidz();
        PressSwitch();
      } else if (HasBeenPressed) {
        TogglePyramidz();
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
