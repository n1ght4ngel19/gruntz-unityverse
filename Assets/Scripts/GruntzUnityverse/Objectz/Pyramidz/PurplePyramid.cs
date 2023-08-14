using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class PurplePyramid : Pyramid {
    public List<PurpleSwitch> switchez;
    public bool hasChanged;

    protected override void Start() {
      base.Start();

      switchez = transform.parent.GetComponentsInChildren<PurpleSwitch>().ToList();
    }

    private void Update() {
      if (switchez.Any(purpleSwitch => !purpleSwitch.isPressed)) {
        if (!hasChanged) {
          return;
        }

        Toggle();
        hasChanged = false;

        return;
      }

      if (hasChanged) {
        return;
      }

      Toggle();
      hasChanged = true;
    }
  }
}
