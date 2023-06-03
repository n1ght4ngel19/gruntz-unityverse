using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class HealthBar : MonoBehaviour {
    public SpriteRenderer Renderer { get; set; }
    public List<Sprite> Frames { get; set; }

    private void Awake() {
      Renderer = GetComponent<SpriteRenderer>();
      Frames = Resources.LoadAll<Sprite>("Animationz/AttributeBarz/HealthBar").ToList();
    }
  }
}
