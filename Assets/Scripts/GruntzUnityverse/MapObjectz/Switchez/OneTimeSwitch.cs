using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class OneTimeSwitch : ObjectSwitch {
    private List<BlackPyramid> _pyramidz;


    protected override void Start() {
      base.Start();

      _pyramidz = transform.parent.GetComponentsInChildren<BlackPyramid>().ToList();
    }

    private void Update() {
      if (_pyramidz.Count.Equals(0)) {
        Debug.LogWarning("There is no Pyramid assigned to this Switch!");

        enabled = false;
      }

      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        TogglePyramidz();

        spriteRenderer.sprite = pressedSprite;
        enabled = false;
      }
    }

    private void TogglePyramidz() {
      foreach (BlackPyramid pyramid in _pyramidz) {
        pyramid.Toggle();
        pyramid.enabled = false;
      }
    }
  }
}
