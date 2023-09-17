using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class FortTrigger : MapObject {
    public Fort ownFort;

    protected override void Start() {
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      spriteRenderer.enabled = false;
    }

    private void Update() {
      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.playerGruntz) {
        if (!grunt.AtNode(ownNode) || !grunt.HasTool(ToolName.Warpstone)) {
          continue;
        }

        GameManager.Instance.currentLevelManager.isLevelCompleted = true;
        enabled = false;
      }
    }
  }
}
