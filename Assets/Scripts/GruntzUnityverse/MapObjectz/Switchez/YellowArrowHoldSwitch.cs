using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class YellowArrowHoldSwitch : ObjectSwitch {
    public List<TwoWayArrow> arrowz;

    protected override void Start() {
      base.Start();

      // Todo: WTF
      int transformIndex = parent.GetSiblingIndex();

      arrowz = parent.GetComponentsInChildren<TwoWayArrow>()
        .Where(arrow => arrow.parent.GetSiblingIndex() == transformIndex)
        .ToList();
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
