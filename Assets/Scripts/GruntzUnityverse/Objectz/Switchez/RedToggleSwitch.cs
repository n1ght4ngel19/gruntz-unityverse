using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class RedToggleSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.AtLocation(Location))) {
        if (HasBeenPressed) {
          return;
        }

        ToggleAllRedPyramidz();
        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }

    private void ToggleAllRedPyramidz() {
      foreach (RedPyramid pyramid in LevelManager.Instance.RedPyramidz) {
        pyramid.TogglePyramid();
      }
    }
  }
}
