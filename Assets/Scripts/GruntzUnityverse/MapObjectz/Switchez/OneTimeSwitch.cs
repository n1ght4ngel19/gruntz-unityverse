using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Pyramidz;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class OneTimeSwitch : ObjectSwitch {
    private List<BlackPyramid> _pyramidz;


    protected override void Start() {
      base.Start();

      _pyramidz = parent.GetComponentsInChildren<BlackPyramid>().ToList();
      
      if (_pyramidz.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Pyramid assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (IsBeingPressed()) {
        TogglePyramidz();
        PressSwitch();
      }
    }

    protected override void PressSwitch() {
      spriteRenderer.sprite = pressedSprite;
      enabled = false;
    }

    private void TogglePyramidz() {
      foreach (BlackPyramid pyramid in _pyramidz) {
        pyramid.Toggle();
        pyramid.enabled = false;
      }
    }
  }
}
