using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class BlueToggleSwitch : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public Sprite[] animFrames;

    public bool IsPressed { get; set; }
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

        IsPressed = IsPressed switch {
          true => false,
          false => true
        };
      } else {
        spriteRenderer.sprite = animFrames[0];
        isUntouched = true;
      }
    }
  }
}
