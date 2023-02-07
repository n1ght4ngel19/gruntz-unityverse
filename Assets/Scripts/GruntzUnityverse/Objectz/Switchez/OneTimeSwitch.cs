using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class OneTimeSwitch : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    private SpriteRenderer Renderer { get; set; }
    [field: SerializeField] public Sprite PressedSprite { get; set; }
    [field: SerializeField] public Sprite ReleasedSprite { get; set; }
    [field: SerializeField] public List<BlackPyramid> Pyramidz { get; set; }
    [field: SerializeField] public bool IsPressed { get; set; }


    private void Start() {
      if (Pyramidz.Count.Equals(0)) {
        Debug.LogWarning("There is no Pyramid assigned to this Switch!");
      }

      OwnLocation = Vector2Int.FloorToInt(transform.position);
      Renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update() {
      if (LevelManager.Instance.PlayerGruntz.Any(grunt => grunt.NavComponent.OwnLocation.Equals(OwnLocation))) {
        TogglePyramidz();

        IsPressed = true;
        Renderer.sprite = PressedSprite;
        enabled = false;
      }
    }

    private void TogglePyramidz() {
      foreach (BlackPyramid pyramid in Pyramidz) {
        pyramid.ChangeState();
        pyramid.enabled = false;
      }
    }
  }
}
