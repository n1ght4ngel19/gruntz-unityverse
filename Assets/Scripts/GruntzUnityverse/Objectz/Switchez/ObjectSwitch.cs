using System;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class ObjectSwitch : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    protected SpriteRenderer Renderer { get; set; }
    [field: SerializeField] public Sprite PressedSprite { get; set; }
    [field: SerializeField] public Sprite ReleasedSprite { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }

    private void Start() {
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
    }
  }
}
