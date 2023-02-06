using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class BlueHoldSwitch : MonoBehaviour {
    public Vector2Int GridLocation {get; set;}
    public bool IsPressed { get; set; }

    public SpriteRenderer spriteRenderer;

    public Sprite[] animFrames;

    public bool isUntouched;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);

      isUntouched = true;
    }

    private void Update() {
      if (LevelManager.Instance.gruntz.Any(grunt1 => grunt1.NavComponent.OwnGridLocation.Equals(GridLocation))) {
        if (!isUntouched) {
          return;
        }

        spriteRenderer.sprite = animFrames[1];
        isUntouched = false;
        IsPressed = true;
      }
      else {
        spriteRenderer.sprite = animFrames[0];
        IsPressed = false;
        isUntouched = true;
      }
    }

    public SpriteRenderer Renderer { get; set; }
    public Sprite DisplayFrame { get; set; }
    public List<Sprite> AnimationFrames { get; set; }
  }
}
