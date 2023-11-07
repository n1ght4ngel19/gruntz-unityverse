using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse {
  /// <summary>
  /// The in-game rectangle with which multiple Gruntz can be selected at once.
  /// </summary>
  public class SelectRect : MonoBehaviour {
    public RectTransform rectTransform;

    /// <summary>
    /// The point in screen-space to draw the rectangle from.
    /// </summary>
    public Vector2 startPosition = Vector2.zero;

    /// <summary>
    /// The point in screen-space to draw the rectangle to.
    /// </summary>
    public Vector2 endPosition = Vector2.zero;

    private bool _doDraw;

    private void OnEnable() {
      rectTransform = GetComponent<RectTransform>();
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.Mouse0)) {
        startPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      }

      if (Input.GetKey(KeyCode.Mouse0)) {
        _doDraw = true;
        endPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
      }

      if (Input.GetKeyUp(KeyCode.Mouse0) && startPosition != endPosition) {
        _doDraw = false;

        float minX = Mathf.Min(startPosition.x, endPosition.x);
        float maxX = Mathf.Max(startPosition.x, endPosition.x);
        float minY = Mathf.Min(startPosition.y, endPosition.y);
        float maxY = Mathf.Max(startPosition.y, endPosition.y);

        foreach (Grunt grunt in GameManager.Instance.currentLevelManager.playerGruntz) {
          Vector3 gruntPoint = Camera.main.WorldToViewportPoint(grunt.transform.position);

          if (gruntPoint.x < minX
            || gruntPoint.x > maxX
            || gruntPoint.y < minY
            || gruntPoint.y > maxY) {
            continue;
          }

          Controller.SelectGrunt(grunt);
        }
      }
    }

    private void OnGUI() {
      if (_doDraw) {
        Rect rect = new Rect(
          startPosition.x,
          startPosition.y,
          endPosition.x - startPosition.x + 500,
          -1 * (endPosition.y - startPosition.y)
        );

        GUI.DrawTexture(rect, Texture2D.redTexture);
      }
    }
  }
}
