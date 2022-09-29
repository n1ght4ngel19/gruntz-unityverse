using System.Collections.Generic;

using Singletonz;

using Switchez;

using UnityEngine;

namespace Pyramidz {
  public class CheckpointPyramid : MonoBehaviour {
    public CheckpointSwitchTool toolCheckpointSwitch;
    public CheckpointSwitchToy toyCheckpointSwitch;
    public List<Sprite> animFrames;
    public SpriteRenderer spriteRenderer;

    private void Update() {
      if (toolCheckpointSwitch) {
        if (!toolCheckpointSwitch.isChecked) {
          return;
        }
      }

      if (toyCheckpointSwitch) {
        if (!toyCheckpointSwitch.isChecked) {
          return;
        }
      }

      MapManager.Instance.AddNavTileAt(transform.position);

      foreach (Sprite frame in animFrames) {
        spriteRenderer.sprite = frame;
      }
    }
  }
}
