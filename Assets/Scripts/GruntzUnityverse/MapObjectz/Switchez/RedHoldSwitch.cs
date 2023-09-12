using System.Linq;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class RedHoldSwitch : ObjectSwitch {
    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        ToggleAllRedPyramidz();
        PressSwitch();
      } else if (hasBeenPressed) {
        ToggleAllRedPyramidz();
        ReleaseSwitch();
      }
    }

    private void ToggleAllRedPyramidz() {
      foreach (RedPyramid pyramid in GameManager.Instance.currentLevelManager.RedPyramidz) {
        pyramid.Toggle();
      }
    }
  }
}
