using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class FortTrigger : MapObject {
    public Fort Main { get; set; }

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
        if (grunt.navigator.OwnNode == OwnNode) {
          Time.timeScale = 0f;
        }
      }
    }
  }
}
