using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.MapObjectz.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  public class Controller : MonoBehaviour {
    public bool leftClick;
    public bool rightClick;
    public bool leftShiftDown;
    public bool leftControlDown;
    public List<Grunt> selectedGruntz;
    // ------------------------------------------------------------ //

    private void OnEnable() {
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
        foreach (Grunt grunt in GameManager.Instance.currentLevelManager.playerGruntz) {
          grunt.isSelected = grunt.isInCircle;

          if (grunt.isSelected) {
            bool doPlayVoice = Random.Range(0, 11) <= GameManager.Instance.selectVoicePlayFrequency;

            if (doPlayVoice) {
              // Todo: Different clip based on clicking
              int idx = Random.Range(1, 12);

              Addressables.LoadAssetAsync<AudioClip>($"Voice_SelectGrunt_1_{idx}.wav").Completed += handle => {
                grunt.audioSource.PlayOneShot(handle.Result);
              };
            }

            selectedGruntz.Clear();
            selectedGruntz.Add(grunt);
          }
        }

        foreach (Grunt grunt in GameManager.Instance.currentLevelManager.enemyGruntz) {
          if (grunt.isInCircle) {
            int idx = Random.Range(1, 12);

            Addressables.LoadAssetAsync<AudioClip>($"Voice_EnemySelect_{idx}.wav").Completed += handle => {
              grunt.audioSource.PlayOneShot(handle.Result);
            };
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
        Node clickedNode = GameManager.Instance.selectorCircle.ownNode;

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
        Node clickedNode = GameManager.Instance.selectorCircle.ownNode;
        List<MapObject> possibleTargets =
          GameManager.Instance.currentLevelManager.mapObjectContainer.GetComponentsInChildren<MapObject>().ToList();

        MapObject targetMapObject = possibleTargets
          .FirstOrDefault(obj => obj.location == clickedNode.location && obj.isTargetable);

        Grunt targetGrunt = GameManager.Instance.currentLevelManager.allGruntz
          .FirstOrDefault(grunt => grunt.navigator.ownNode == clickedNode);

        // Issuing action command according to the target being a MapObject or a Grunt
        selectedGruntz.ForEach(grunt => {
          grunt.CleanState();

          if (targetGrunt is not null && targetGrunt.IsValidTargetFor(grunt)) {
            grunt.targetGrunt = targetGrunt;
            grunt.navigator.targetNode = targetGrunt.navigator.ownNode;
            grunt.haveActionCommand = true;
            // Special Giant Rock treatment
          } else if (targetMapObject is GiantRockEdge && grunt.equipment.tool is Gauntletz) {
            grunt.navigator.SetTargetBesideNode(targetMapObject.ownNode);

            // List<Node> nodeNeighbours = targetMapObject.ownNode.Neighbours;
            // List<Node> shortestPath = Pathfinder.PathBetween(grunt.navigator.ownNode, nodeNeighbours[0],
            //   grunt.navigator.isMoveForced, GameManager.Instance.currentLevelManager.nodes);
            //
            // foreach (Node neighbour in nodeNeighbours) {
            //   List<Node> pathToNode = Pathfinder.PathBetween(grunt.navigator.ownNode, neighbour,
            //     grunt.navigator.isMoveForced, GameManager.Instance.currentLevelManager.nodes);
            //
            //   if (pathToNode.Count != 0 && pathToNode.Count < shortestPath.Count) {
            //     shortestPath = pathToNode;
            //   }
            // }

            grunt.gruntState = GruntState.Use;
            grunt.targetMapObject = targetMapObject;
            // grunt.navigator.targetNode = shortestPath.Last();
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
        Node clickedNode = GameManager.Instance.selectorCircle.ownNode;

        Grunt targetGrunt =
          GameManager.Instance.currentLevelManager.allGruntz.FirstOrDefault(grunt => grunt.navigator.ownNode == clickedNode);

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
