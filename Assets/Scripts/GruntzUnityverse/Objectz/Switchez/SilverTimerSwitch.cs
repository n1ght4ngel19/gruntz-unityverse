using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class SilverTimerSwitch : ObjectSwitch {
    [field: SerializeField] public List<SilverPyramid> Pyramidz { get; set; }
    private const float TimeStep = 0.1f;


    private void Update() {
      if (LevelManager.Instance.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (HasBeenPressed) {
          return;
        }

        HandleSilverPyramidz();

        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }

    private void HandleSilverPyramidz() {
      foreach (SilverPyramid pyramid in Pyramidz) {
        StartCoroutine(HandleSilverPyramid(pyramid));
      }
    }

    private IEnumerator HandleSilverPyramid(SilverPyramid pyramid) {
      while (pyramid.Delay > 0) {
        pyramid.Delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      pyramid.TogglePyramid();

      while (pyramid.Duration > 0) {
        pyramid.Duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      pyramid.TogglePyramid();
    }
  }
}
