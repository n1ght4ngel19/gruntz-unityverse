using System.Linq;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (!HasBeenPressed) {
          PressSwitch();
        }
      } else {
        ReleaseSwitch();
      }
    }
  }
}
