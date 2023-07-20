using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class FortTrigger : MapObject {
    public Fort mainFort;

    private void Update() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
        if (grunt.AtLocation(OwnNode.OwnLocation) && grunt.HasTool(ToolName.Warpstone)) {
          LevelManager.Instance.isLevelCompleted = true;
          enabled = false;
        }
      }
    }
  }
}
