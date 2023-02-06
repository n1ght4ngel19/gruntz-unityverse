using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueHoldSwitch : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public List<WaterBridge> Bridges { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }

    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }

    private void Update() {
      if (LevelManager.Instance.testGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          ToggleBridges();

          IsPressed = true;
          HasBeenPressed = true;
        }
      } else if (HasBeenPressed) {
        ToggleBridges();

        IsPressed = false;
        HasBeenPressed = false;
      }
    }

    private void ToggleBridges() {
      foreach (WaterBridge bridge in Bridges) {
        bridge.ChangeState();
      }
    }
  }
}
