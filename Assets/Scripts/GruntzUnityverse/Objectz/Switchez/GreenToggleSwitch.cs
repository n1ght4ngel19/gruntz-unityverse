using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class GreenToggleSwitch : ObjectSwitch {
    [field: SerializeField] public List<GreenPyramid> Pyramidz { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }


    private void Update() {
      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          TogglePyramidz();

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

    private void TogglePyramidz() {
      foreach (GreenPyramid pyramid in Pyramidz) {
        pyramid.ChangeState();
      }
    }
  }
}
