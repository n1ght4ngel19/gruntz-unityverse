using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.DataPersistence;
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
      selectedGruntz.Clear();

      if (gruntz.Value.Any(grunt => grunt.location2D == selector.location2D)) {
        Debug.Log("Selecting");
      }

      gruntz.Value.ForEach(
        grunt => {
          grunt.SetSelected(grunt.location2D == selector.location2D);

          if (grunt.Selected) {
            selectedGruntz.Add(grunt);
          }
        }
      );
    }

    private void OnMove() {
      Vector2Int target = Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));

      gruntz.Value
        .Where(grunt => grunt.Selected)
        .ToList()
        .ForEach(grunt => grunt.Move(target));
    }

    private void OnAction() {
      // gruntz.Value
      //   .Where(grunt => grunt.flagz.selected)
      //   .ToList()
      //   .ForEach(grunt => grunt.Act());
    }

    private void OnGive() {
      // gruntz.Value
      //   .Where(grunt => grunt.flagz.selected)
      //   .ToList()
      //   .ForEach(grunt => grunt.Give());
    }

    private void OnSaveGame() {
      DataPersistenceManager.Instance.SaveGame();
    }

    private void OnLoadGame() {
      DataPersistenceManager.Instance.LoadGame();
    }

    private bool CompareVector2Int(Vector3 a, Vector3 b) {
      return Vector2Int.RoundToInt(a) == Vector2Int.FloorToInt(b);
    }
  }

}
