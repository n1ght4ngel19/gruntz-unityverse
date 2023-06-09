using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class OneTimeSwitch : ObjectSwitch {
    private List<BlackPyramid> Pyramidz { get; set; }


    protected override void Start() {
      base.Start();

      Pyramidz = transform.parent.GetComponentsInChildren<BlackPyramid>().ToList();
    }

    private void Update() {
      if (Pyramidz.Count.Equals(0)) {
        Debug.LogWarning("There is no Pyramid assigned to this Switch!");

        enabled = false;
      }

      if (!LevelManager.Instance.AllGruntz.Any(grunt => grunt.IsOnLocation(Location))) {
        return;
      }

      TogglePyramidz();

      Renderer.sprite = PressedSprite;
      enabled = false;
    }

    private void TogglePyramidz() {
      foreach (BlackPyramid pyramid in Pyramidz) {
        pyramid.TogglePyramid();
        pyramid.enabled = false;
      }
    }
  }
}
