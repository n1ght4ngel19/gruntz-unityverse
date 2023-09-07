using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class SelectorCircle : MapObject {
    private void Update() {
      ownTransform.position = MousePositionAsVector3();
      location = Vector2Int.FloorToInt(ownTransform.position);

      if (GameManager.Instance.currentLevelManager.nodeLocations.Contains(location)) {
        ownNode = GameManager.Instance.currentLevelManager.NodeAt(location);
      }
    }
    // ------------------------------------------------------------ //

    private Vector3 MousePositionAsVector3() {
      return new Vector3(Mathf.Round(mainCamera.ScreenToWorldPoint(Input.mousePosition).x),
        Mathf.Round(mainCamera.ScreenToWorldPoint(Input.mousePosition).y),
        15f);
    }
  }
}
