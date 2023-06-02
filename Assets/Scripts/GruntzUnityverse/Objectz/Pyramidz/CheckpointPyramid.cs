using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class CheckpointPyramid : Pyramid {
    [field: SerializeField] public bool HasChanged { get; set; }
    public List<CheckpointSwitch> Switchez { get; set; }

    protected override void Start() {
      base.Start();

      Switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();
    }

    private void Update() {
      if (Switchez.Any(checkpointSwitch => !checkpointSwitch.IsPressed)) {
        return;
      }

      TogglePyramid();

      HasChanged = true;
      enabled = false;
    }
  }
}
