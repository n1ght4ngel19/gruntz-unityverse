using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class RedHoldSwitch : ObjectSwitch {
    [field: SerializeField] public bool HasBeenPressed { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          ToggleAllRedPyramidz();

          IsPressed = true;
          HasBeenPressed = true;
          Renderer.sprite = PressedSprite;
        }
      } else if (HasBeenPressed) {
        ToggleAllRedPyramidz();

        IsPressed = false;
        HasBeenPressed = false;
        Renderer.sprite = ReleasedSprite;
      }
    }

    private void ToggleAllRedPyramidz() {
      foreach (RedPyramid pyramid in LevelManager.Instance.RedPyramidz) {
        pyramid.ChangeState();
      }
    }
  }
}
