using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class SelectorCircle : MapObject {
    private static SelectorCircle _instance;

    public static SelectorCircle Instance {
      get => _instance;
    }

    protected override void Start() {
      if (_instance is not null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      base.Start();
    }

    private void Update() {
      ownTransform.position = MousePositionAsVector3();
      location = Vector2Int.FloorToInt(ownTransform.position);

      if (LevelManager.Instance.nodeLocations.Contains(location)) {
        ownNode = LevelManager.Instance.NodeAt(location);
      }
    }

    private Vector3 MousePositionAsVector3() {
      return new Vector3(
        Mathf.Round(mainCamera.ScreenToWorldPoint(Input.mousePosition).x),
        Mathf.Round(mainCamera.ScreenToWorldPoint(Input.mousePosition).y),
        15f
      );
    }
  }
}
