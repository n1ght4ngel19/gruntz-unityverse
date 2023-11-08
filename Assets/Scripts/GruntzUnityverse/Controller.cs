using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse {
  /// <summary>
  /// Player input manager for orchestrating input/command communication.
  /// </summary>
  public class Controller : MonoBehaviour {
    /// <summary>
    /// Shorthand for "Input.GetMouseButtonDown(0)".
    /// </summary>
    public bool leftClick;

    /// <summary>
    /// Shorthand for "Input.GetMouseButtonDown(1)".
    /// </summary>
    public bool rightClick;

    /// <summary>
    /// Shorthand for "Input.GetKey(KeyCode.LeftShift)".
    /// </summary>
    public bool leftShiftDown;

    /// <summary>
    /// Shorthand for "Input.GetKey(KeyCode.LeftControl)".
    /// </summary>
    public bool leftControlDown;

    /// <summary>
    /// The Gruntz currently selected by the team.
    /// </summary>
    public static List<Grunt> selectedGruntz;

    private void OnEnable() {
      selectedGruntz = new List<Grunt>();
    }

    private void Update() {
      leftClick = Input.GetMouseButtonDown(0);
      rightClick = Input.GetMouseButtonDown(1);
      leftShiftDown = Input.GetKey(KeyCode.LeftShift);
      leftControlDown = Input.GetKey(KeyCode.LeftControl);

      // Single select with click
      if (leftClick && !leftShiftDown && !leftControlDown) {
        DeselectAllGruntz();

        SelectGrunt(
          GameManager.Instance.currentLevelManager.allGruntz.FirstOrDefault(
            grunt => grunt.navigator.ownNode == GameManager.Instance.selectorCircle.ownNode
          )
        );
      }

      // Single select with numeric keys
      SelectById();

      // ------------------------------
      // Multi select with rect
      // ------------------------------
      // Todo

      // ------------------------------
      // Move command
      // ------------------------------
      if (rightClick && selectedGruntz.Count > 0) {
        Node clickedNode = GameManager.Instance.selectorCircle.ownNode;

        foreach (Grunt grunt in selectedGruntz) {
          if (grunt.navigator.isMoveForced) {
            continue;
          }

          grunt.CleanState();
          grunt.navigator.targetNode = clickedNode;
          grunt.haveMoveCommand = true;
          grunt.hasPlayedMovementAcknowledgeSound = false;
        }
      }

      // ------------------------------
      // Action command
      // ------------------------------
      if (leftClick && leftShiftDown) {
        Node clickedNode = GameManager.Instance.selectorCircle.ownNode;

        List<MapObject> possibleTargets = GameManager.Instance.currentLevelManager
          .mapObjectContainer.GetComponentsInChildren<MapObject>()
          .ToList();

        MapObject targetMapObject = possibleTargets.FirstOrDefault(
          obj => obj.location == clickedNode.location && obj.isTargetable
        );

        Grunt targetGrunt =
          GameManager.Instance.currentLevelManager.allGruntz.FirstOrDefault(
            grunt => grunt.navigator.ownNode == clickedNode
          );

        // Issuing action command according to the target being a MapObject or a Grunt
        selectedGruntz.ForEach(
          grunt => {
            grunt.CleanState();

            // Attack command (when target is a Grunt)
            if (targetGrunt is not null && targetGrunt.IsValidTargetFor(grunt)) {
              grunt.haveMovingToAttackingCommand = true;
              grunt.targetGrunt = targetGrunt;
              grunt.navigator.targetNode = targetGrunt.navigator.ownNode;
              // grunt.hasPlayedAttackAcknowledgeSound = false;

              return;
            }

            // GiantRockEdge break command (when target is a MapObject)
            if (targetMapObject is GiantRockEdge && grunt.equipment.tool is Gauntletz) {
              grunt.haveMovingToUsingCommand = true;
              grunt.targetMapObject = targetMapObject;
              grunt.navigator.targetNode = targetMapObject.ownNode;
              grunt.hasPlayedMovementAcknowledgeSound = false;

              return;
            }

            // Use command (when target is a MapObject)
            if (targetMapObject is not null && targetMapObject.IsValidTargetFor(grunt)) {
              grunt.haveMovingToUsingCommand = true;
              grunt.targetMapObject = targetMapObject;
              grunt.navigator.targetNode = targetMapObject.ownNode;
              grunt.hasPlayedMovementAcknowledgeSound = false;
            }
          }
        );
      }

      // ------------------------------
      // Giving command
      // ------------------------------
      if (leftClick && leftControlDown) {
        Node clickedNode = GameManager.Instance.selectorCircle.ownNode;

        Grunt targetGrunt =
          GameManager.Instance.currentLevelManager.allGruntz.FirstOrDefault(
            grunt => grunt.navigator.ownNode == clickedNode
          );

        // Issuing action command according to the target being a Grunt or a spot on the ground
        selectedGruntz.ForEach(
          grunt => {
            if (grunt.equipment.toy == null) {
              return;
            }

            if (targetGrunt is not null && targetGrunt != grunt) {
              grunt.targetGrunt = targetGrunt;
              grunt.navigator.targetNode = targetGrunt.navigator.ownNode;
              grunt.haveMovingToGivingCommand = true;
              // grunt.hasPlayedGiveToyAcknowledgeSound = false;
            } else if (!clickedNode.IsUnavailable()) {
              grunt.navigator.targetNode = clickedNode;
              grunt.haveMovingToGivingCommand = true;
              // grunt.hasPlayedGiveToyAcknowledgeSound = false;
            }
          }
        );
      }
    }

    public static void SelectGrunt(Grunt grunt) {
      if (grunt == null) {
        return;
      }

      switch (grunt.team) {
        case Team.Player1:
          grunt.isSelected = true;
          grunt.selectedCircle.spriteRenderer.enabled = true;

          bool doPlayVoice = Random.Range(0, 11) <= GameManager.Instance.selectVoicePlayFrequency;

          if (doPlayVoice) {
            // Todo: Different clip based on clicking
            int selectVoiceIndex = Random.Range(1, 12);

            Addressables.LoadAssetAsync<AudioClip>($"Voice_SelectGrunt_1_{selectVoiceIndex}.wav")
              .Completed += handle => {
              if (!grunt.audioSource.isPlaying) {
                grunt.audioSource.PlayOneShot(handle.Result);
              }
            };
          }

          selectedGruntz.Add(grunt);

          break;
        case Team.Ai1:
          int enemySelectVoiceIndex = Random.Range(1, 12);

          Addressables.LoadAssetAsync<AudioClip>($"Voice_EnemySelect_{enemySelectVoiceIndex}.wav")
            .Completed += handle => {
            if (!grunt.audioSource.isPlaying) {
              grunt.audioSource.PlayOneShot(handle.Result);
            }
          };

          break;
      }
    }

    public static void DeselectAllGruntz() {
      selectedGruntz.Clear();

      GameManager.Instance.currentLevelManager.player1Gruntz.ForEach(
        grunt => {
          grunt.isSelected = false;
          grunt.selectedCircle.spriteRenderer.enabled = false;
        }
      );
    }

    private void SelectById() {
      int alphaKeyPressed = -1;

      if (Input.GetKeyDown(KeyCode.Alpha1)) {
        alphaKeyPressed = 1;
      }

      if (Input.GetKeyDown(KeyCode.Alpha2)) {
        alphaKeyPressed = 2;
      }

      if (Input.GetKeyDown(KeyCode.Alpha3)) {
        alphaKeyPressed = 3;
      }

      if (Input.GetKeyDown(KeyCode.Alpha4)) {
        alphaKeyPressed = 4;
      }

      if (Input.GetKeyDown(KeyCode.Alpha5)) {
        alphaKeyPressed = 5;
      }

      if (Input.GetKeyDown(KeyCode.Alpha6)) {
        alphaKeyPressed = 6;
      }

      if (Input.GetKeyDown(KeyCode.Alpha7)) {
        alphaKeyPressed = 7;
      }

      if (Input.GetKeyDown(KeyCode.Alpha8)) {
        alphaKeyPressed = 8;
      }

      if (Input.GetKeyDown(KeyCode.Alpha9)) {
        alphaKeyPressed = 9;
      }

      if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A)) {
        alphaKeyPressed = 0;
      }

      if (alphaKeyPressed == 0) {
        GameManager.Instance.currentLevelManager.player1Gruntz.ForEach(SelectGrunt);
      } else {
        Grunt gruntToSelect =
          GameManager.Instance.currentLevelManager.player1Gruntz.FirstOrDefault(
            grunt => grunt.playerGruntId == alphaKeyPressed
          );

        if (gruntToSelect is null) {
          return;
        }

        DeselectAllGruntz();
        SelectGrunt(gruntToSelect);
      }
    }
  }
}
