using System.Collections.Generic;

using UnityEngine;

namespace GruntzUnityverse {
  public class Fort : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public List<Sprite> animFrames;
    private const int FrameRate = 12;

    private void Update() {
      int frame = (int)(Time.time * FrameRate % animFrames.Count);

      spriteRenderer.sprite = animFrames[frame];
    }
  }
}
