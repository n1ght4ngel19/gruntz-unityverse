using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class RedToggleSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        if (!HasBeenPressed) {
          ToggleAllRedPyramidz();
          PressSwitch();
        }
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
