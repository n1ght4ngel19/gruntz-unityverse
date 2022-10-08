using GruntzUnityverse.Utilitiez;

using UnityEngine;

namespace GruntzUnityverse.Singletonz {
  public class SelectorCircle : MonoBehaviour {
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
    public Vector2 twoDimPosition;


    private void Update() {
      twoDimPosition = transform.position;
      transform.position = Positioning.SetSelectorCirclePosition(mainCamera.ScreenToWorldPoint(Input.mousePosition));
    }
  }
}
