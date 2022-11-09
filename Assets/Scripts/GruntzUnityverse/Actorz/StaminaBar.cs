using System;
using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class StaminaBar : MonoBehaviour {
    public const int MaxValue = 20;
    public int value = MaxValue;
    
    public SpriteRenderer spriteRenderer;
    public List<Sprite> animFrames;

    private void Start() {
      // spriteRenderer.enabled = false;
    }

    private void Update() {
      spriteRenderer.enabled = value < MaxValue;
      spriteRenderer.sprite = animFrames[value];
    }
  }
}
