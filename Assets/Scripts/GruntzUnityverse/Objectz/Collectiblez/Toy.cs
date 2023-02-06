using System.Collections.Generic;
using System.Linq;
using Enumz;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Collectiblez {
  public class Toy : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    private List<Sprite> animFrames;
    private const int FrameRate = 12;

    public ToyType type;

    private void Start() {
      animFrames = Resources.LoadAll<Sprite>($"Animated Sprites/MapObjectz/Itemz/Toyz/Toy{type}").ToList();
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      foreach (
        Grunt grunt in LevelManager.Instance.gruntz
          .Where(grunt => grunt.NavComponent.OwnGridLocation.Equals(GridLocation))
      ) {
        grunt.toy = type;

        Destroy(gameObject);
      }

      int frame = (int)(Time.time * FrameRate % animFrames.Count);
      spriteRenderer.sprite = animFrames[frame];
    }
  }
}
