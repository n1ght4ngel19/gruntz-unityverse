using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Saving {
  public struct GruntData {
    public int gruntId;
    public string gruntName;
    public string tool;
    public Vector3 position;
    public Owner owner;
    public string materialKey;
    public string state;
    public int health;
    public int stamina;
    public int toyTime;
    public int wingzTime;

    public bool haveMoveCommand;
    public bool haveGivingCommand;
    public bool haveMovingToUsingCommand;
    public bool haveMovingToAttackingCommand;
    public bool haveMovingToGivingCommand;
    public bool isInterrupted;
    public bool hasPlayedMovementAcknowledgeSound;


    public Vector2Int navigatorOwnLocation;
    public Vector2Int navigatorTargetNodeLocation;

    public bool navigatorIsMoving;
    public bool navigatorIsMoveForced;
    public bool navigatorMovesDiagonally;

    public Vector3 navigatorMoveVector;
    public Direction navigatorFacingDirection;

    public int targetGruntId;
    public int targetMapObjectId;

    public GruntData(Grunt grunt) {
      gruntId = grunt.gruntId;
      gruntName = grunt.gameObject.name;
      tool = grunt.equipment.tool.mapItemName;
      position = grunt.transform.position;
      owner = grunt.owner;
      materialKey = grunt.spriteRenderer.material.name.Split("(")[0].Trim();
      state = grunt.state.ToString();
      health = grunt.health;
      stamina = grunt.stamina;
      toyTime = grunt.toyTime;
      wingzTime = grunt.wingzTime;

      haveMoveCommand = grunt.haveMoveCommand;
      haveGivingCommand = grunt.haveGivingCommand;
      haveMovingToUsingCommand = grunt.haveMovingToUsingCommand;
      haveMovingToAttackingCommand = grunt.haveMovingToAttackingCommand;
      haveMovingToGivingCommand = grunt.haveMovingToGivingCommand;
      isInterrupted = grunt.isInterrupted;
      hasPlayedMovementAcknowledgeSound = grunt.hasPlayedMovementAcknowledgeSound;

      navigatorOwnLocation = grunt.navigator.ownLocation;
      navigatorTargetNodeLocation = grunt.navigator.targetNode.location;

      navigatorIsMoving = grunt.navigator.isMoving;
      navigatorIsMoveForced = grunt.navigator.isMoveForced;
      navigatorMovesDiagonally = grunt.navigator.movesDiagonally;

      navigatorMoveVector = grunt.navigator.moveVector;
      navigatorFacingDirection = grunt.navigator.facingDirection;

      targetGruntId = grunt.targetGrunt != null ? grunt.targetGrunt.gruntId : -1;
      targetMapObjectId = grunt.targetMapObject != null ? grunt.targetMapObject.objectId : -1;
    }
  }
}
