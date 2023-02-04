using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.Actorz
{
  public class StaminaBar : MonoBehaviour
  {
    public SpriteRenderer Renderer { get; set; }
    public Sprite DisplayFrame { get; set; }
    [field: SerializeField] public List<Sprite> AnimationFrames { get; set; }

    private void Start()
    {
      Renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
      // Renderer.enabled = false;
    }

    private void Update()
    {
      // Renderer.enabled = value < MaxValue;
    }
  }
}
