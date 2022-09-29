using System.Collections.Generic;

using Singletonz;

using Switchez;

using UnityEngine;

namespace Pyramidz {
  public class CheckpointPyramid : MonoBehaviour {
    public CheckpointSwitch checkpointSwitch;
    public List<Sprite> animFrames;
    public SpriteRenderer spriteRenderer;

    private void Update() {
      if (!checkpointSwitch.isChecked) {
        return;
      }

      MapManager.Instance.AddNavTileAt(transform.position);

      foreach (Sprite frame in animFrames) {
        spriteRenderer.sprite = frame;
      }
    }
  }
}
