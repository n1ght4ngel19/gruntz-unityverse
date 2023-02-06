using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class GreenToggleSwitch : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public List<GreenPyramid> Pyramidz { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }


    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }

    private void Update() {
      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
          TogglePyramidz();

          IsPressed = true;
          HasBeenPressed = true;
        }
      } else {
        IsPressed = false;
        HasBeenPressed = false;
      }
    }

    private void TogglePyramidz() {
      foreach (GreenPyramid pyramid in Pyramidz) {
        pyramid.ChangeState();
      }
    }
  }
}
