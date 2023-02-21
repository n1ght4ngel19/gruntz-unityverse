using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class SelectorCircle : MapObject {
    private static SelectorCircle _instance;

    public static SelectorCircle Instance {
      get => _instance;
    }

    private void Awake() {
      if (_instance != null && _instance != this)
        Destroy(gameObject);
      else
        _instance = this;
    }

    public Camera mainCamera;


    private void Update() {
      transform.position = SetPositionBasedOnMousePosition();

      OwnLocation = Vector2Int.FloorToInt(transform.position);
    }

    private Vector3 SetPositionBasedOnMousePosition() {
      return new Vector3(
        Mathf.Floor(mainCamera.ScreenToWorldPoint(Input.mousePosition).x) + 0.5f,
        Mathf.Floor(mainCamera.ScreenToWorldPoint(Input.mousePosition).y) + 0.5f,
        -10f
      );
    }
  }
}
