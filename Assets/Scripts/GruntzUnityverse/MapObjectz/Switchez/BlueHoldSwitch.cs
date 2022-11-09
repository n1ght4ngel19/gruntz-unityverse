using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class BlueHoldSwitch : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public Sprite[] animFrames;

    public bool isPressed;
    public bool isUntouched;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);

      isUntouched = true;
    }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.gruntz) {
        if (LevelManager.Instance.gruntz.Any(grunt1 => grunt1.ownGridLocation.Equals(GridLocation))) {
          if (!isUntouched) {
            continue;
          }

          spriteRenderer.sprite = animFrames[1];
          isUntouched = false;
          isPressed = true;
        }
        else {
          spriteRenderer.sprite = animFrames[0];
          isPressed = false;
          isUntouched = true;
        }
      }
    }
  }
}
