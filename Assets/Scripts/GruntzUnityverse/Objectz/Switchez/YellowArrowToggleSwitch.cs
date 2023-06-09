using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class YellowArrowToggleSwitch : ObjectSwitch {
    [field: SerializeField] public List<TwoWayArrow> Arrowz { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(Location))) {
        if (HasBeenPressed) {
          return;
        }

        ToggleArrowz();
        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }

    private void ToggleArrowz() {
      foreach (TwoWayArrow arrow in Arrowz) {
        arrow.ChangeDirection();
      }
    }
  }
}
