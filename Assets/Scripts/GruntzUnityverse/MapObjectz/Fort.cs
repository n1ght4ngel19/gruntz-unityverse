using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Fort : MonoBehaviour, IMapObject {
    public SpriteRenderer spriteRenderer;
    public Vector2Int GridLocation {get; set;}

    public List<Sprite> animFrames;
    private const int FrameRate = 12;

    private void Start() {
      GridLocation = Vector2Int.FloorToInt(transform.position);
    }

    private void Update() {
      int frame = (int)(Time.time * FrameRate % animFrames.Count);

      spriteRenderer.sprite = animFrames[frame];
    }
  }
}
