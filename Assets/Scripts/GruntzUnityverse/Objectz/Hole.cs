using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Hole : MapObject {
    [field: SerializeField] public bool IsOpen { get; set; }


    private void Update() {
      if (!IsOpen) {
        return;
      }

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz) {
        if (grunt.IsOnLocation(OwnLocation)) {
          StartCoroutine(grunt.FallInHole());
        }
      }
    }
  }
}
