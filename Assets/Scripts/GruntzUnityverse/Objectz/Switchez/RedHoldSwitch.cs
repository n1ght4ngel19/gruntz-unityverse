using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class RedHoldSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        if (!HasBeenPressed) {
          ToggleAllRedPyramidz();
          PressSwitch();
        }
      } else if (HasBeenPressed) {
        ToggleAllRedPyramidz();
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
