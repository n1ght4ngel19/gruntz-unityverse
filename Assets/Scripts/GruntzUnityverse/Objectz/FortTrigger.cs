using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz {
  public class FortTrigger : MapObject {
    public Fort mainFort;


    protected override void Start() {
      base.Start();

      spriteRenderer.enabled = false;
    }

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
