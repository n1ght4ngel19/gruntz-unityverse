using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class HealthBar : MonoBehaviour {
    public const int MaxValue = 20;
    public int value = MaxValue;
    
    public SpriteRenderer spriteRenderer;
    public List<Sprite> animFrames;

    private void Update() {
      spriteRenderer.sprite = animFrames[value];
    }
  }
}
