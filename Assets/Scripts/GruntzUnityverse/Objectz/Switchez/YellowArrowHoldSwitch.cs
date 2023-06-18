using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Arrowz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class YellowArrowHoldSwitch : ObjectSwitch {
    [field: SerializeField] public List<TwoWayArrow> Arrowz { get; set; }


    protected override void Start() {
      base.Start();

      int transformIndex = transform.parent.GetSiblingIndex();

      Arrowz = transform.parent.GetComponentsInChildren<TwoWayArrow>()
        .Where(arrow => arrow.transform.parent.GetSiblingIndex() == transformIndex)
        .ToList();
    }

    private void Update() {
      if (Arrowz.Count == 0) {
        Debug.LogError(ErrorMessage.ArrowSwitchArrowzMissing + $"Switch: {transform.parent.name} -> {gameObject.name}");

        enabled = false;
      }

      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtLocation(Location))) {
        if (HasBeenPressed) {
          return;
        }

        ToggleArrowz();
        PressSwitch();
      } else if (HasBeenPressed) {
        ToggleArrowz();
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
