using UnityEngine;

namespace Singletonz {
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


    private void Update() {
      transform.position = CustomStuff.SetSelectorCirclePosition(mainCamera.ScreenToWorldPoint(Input.mousePosition));
    }
  }
}
