using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class PurplePyramid : Pyramid {
    [field: SerializeField] public List<PurpleSwitch> Switchez { get; set; }
    [field: SerializeField] public bool HasChanged { get; set; }

    private void Start() {
      if (Switchez.Count.Equals(0)) {
        Debug.LogError("There is no Switch assigned to this Pyramid!");
      }

      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Update() {
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
