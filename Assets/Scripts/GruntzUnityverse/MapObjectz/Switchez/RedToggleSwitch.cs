using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class RedToggleSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
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
        pyramid.Toggle();
      }
    }
  }
}
