﻿using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class Pyramid : MapObject {
    [field: SerializeField] public bool IsDown { get; set; }


    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsDown);
    }

    public void TogglePyramid() {
      
      Animator.Play(Resources.load);
      Animator.Play(IsDown ? "Pyramid_Up" : "Pyramid_Down");

      IsDown = !IsDown;
      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsDown);
    }
  }
}
