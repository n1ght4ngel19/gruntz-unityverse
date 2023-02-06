using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class CheckpointPyramid : Pyramid {
    [field: SerializeField] public List<CheckpointSwitch> Switches { get; set; }
    [field: SerializeField] public bool HasChanged { get; set; }


    private void Start() {
      if (Switches.Count.Equals(0)) {
        Debug.LogError("There is no Switch assigned to this Pyramid, this way the Checkpoint won't work properly!");
      }

      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update() {
      if (Switches.Any(checkpointSwitch => !checkpointSwitch.IsPressed)) {
        return;
      }

      HasChanged = true;

      ChangeState();

      enabled = false;
    }
  }
}
