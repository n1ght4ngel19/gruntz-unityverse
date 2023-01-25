using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Itemz {
  public class Tool : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    private List<Sprite> animFrames;
    private const int FrameRate = 12;

    public ToolType type;

    private void Start() {
      animFrames = Resources.LoadAll<Sprite>($"Animated Sprites/MapObjectz/Itemz/Toolz/Tool{type}").ToList();
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      foreach (
        Grunt grunt in LevelManager.Instance.gruntz
          .Where(grunt => grunt.NavComponent.OwnGridLocation.Equals(GridLocation))
      ) {
        grunt.tool = type;
        grunt.SelectGruntAnimationPack(type);

        Destroy(gameObject);
      }

      int frame = (int)(Time.time * FrameRate % animFrames.Count);
      spriteRenderer.sprite = animFrames[frame];
    }
  }
}
