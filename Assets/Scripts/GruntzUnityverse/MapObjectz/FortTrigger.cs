using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.MapObjectz {
  public class FortTrigger : MapObject {
    public Fort fort;
    // ------------------------------------------------------------ //

    protected override void Start() {
      base.Start();

      spriteRenderer.enabled = false;
    }
    // ------------------------------------------------------------ //

    private void Update() {
      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.playerGruntz) {
        if (grunt.AtNode(ownNode) && grunt.HasTool(ToolName.Warpstone)) {
          GameManager.Instance.currentLevelManager.isLevelCompleted = true;
          enabled = false;
        }
      }
    }
  }
}
