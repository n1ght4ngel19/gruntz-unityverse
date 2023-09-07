using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class SilverTimerSwitch : ObjectSwitch {
    public List<SilverPyramid> pyramidz;
    private const float TimeStep = 0.1f;


    private void Update() {
      if (GameManager.Instance.currentLevelManager.allGruntz.Any(grunt => grunt.AtNode(ownNode))) {
        if (hasBeenPressed) {
          return;
        }

        HandleSilverPyramidz();

        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }

    private void HandleSilverPyramidz() {
      foreach (SilverPyramid pyramid in pyramidz) {
        StartCoroutine(HandleSilverPyramid(pyramid));
      }
    }

    private IEnumerator HandleSilverPyramid(SilverPyramid pyramid) {
      while (pyramid.Delay > 0) {
        pyramid.Delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      pyramid.Toggle();

      while (pyramid.Duration > 0) {
        pyramid.Duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      pyramid.Toggle();
    }
  }
}
