using System.Collections;
using System.Collections.Generic;

using GruntzUnityverse.MapObjectz.Switchez;
using GruntzUnityverse.Singletonz;

using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Pyramidz {
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

      StartCoroutine(LowerPyramid());

      MapManager.Instance.AddNavTileAt(transform.position);
    }

    private IEnumerator LowerPyramid() {
      foreach (Sprite frame in animFrames) {
        spriteRenderer.sprite = frame;

        yield return null;
      }
    }
  }
}
