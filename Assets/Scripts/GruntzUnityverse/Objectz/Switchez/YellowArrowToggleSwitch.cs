using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class YellowArrowToggleSwitch : ObjectSwitch {
    [field: SerializeField] public bool HasBeenPressed { get; set; }
    [field: SerializeField] public List<TwoWayArrow> Arrowz { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          ToggleArrowz();

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

    private void ToggleArrowz() {
      foreach (TwoWayArrow arrow in Arrowz) {
        arrow.ChangeDirection();
      }
    }
  }
}
