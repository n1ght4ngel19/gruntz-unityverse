using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse {
  public class GameCursor : MonoBehaviour {
    private static GameCursor _instance;

    public static GameCursor Instance {
      get => _instance;
    }

    public Camera mainCamera;
    private SpriteRenderer _spriteRenderer;
    private List<Sprite> _currentFramez;
    private int Counter { get; set; }


    private void Start() {
      if (_instance is not null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }

      _spriteRenderer = GetComponent<SpriteRenderer>();
      Cursor.visible = false;

    }
    // ------------------------------------------------------------ //

    private void Update() {
      Counter = Counter % 10 + 1;

      transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 15;
    }
    // ------------------------------------------------------------ //

    private void SetCursorTo(List<Sprite> frames) {
      _currentFramez = frames;
    }
  }
}
