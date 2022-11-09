using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.Itemz {
  public class Toy : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    private List<Sprite> animFrames;
    private const int FrameRate = 12;

    public ToyType type;

    private void Start() {
      animFrames = Resources.LoadAll<Sprite>($"Animations/MapObjectz/Itemz/Toyz/Toy{type}").ToList();
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      foreach (
        Grunt grunt in MapManager.Instance.gruntz
          .Where(grunt => grunt.ownGridLocation.Equals(GridLocation))
      ) {
        grunt.toy = type;

        Destroy(gameObject);
      }

      int frame = (int)(Time.time * FrameRate % animFrames.Count);
      spriteRenderer.sprite = animFrames[frame];
    }
  }
}
