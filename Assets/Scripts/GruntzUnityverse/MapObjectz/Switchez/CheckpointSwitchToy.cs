using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Itemz;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class CheckpointSwitchToy : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> animFrames;

    public ToyType requirement;
    public bool isChecked;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      if (isChecked) {
        return;
      }

      if (MapManager.Instance.gruntz
          .Any(grunt => grunt.ownGridLocation.Equals(GridLocation) && grunt.toy == requirement)) {
        isChecked = true;
        spriteRenderer.sprite = animFrames[1];
      }
    }
  }
}
