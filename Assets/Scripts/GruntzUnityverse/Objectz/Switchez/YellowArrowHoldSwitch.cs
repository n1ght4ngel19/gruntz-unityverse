using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class YellowArrowHoldSwitch : ObjectSwitch {
    public List<TwoWayArrow> arrowz;


    protected override void Start() {
      base.Start();

      int transformIndex = transform.parent.GetSiblingIndex();

      arrowz = parent.GetComponentsInChildren<TwoWayArrow>()
        .Where(arrow => arrow.transform.parent.GetSiblingIndex() == transformIndex)
        .ToList();
    }

    private void Update() {
      if (arrowz.Count == 0) {
        Debug.LogError(ErrorMessage.ArrowSwitchArrowzMissing + $"Switch: {transform.parent.name} -> {gameObject.name}");

        enabled = false;
      }

      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        ToggleArrowz();
        PressSwitch();
      } else if (hasBeenPressed) {
        ToggleArrowz();
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
