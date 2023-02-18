using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class PurpleSwitch : ObjectSwitch {
    [field: SerializeField] public bool HasBeenPressed { get; set; }


    private void Update() {
      if (LevelManager.Instance.AllGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        if (!HasBeenPressed) {
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
  }
}
