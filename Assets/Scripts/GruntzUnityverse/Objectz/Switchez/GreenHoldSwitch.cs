﻿using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class GreenHoldSwitch : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public List<GreenPyramid> Pyramids { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }


    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }

    private void Update() {
      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          TogglePyramids();

          IsPressed = true;
          HasBeenPressed = true;
        }
      } else if (HasBeenPressed) {
        TogglePyramids();

        IsPressed = false;
        HasBeenPressed = false;
      }
    }

    private void TogglePyramids() {
      foreach (GreenPyramid pyramid in Pyramids) {
        pyramid.ChangeState();
      }
    }
  }
}
