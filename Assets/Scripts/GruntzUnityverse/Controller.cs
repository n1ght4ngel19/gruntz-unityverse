using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse {
  public class Controller : MonoBehaviour {
    public static Controller Instance { get; private set; }
    public bool leftClick;
    public bool rightClick;
    public bool leftShiftDown;
    public List<Grunt> selectedGruntz;

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }

      selectedGruntz = new List<Grunt>();
    }

    private void Update() {
      leftClick = Input.GetMouseButtonDown(0);
      rightClick = Input.GetMouseButtonDown(1);
      leftShiftDown = Input.GetKey(KeyCode.LeftShift);

      // Single select command
      if (leftClick && !leftShiftDown) {
        foreach (Grunt grunt in LevelManager.Instance.playerGruntz) {
          grunt.isSelected = grunt.isInCircle;

          if (grunt.isInCircle) {
            selectedGruntz.Clear();
            selectedGruntz.Add(grunt);
          }
        }
      }
      // ------------------------------

      // Multi select command
      // Todo
      // ------------------------------

      // Single move command
      if (rightClick && selectedGruntz.Count > 0) {
        Node clickedNode = SelectorCircle.Instance.ownNode;

        selectedGruntz.ForEach(
          grunt => {
            grunt.navigator.targetNode = clickedNode;
            grunt.navigator.haveMoveCommand = true;
          }
        );
      }
      // ------------------------------

      // Action command
      if (leftClick && leftShiftDown) {
        Node clickedNode = SelectorCircle.Instance.ownNode;

        MapObject targetMapObject = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<MapObject>()
          .FirstOrDefault(obj => obj.ownNode == clickedNode);

        Grunt targetGrunt =
          LevelManager.Instance.allGruntz.FirstOrDefault(grunt => grunt.navigator.ownNode == clickedNode);

        // Issuing action command according to the target being a MapObject or a Grunt
        selectedGruntz.ForEach(
          selectedGrunt => {
            if (targetGrunt is not null && targetGrunt != selectedGrunt) {
              selectedGrunt.targetGrunt = targetGrunt;
              selectedGrunt.actAsPlayer = true;
              // selectedGrunt.ActAsPlayer(targetGrunt);
            } else if (targetMapObject is not null) {
              selectedGrunt.targetMapObject = targetMapObject;
              selectedGrunt.actAsPlayer = true;
              // selectedGrunt.ActAsPlayer(targetMapObject);
            }
          }
        );
      }
      // ------------------------------
    }
  }
}
