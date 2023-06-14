using System.Linq;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.AtLocation(Location))) {
        if (!HasBeenPressed) {
          PressSwitch();
        }
      } else {
        ReleaseSwitch();
      }
    }
  }
}
