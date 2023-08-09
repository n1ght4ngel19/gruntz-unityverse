using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse {
  public class AboveBelow : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    private int _initialLayerOrderValue;

    private void Start() {
      _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      _initialLayerOrderValue = _spriteRenderer.sortingOrder;

      InvokeRepeating(nameof(AdjustZValue), 0, 0.1f);
    }

    public void AdjustZValue() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
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
