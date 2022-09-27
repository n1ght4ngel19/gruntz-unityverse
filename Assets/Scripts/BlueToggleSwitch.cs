using UnityEngine;

public class BlueToggleSwitch : MonoBehaviour {
  public bool isPressed;
  public bool isUntouched;
  public Sprite[] animFrames;

  private void Start() {
    isPressed = false;
    isUntouched = true;
  }

  private void Update() {
    foreach (Grunt grunt in MapManager.Instance.gruntz) {
      if (
        grunt.transform.position.x == transform.position.x
        && grunt.transform.position.y == transform.position.y
      ) {
        if (isUntouched) {
          isUntouched = false;

          isPressed = !isPressed;
          gameObject.GetComponent<SpriteRenderer>().sprite = animFrames[1];
        }

        break;
      }

      isUntouched = true;
    }
  }
}
