using System.Linq;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(OwnLocation))) {
        if (!HasBeenPressed) {
          PressSwitch();
        }
      } else {
        ReleaseSwitch();
      }
    }
  }
}
