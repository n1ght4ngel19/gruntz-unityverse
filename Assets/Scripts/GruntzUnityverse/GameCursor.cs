using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse {
  public class GameCursor : MonoBehaviour {
    private static GameCursor _instance;

    public static GameCursor Instance {
      get => _instance;
    }

    private void Awake() {
      if (_instance != null && _instance != this) {
        Destroy(gameObject);
      } else {
        _instance = this;
      }
    }

    public Camera mainCamera;
    public SpriteRenderer spriteRenderer;
    private List<Sprite> currentCursor;
    private int counter;

    private void Start() {
      Cursor.visible = false;
      currentCursor = AnimationManager.CursorAnimations.Pointer;
    }

    private void Update() {
      counter++;
      transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * -50;
      spriteRenderer.sprite = currentCursor[counter % currentCursor.Count];
    }

    private void SetCursorTo(List<Sprite> cursor) {
      currentCursor = cursor;
    }
  }
}
