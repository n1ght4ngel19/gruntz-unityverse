using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2.Core {
  public class Selector : MonoBehaviour {
    public Vector2Int location2D;
    public Camera mainCamera;

    private void Start() {
      mainCamera = Camera.main;
    }

    private void Update() {
      transform.position = Input.mousePosition.FromCameraView(mainCamera).RoundedToInt(15f);
      location2D = Vector2Int.RoundToInt(transform.position);
    }
  }
}
