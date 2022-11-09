using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Coin : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> animFrames;
    private const int FrameRate = 12;

    private void Start() {
      animFrames = Resources.LoadAll<Sprite>("Animations/MapObjectz/Misc/Coin").ToList();
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      foreach (
        Grunt grunt in MapManager.Instance.gruntz
          .Where(grunt1 => grunt1.ownGridLocation.Equals(GridLocation))
      ) {
        RemoveFromGame();
      }

      int frame = (int)(Time.time * FrameRate % animFrames.Count);
      spriteRenderer.sprite = animFrames[frame];
    }

    private void RemoveFromGame() {
      StatzManager.Instance.acquiredCoinz++;
      Destroy(gameObject);
    }
  }
}
