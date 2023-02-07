﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class SilverTimerSwitch : ObjectSwitch {
    [field: SerializeField] public List<SilverPyramid> Pyramidz { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }
    private const float TimeStep = 0.1f;


    private void Update() {
      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          foreach (SilverPyramid pyramid in Pyramidz) {
            StartCoroutine(HandleSilverPyramid(pyramid));
          }

          IsPressed = true;
          HasBeenPressed = true;
          Renderer.sprite = PressedSprite;
        }
      } else {
        IsPressed = false;
        HasBeenPressed = false;
        Renderer.sprite = ReleasedSprite;
      }
    }

    private IEnumerator HandleSilverPyramid(SilverPyramid pyramid) {
      while (pyramid.Delay > 0) {
        pyramid.Delay -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      pyramid.ChangeState();

      while (pyramid.Duration > 0) {
        pyramid.Duration -= TimeStep;

        yield return new WaitForSeconds(TimeStep);
      }

      pyramid.ChangeState();
    }
  }
}
