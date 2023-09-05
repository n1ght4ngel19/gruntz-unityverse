using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.MapObjectz.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse {
  public class Controller : MonoBehaviour {
    public static Controller Instance { get; private set; }
    public bool leftClick;
    public bool rightClick;
    public bool leftShiftDown;
    public bool leftControlDown;
    public List<Grunt> selectedGruntz;
    // ------------------------------------------------------------ //

    private void Awake() {
      if (Instance is not null && Instance != this) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }

      selectedGruntz = new List<Grunt>();
    }
    // ------------------------------------------------------------ //

    private void Update() {
      leftClick = Input.GetMouseButtonDown(0);
      rightClick = Input.GetMouseButtonDown(1);
      leftShiftDown = Input.GetKey(KeyCode.LeftShift);
      leftControlDown = Input.GetKey(KeyCode.LeftControl);

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
      // ------------------------------
      // Todo

      // ------------------------------
      // Single move command
      // ------------------------------
      if (rightClick && selectedGruntz.Count > 0) {
        Node clickedNode = SelectorCircle.Instance.ownNode;

        foreach (Grunt grunt in selectedGruntz.Where(grunt1 => !grunt1.navigator.isMoveForced)) {
          grunt.CleanState();

          grunt.navigator.targetNode = clickedNode;
          grunt.navigator.haveMoveCommand = true;
        }
      }

      // ------------------------------
      // Action command
      // ------------------------------
      if (leftClick && leftShiftDown) {
        Node clickedNode = SelectorCircle.Instance.ownNode;
        List<MapObject> possibleTargets = LevelManager.Instance.mapObjectContainer.GetComponentsInChildren<MapObject>().ToList();

        MapObject targetMapObject = possibleTargets
          .FirstOrDefault(obj => obj.location == clickedNode.location);

        Grunt targetGrunt = LevelManager.Instance.allGruntz
          .FirstOrDefault(grunt => grunt.navigator.ownNode == clickedNode);

        // Issuing action command according to the target being a MapObject or a Grunt
        selectedGruntz.ForEach(grunt => {
          grunt.CleanState();

          if (targetGrunt is not null && targetGrunt.IsValidTargetFor(grunt)) {
            grunt.targetGrunt = targetGrunt;
            grunt.navigator.targetNode = targetGrunt.navigator.ownNode;
            grunt.haveActionCommand = true;
          } else if (targetMapObject is GiantRockEdge && grunt.equipment.tool is Gauntletz) {
            List<Node> nodeNeighbours = targetMapObject.ownNode.Neighbours;
            List<Node> shortestPath = Pathfinder.PathBetween(grunt.navigator.ownNode, nodeNeighbours[0],
              grunt.navigator.isMoveForced, LevelManager.Instance.nodes);

            foreach (Node neighbour in nodeNeighbours) {
              List<Node> pathToNode = Pathfinder.PathBetween(grunt.navigator.ownNode, neighbour,
                grunt.navigator.isMoveForced, LevelManager.Instance.nodes);

              if (pathToNode.Count != 0 && pathToNode.Count < shortestPath.Count) {
                shortestPath = pathToNode;
              }
            }

            grunt.gruntState = GruntState.Use;
            grunt.targetMapObject = targetMapObject;
            grunt.navigator.targetNode = shortestPath.Last();
            grunt.haveActionCommand = true;
          } else if (targetMapObject is not null && targetMapObject.IsValidTargetFor(grunt)) {
            grunt.targetMapObject = targetMapObject;
            grunt.navigator.targetNode = targetMapObject.ownNode;
            grunt.haveActionCommand = true;
          }
        });
      }

      // ------------------------------
      // Give toy command
      // ------------------------------
      if (leftClick && leftControlDown) {
        Node clickedNode = SelectorCircle.Instance.ownNode;

        Grunt targetGrunt =
          LevelManager.Instance.allGruntz.FirstOrDefault(grunt => grunt.navigator.ownNode == clickedNode);

        // Issuing action command according to the target being a Grunt or not
        selectedGruntz.ForEach(grunt => {
          if (targetGrunt is not null && targetGrunt != grunt) {
            grunt.targetGrunt = targetGrunt;
            grunt.navigator.targetNode = targetGrunt.navigator.ownNode;
            grunt.haveActionCommand = true;
            grunt.haveGiveToyCommand = true;
          } else if (!clickedNode.IsUnavailable()) {
            grunt.navigator.targetNode = clickedNode;
            grunt.haveActionCommand = true;
            grunt.haveGiveToyCommand = true;
          }
        });
      }
    }
  }
}
