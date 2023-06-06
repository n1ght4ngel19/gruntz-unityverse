using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class SelectorCircle : MapObject {
    private static SelectorCircle _instance;

    public static SelectorCircle Instance {
      get => _instance;
    }


    protected override void Awake() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      base.Awake();
    }

    private void Update() {
      OwnTransform.position = MousePositionAsVector3();
      OwnLocation = Vector2Int.FloorToInt(OwnTransform.position);

      if (LevelManager.Instance.nodeLocationsList.Contains(OwnLocation)) {
        OwnNode = LevelManager.Instance.NodeAt(OwnLocation);
      }
    }

    private Vector3 MousePositionAsVector3() {
      return new Vector3(
        Mathf.Floor(MainCamera.ScreenToWorldPoint(Input.mousePosition).x) + 0.5f,
        Mathf.Floor(MainCamera.ScreenToWorldPoint(Input.mousePosition).y) + 0.5f,
        75f
      );
    }
  }
}
