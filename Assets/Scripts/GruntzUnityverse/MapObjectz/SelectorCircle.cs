using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class SelectorCircle : MonoBehaviour, IMapObject {
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
    public Vector2Int GridLocation {get; set;}

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      transform.position = SetPositionBasedOnMousePosition();
      GridLocation = Vector2Int.FloorToInt(transform.position);
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
