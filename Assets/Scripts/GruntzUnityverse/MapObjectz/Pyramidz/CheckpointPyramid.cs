using System.Collections;
using System.Collections.Generic;
using GruntzUnityverse.MapObjectz.Switchez;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Pyramidz {
  public class CheckpointPyramid : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> animFrames;

    public CheckpointSwitchTool toolCheckpointSwitch;
    public CheckpointSwitchToy toyCheckpointSwitch;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      if (!toolCheckpointSwitch.isChecked && !toolCheckpointSwitch.isChecked) {
        return;
      }

      // if (toolCheckpointSwitch) {
      //   if (!toolCheckpointSwitch.isChecked) {
      //     return;
      //   }
      // }
      //
      // if (toyCheckpointSwitch) {
      //   if (!toyCheckpointSwitch.isChecked) {
      //     return;
      //   }
      // }

      StartCoroutine(LowerPyramid());
      MapManager.Instance.UnblockNodeAt(GridLocation);
    }

    private IEnumerator LowerPyramid() {
      foreach (Sprite frame in animFrames) {
        spriteRenderer.sprite = frame;

        yield return null;
      }
    }
  }
}
