using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public class TwoWayArrow : Arrow {
    public Sprite state1Sprite;
    public Sprite state2Sprite;

    public void Toggle() {
      spriteRenderer.sprite = spriteRenderer.sprite == state1Sprite
        ? state2Sprite
        : state1Sprite;

      direction = direction.Opposite();
      SetTargetNode();
    }
  }
}
