using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.Arrowz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class YellowArrowToggleSwitch : ObjectSwitch {
    public List<TwoWayArrow> arrowz;


    protected override void Start() {
      base.Start();

      arrowz = parent.GetComponentsInChildren<TwoWayArrow>().ToList();
    }

    private void Update() {
      if (arrowz.Count == 0) {
        Debug.LogError(ErrorMessage.ArrowSwitchArrowzMissing + $"Switch: {parent.name} -> {gameObject.name}");

        enabled = false;
      }

      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        PressSwitch();
        ToggleArrowz();
      } else if (!hasBeenReleased) {
        ReleaseSwitch();
      }
    }

    private void ToggleArrowz() {
      foreach (TwoWayArrow arrow in arrowz) {
        arrow.ChangeDirection();
      }
    }
  }
}
