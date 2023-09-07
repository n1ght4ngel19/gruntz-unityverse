using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class GreenToggleSwitch : ObjectSwitch {
    private List<GreenPyramid> _pyramidz;


    protected override void Start() {
      base.Start();

      _pyramidz = transform.parent.GetComponentsInChildren<GreenPyramid>().ToList();
    }

    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        TogglePyramidz();
        PressSwitch();
      } else {
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
