using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class GreenToggleSwitch : ObjectSwitch {
    private List<GreenPyramid> _pyramidz;


    protected override void Start() {
      base.Start();

      _pyramidz = parent.GetComponentsInChildren<GreenPyramid>().ToList();

      if (_pyramidz.Count.Equals(0)) {
        DisableWithError("There is no Green Pyramid assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        TogglePyramidz();
        PressSwitch();
      } else if (!hasBeenReleased) {
        ReleaseSwitch();
      }
    }

    private void TogglePyramidz() {
      foreach (GreenPyramid pyramid in _pyramidz) {
        pyramid.Toggle();
      }
    }
  }
}
