using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class OrangeSwitch : MonoBehaviour {
    private SpriteRenderer Renderer { get; set; }
    [field: SerializeField] public Sprite PressedSprite { get; set; }
    [field: SerializeField] public Sprite ReleasedSprite { get; set; }
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public List<OrangeSwitch> OtherSwitchez { get; set; }
    [field: SerializeField] public List<OrangePyramid> Pyramidz { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }
    [field: SerializeField] public bool HasBeenPressed { get; set; }


    private void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update() {
      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (IsPressed) {
          return;
        }

        if (!HasBeenPressed) {
          IsPressed = true;
          HasBeenPressed = true;
          Renderer.sprite = PressedSprite;

          TogglePyramidz();
          ToggleOtherSwitchez();
        }
      }
    }

    private void ToggleOtherSwitchez() {
      foreach (OrangeSwitch orangeSwitch in OtherSwitchez) {
        orangeSwitch.IsPressed = !orangeSwitch.IsPressed;
        orangeSwitch.HasBeenPressed = false;

        orangeSwitch.Renderer.sprite = orangeSwitch.IsPressed
          ? PressedSprite
          : ReleasedSprite;
      }
    }

    private void TogglePyramidz() {
      foreach (OrangePyramid pyramid in Pyramidz) {
        pyramid.ChangeState();
      }
    }
  }
}
