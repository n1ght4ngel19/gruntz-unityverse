using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class CheckpointSwitchTool : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> animFrames;

    public ToolType requirement;
    public bool isChecked;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
      LevelManager.Instance.mapNodes.First(node => node.GridLocation.Equals(GridLocation)).isBlocked = false;
    }

    private void Update() {
      if (isChecked) {
        return;
      }

      if (LevelManager.Instance.gruntz
          .Any(grunt => grunt.ownGridLocation.Equals(GridLocation) && grunt.tool == requirement)) {
        isChecked = true;
        spriteRenderer.sprite = animFrames[1];
      }
    }
  }
}
