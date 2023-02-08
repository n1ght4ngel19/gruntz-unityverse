using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class CheckpointPyramid : Pyramid {
    [field: SerializeField] public List<CheckpointSwitch> Switchez { get; set; }
    [field: SerializeField] public bool HasChanged { get; set; }

    private void Update() {
      if (!IsInitialized && LevelManager.Instance.CheckpointPyramidz.Contains(this)) {
        InitializeNodeAtOwnLocation();
      }

      if (Switchez.Any(checkpointSwitch => !checkpointSwitch.IsPressed)) {
        return;
      }

      HasChanged = true;

      ChangeState();

      enabled = false;
    }
  }
}
