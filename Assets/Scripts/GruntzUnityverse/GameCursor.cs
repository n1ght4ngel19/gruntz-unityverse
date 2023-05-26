using System.Collections.Generic;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse {
  public class GameCursor : MonoBehaviour {
    private static GameCursor _instance;

    public static GameCursor Instance {
      get => _instance;
    }

    public Camera mainCamera;
    private SpriteRenderer Renderer { get; set; }
    private List<Sprite> CurrentFrames { get; set; }
    private int Counter { get; set; }


    private void Awake() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      Renderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
      Cursor.visible = false;
      CurrentFrames = AnimationManager.CursorAnimations.Pointer;
    }

    private void Update() {
      Counter++;

      transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * -50;

      // Todo
      // Renderer.sprite = CurrentFrames[Counter % CurrentFrames.Count];
    }

    private void SetCursorTo(List<Sprite> frames) {
      CurrentFrames = frames;
    }
  }
}
