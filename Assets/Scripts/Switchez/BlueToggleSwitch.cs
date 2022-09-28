using System.Linq;

using UnityEngine;

public class BlueToggleSwitch : MonoBehaviour {
  public SpriteRenderer spriteRenderer;
  public bool isPressed;
  public bool isUntouched;
  public Sprite[] animFrames;

  private void Start() {
    isUntouched = true;
  }

  private void Update() {
    foreach (Grunt grunt in MapManager.Instance.gruntz) {
      if (
        MapManager.Instance.gruntz.Any(grunt1 => (Vector2)grunt1.transform.position == (Vector2)transform.position)
      ) {
        if (isUntouched) {
          spriteRenderer.sprite = animFrames[1];
          isUntouched = false;
          isPressed = isPressed switch {
            true => false,
            false => true
          };
        }
      } else {
        spriteRenderer.sprite = animFrames[0];
        isUntouched = true;
      }
    }
  }
}

