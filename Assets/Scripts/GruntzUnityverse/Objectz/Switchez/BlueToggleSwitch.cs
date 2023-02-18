using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Bridgez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueToggleSwitch : ObjectSwitch {
    [field: SerializeField] public List<WaterBridge> Bridgez { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          ToggleBridgez();

          IsPressed = true;
          HasBeenPressed = true;
          Renderer.sprite = PressedSprite;
        }
      } else {
        IsPressed = false;
        HasBeenPressed = false;
        Renderer.sprite = ReleasedSprite;
      }
    }

    private void ToggleBridgez() {
      foreach (WaterBridge bridge in Bridgez) {
        bridge.ChangeState();
      }
    }
  }
}
