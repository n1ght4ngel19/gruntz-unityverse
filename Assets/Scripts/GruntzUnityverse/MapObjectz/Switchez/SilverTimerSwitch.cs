using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class SilverTimerSwitch : ObjectSwitch {
    private List<SilverPyramid> _pyramidz;

    protected override void Start() {
      base.Start();

      _pyramidz = parent.GetComponentsInChildren<SilverPyramid>().ToList();

      if (_pyramidz.Count.Equals(0)) {
        WarnWithSpriteChange("There is no Silver Pyramid assigned to this Switch, this way the Switch won't work properly!");
      }
    }

    private void Update() {
      if (IsBeingPressed()) {
        if (hasBeenPressed) {
          return;
        }

        HandleSilverPyramidz();

        PressSwitch();
      } else if (!hasBeenReleased) {
        ReleaseSwitch();
      }
    }

    private void HandleSilverPyramidz() {
      foreach (SilverPyramid pyramid in _pyramidz) {
        StartCoroutine(HandleSilverPyramid(pyramid));
      }
    }

    private IEnumerator HandleSilverPyramid(SilverPyramid pyramid) {
      pyramid.Toggle();

      yield return new WaitForSeconds(pyramid.initialDelay);

      pyramid.Toggle();


      foreach (float delay in pyramid.toggleDelayz) {
        yield return new WaitForSeconds(delay);

        pyramid.Toggle();
      }
    }
  }
}
