using System.Collections.Generic;

using UnityEngine;

namespace Singletonz {
  public class Cursor : MonoBehaviour {
    private static Cursor _instance;

    public static Cursor Instance {
      get => _instance;
    }

    private void Awake() {
      if (_instance != null && _instance != this)
        Destroy(gameObject);
      else
        _instance = this;
    }

    public Camera mainCamera;
    public SpriteRenderer spriteRenderer;
    public List<Sprite> pointer;
    private List<Sprite> currentCursor;
    private int counter = 0;
    
    private void Start() {
      UnityEngine.Cursor.visible = false;
      currentCursor = pointer;
    }

    private void Update() {
      counter++;
      transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * -50;
      spriteRenderer.sprite = currentCursor[counter % currentCursor.Count];
    }

    // private IEnumerator cycleThroughFrames() {
    //   foreach (Sprite frame in currentCursor) {
    //
    //     yield return null;
    //   }
    // }

    public void SetCursorTo(List<Sprite> cursor) {
      currentCursor = cursor;
    }
  }
}
