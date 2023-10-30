using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    private void Update() {
      if (IsBeingPressed()) {
        if (hasBeenPressed) {
          return;
        }

        PressSwitch();
      } else if (!hasBeenReleased) {
        ReleaseSwitch();
      }
    }
  }
}
