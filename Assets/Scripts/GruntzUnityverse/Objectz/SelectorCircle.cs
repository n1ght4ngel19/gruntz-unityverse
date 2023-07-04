using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class SelectorCircle : MapObject {
    private static SelectorCircle _Instance;

    public static SelectorCircle Instance {
      get => _Instance;
    }

    protected override void Start() {
      if (_Instance != null && _Instance != this) {
        Destroy(gameObject);
      } else {
        _Instance = this;
      }

      base.Start();
    }

    private void Update() {
      OwnTransform.position = MousePositionAsVector3();
      Location = Vector2Int.FloorToInt(OwnTransform.position);

      if (LevelManager.Instance.nodeLocations.Contains(Location)) {
        OwnNode = LevelManager.Instance.NodeAt(Location);
      }
    }

    private Vector3 MousePositionAsVector3() {
      return new Vector3(
        Mathf.Round(MainCamera.ScreenToWorldPoint(Input.mousePosition).x),
        Mathf.Round(MainCamera.ScreenToWorldPoint(Input.mousePosition).y),
        75f
      );
    }
  }
}
