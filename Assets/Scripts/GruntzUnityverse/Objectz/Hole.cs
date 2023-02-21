using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Hole : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public bool IsOpen { get; set; }

    private void Start() { OwnLocation = Vector2Int.FloorToInt(transform.position); }

    private void Update() {
      if (!IsOpen) {
        return;
      }

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz) {
        if (grunt.NavComponent.OwnLocation.Equals(OwnLocation)) {

          StartCoroutine(grunt.FallInHole());
        }
      }
    }
  }
}
