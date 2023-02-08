using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class PurplePyramid : Pyramid {
    [field: SerializeField] public List<PurpleSwitch> Switchez { get; set; }
    [field: SerializeField] public bool HasChanged { get; set; }

    private void Update() {
      if (!IsInitialized && LevelManager.Instance.PurplePyramidz.Contains(this)) {
        InitializeNodeAtOwnLocation();
      }

      if (Switchez.Any(purpleSwitch => !purpleSwitch.IsPressed)) {
        if (HasChanged) {
          ChangeState();
          HasChanged = false;
        }

        return;
      }

      if (!HasChanged) {
        ChangeState();
        HasChanged = true;
      }
    }
  }
}
