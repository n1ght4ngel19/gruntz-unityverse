using System.Collections.Generic;
using System.Linq;

using Itemz;

using UnityEngine;

namespace Singletonz {
  public class GameCursor : MonoBehaviour {
    private static GameCursor _instance;

    public static GameCursor Instance {
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
    public List<Sprite> gauntletz;
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

      HandleRockCursor();
    }

    private void SetCursorTo(List<Sprite> cursor) {
      currentCursor = cursor;
    }

    private void HandleRockCursor() {
      if (
        MapManager.Instance.rockz.Any(rock => rock.twoDimPosition == SelectorCircle.Instance.twoDimPosition)
        && MapManager.Instance.gruntz.Any(grunt => grunt.tool == ToolType.Gauntletz && grunt.isSelected)
      ) {
        Instance.SetCursorTo(Instance.gauntletz);
      } else {
        Instance.SetCursorTo(Instance.pointer);
      }
    }
  }
}
