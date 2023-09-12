using System.Linq;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (!hasBeenPressed) {
          PressSwitch();
        }
      } else {
        ReleaseSwitch();
      }
    }
  }
}
