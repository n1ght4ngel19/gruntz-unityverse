using System.Linq;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(Location))) {
        if (!HasBeenPressed) {
          PressSwitch();
        }
      } else {
        ReleaseSwitch();
      }
    }
  }
}
