using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class HealthBar : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public List<Sprite> frames;

    private void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();
      frames = Resources.LoadAll<Sprite>("Animationz/AttributeBarz/healthBar").ToList();
    }
  }
}
