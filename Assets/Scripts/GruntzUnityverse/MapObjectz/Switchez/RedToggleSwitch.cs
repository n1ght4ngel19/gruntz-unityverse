using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class RedToggleSwitch : ObjectSwitch {
    private void Update() {
      if (IsBeingPressed()) {
        if (hasBeenPressed) {
          return;
        }

        ToggleAllRedPyramidz();
        PressSwitch();
      } else if (!hasBeenReleased) {
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
