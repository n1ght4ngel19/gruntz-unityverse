using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse {
  public class AboveBelow : MonoBehaviour {
    private void Start() {
      InvokeRepeating(nameof(AdjustZValue), 0, 0.1f);
    }

    public void AdjustZValue() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
        Vector3 gruntPosition = grunt.transform.position;

        if (Vector3.Distance(gruntPosition, transform.position) > 2f) {
          continue;
        }

        // When Grunt is below self
        if (grunt.IsBehind && gruntPosition.y < transform.position.y + 0.5f) {
          // Set Grunt in the foreground
          gruntPosition = new Vector3(gruntPosition.x, gruntPosition.y, grunt.InitialZ);

          grunt.transform.position = gruntPosition;
          grunt.IsBehind = false;
        }

        // When other Grunt is above self
        if (!grunt.IsBehind && gruntPosition.y >= transform.position.y + 0.5f) {
          // Set other Grunt in the background
          grunt.transform.position += Vector3.forward * 5;
          grunt.IsBehind = true;
        }
      }
    }
  }
}
