using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Itemz;
using UnityEngine;

namespace GruntzUnityverse {
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
    private List<Sprite> currentCursor;
    private int counter;

    private void Start() {
      Cursor.visible = false;
      currentCursor = AnimationManager.CursorAnimations.Pointer;
    }

    private void Update() {
      counter++;
      transform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * -50;
      // spriteRenderer.sprite = currentCursor[counter % currentCursor.Count];
      spriteRenderer.sprite = currentCursor[counter % (currentCursor.Count * 4)];

      HandleRockCursor();
    }

    private void SetCursorTo(List<Sprite> cursor) {
      currentCursor = cursor;
    }

    /// <summary>
    /// Handles the animation of the cursor with a GauntletzGrunt selected and hovering over a Rock or a GiantRock.
    /// </summary>
    private void HandleRockCursor() {
      if (
        LevelManager.Instance.rockz.Any(rock => rock.GridLocation.Equals(SelectorCircle.Instance.GridLocation))
        && LevelManager.Instance.gruntz.Any(grunt => grunt.tool == ToolType.Gauntletz && grunt.isSelected)
      ) {
        Instance.SetCursorTo(AnimationManager.CursorAnimations.Gauntletz);
      }
      else {
        Instance.SetCursorTo(AnimationManager.CursorAnimations.Pointer);
      }
    }
  }
}
