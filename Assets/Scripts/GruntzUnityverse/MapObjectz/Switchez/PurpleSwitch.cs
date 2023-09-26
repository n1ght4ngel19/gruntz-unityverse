using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    private void Update() {
      if (IsBeingPressed()) {
        if (!hasBeenPressed) {
          PressSwitch();
        }
      } else {
        ReleaseSwitch();
      }
    }
  }
}
