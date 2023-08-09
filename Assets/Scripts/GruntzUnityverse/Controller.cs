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
    public bool leftControlDown;
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

        MapObject targetMapObject = LevelManager.Instance.mapObjectContainer
          .GetComponentsInChildren<MapObject>()
          .FirstOrDefault(obj => obj.ownNode == clickedNode);

        Grunt targetGrunt =
          LevelManager.Instance.allGruntz.FirstOrDefault(
            grunt => grunt.navigator.ownNode == clickedNode
          );

        // Issuing action command according to the target being a MapObject or a Grunt
        selectedGruntz.ForEach(
          grunt => {
            grunt.CleanState();

            if (targetGrunt != null && targetGrunt.IsValidTargetFor(grunt)) {
              grunt.targetGrunt = targetGrunt;
              grunt.navigator.targetNode = targetGrunt.navigator.ownNode;
              grunt.haveActionCommand = true;
            } else if (targetMapObject != null && targetMapObject.IsValidTargetFor(grunt)) {
              grunt.targetMapObject = targetMapObject;
              grunt.navigator.targetNode = targetMapObject.ownNode;
              grunt.haveActionCommand = true;
            }
          }
        );
      }
      // ------------------------------

      if (leftClick && leftControlDown) {
        Node clickedNode = SelectorCircle.Instance.ownNode;

        Grunt targetGrunt =
          LevelManager.Instance.allGruntz.FirstOrDefault(
            grunt => grunt.navigator.ownNode == clickedNode
          );

        // Issuing action command according to the target being a Grunt or not
        selectedGruntz.ForEach(
          grunt => {
            if (targetGrunt != null && targetGrunt != grunt) {
              grunt.targetGrunt = targetGrunt;
              grunt.navigator.targetNode = targetGrunt.navigator.ownNode;
              grunt.haveActionCommand = true;
              grunt.haveGiveToyCommand = true;
            } else if (!clickedNode.IsUnavailable()) {
              grunt.navigator.targetNode = clickedNode;
              grunt.haveActionCommand = true;
              grunt.haveGiveToyCommand = true;
            }
          }
        );
      }
    }
  }
}
