using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse {
  /// <summary>
  /// Helper script for EyeCandy objects for moving themselves before/behind moving Gruntz.
  /// </summary>
  public class AboveBelow : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    private int _initialLayerOrderValue;

    private void Start() {
      _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      _initialLayerOrderValue = _spriteRenderer.sortingOrder;

      InvokeRepeating(nameof(AdjustZValue), 0, 0.1f);
    }

    /// <summary>
    /// Adjusts the GameObject's layer value when a Grunt is moving nearby.
    /// </summary>
    public void AdjustZValue() {
      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz) {
        Vector3 gruntPosition = grunt.transform.position;

        if (Vector2.Distance(grunt.transform.position, transform.position) > 2f) {
          continue;
        }

        // When Grunt is below self
        if (grunt.transform.position.y < transform.position.y + 0.1f) {
          // Set self in the background
          _spriteRenderer.sortingOrder = _initialLayerOrderValue;
        }

        // When Grunt is above self
        if (grunt.transform.position.y >= transform.position.y + 0.1f) {
          // Set self in the foreground
          _spriteRenderer.sortingOrder = grunt.spriteRenderer.sortingOrder + 10;
        }
      }
    }
  }
}
