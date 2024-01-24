using System.Collections.Generic;
using System.Linq;
using InfinityCode.Observers;
using UnityEngine;

namespace GruntzUnityverse.V2 {
  public class InputManager : MonoBehaviour {
    public LinkedValue<List<GruntV2>> gruntz;
    public List<GruntV2> selectedGruntz;
    public LinkedValue<HashSet<NodeV2>> levelNodes;

    [Header("Selector")]
    public Selector selector;

    private void Awake() {
      selector = FindFirstObjectByType<Selector>();
    }

    private void OnSelect() {
      Debug.Log("OnSelect");
      selectedGruntz.Clear();

      if (gruntz.Value.Any(grunt => grunt.Location2D == selector.location2D)) {
        Debug.Log("Selecting");
      }

      gruntz.Value.ForEach(
        grunt => {
          grunt.Select(grunt.Location2D == selector.location2D);

          if (grunt.flagz.selected) {
            selectedGruntz.Add(grunt);
          }
        }
      );
    }

    private void OnMove() {
      Debug.Log("OnMove");
      Vector2Int target = Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));

      gruntz.Value
        .Where(grunt => grunt.flagz.selected)
        .ToList()
        .ForEach(grunt => grunt.Move(target));
    }

    private void OnAction() {
      Debug.Log("OnAction");

      // gruntz.Value
      //   .Where(grunt => grunt.flagz.selected)
      //   .ToList()
      //   .ForEach(grunt => grunt.Act());
    }

    private void OnGive() {
      Debug.Log("OnGive");

      // gruntz.Value
      //   .Where(grunt => grunt.flagz.selected)
      //   .ToList()
      //   .ForEach(grunt => grunt.Give());
    }

    private bool CompareVector2Int(Vector3 a, Vector3 b) {
      return Vector2Int.RoundToInt(a) == Vector2Int.FloorToInt(b);
    }
  }

}
