using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class PurplePyramid : Pyramid {
    [field: SerializeField] public List<PurpleSwitch> Switchez { get; set; }
    [field: SerializeField] public bool HasChanged { get; set; }

    private void Update() {
      if (Switchez.Any(purpleSwitch => !purpleSwitch.IsPressed)) {
        if (!HasChanged) {
          return;
        }

        TogglePyramid();
        HasChanged = false;

        return;
      }

      if (HasChanged) {
        return;
      }

      TogglePyramid();
      HasChanged = true;
    }
  }
}
