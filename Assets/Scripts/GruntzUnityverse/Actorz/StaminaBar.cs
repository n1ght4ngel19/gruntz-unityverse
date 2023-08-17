using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class StaminaBar : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public List<Sprite> frames;

    private void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();
      frames = Resources.LoadAll<Sprite>("Animationz/AttributeBarz/staminaBar").ToList();
    }
  }
}
