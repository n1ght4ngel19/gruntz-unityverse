using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using GruntzUnityverse.Itemz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.PathFinding;
using GruntzUnityverse.Singletonz;
using GruntzUnityverse.Utilitiez;

using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Grunt : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public ToolType tool;
    public ToyType toy;
 
    private GruntAnimationPack animations;

    // protected const float TimePerTile = (float)TravelSpeed.Grunt / 1000;
    private const float TimePerTile = 0.2f;
    private Vector3 targetPosition;
    // TODO: Replace
    // private Vector3 diffVector;
    private bool isMoving;
    private bool hasMoved;

    public bool isSelected;
    public CompassDirection facingDirection = CompassDirection.South;

    private const float WalkFrameRate = 12;
    private const float IdleFrameRate = 8;
    private float idleTime;

    public List<NavTile> path;
    public NavTile startNode;
    public NavTile endNode;

    private void Start() {
      SwitchGruntAnimations(tool);

      targetPosition = transform.position;
    }

    private void Update() {
      PlaySouthIdleAnimationByDefault();

      if (Time.timeScale == 0) {
        return;
      }
      
      Move();
    }

    public void SwitchGruntAnimations(ToolType toolType) {
      animations = toolType switch {
        ToolType.BareHandz => AnimationManager.BareHandzGruntAnimations,
        ToolType.Club => AnimationManager.ClubGruntAnimations,
        ToolType.Gauntletz => AnimationManager.GauntletzGruntAnimations,
        _ => throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null)
      };
    }

    private void Move() {
      SetTargetPosition();

      SetPath();

      Vector3 destination = new(
        Mathf.Floor(path[0].gridLocation.x) + 0.5f,
        Mathf.Floor(path[0].gridLocation.y) + 0.5f,
        -5
      );

      // diffVector = destination - transform.position;
      StartCoroutine(Move(destination));
    
      path.RemoveAt(0);
    }

    private IEnumerator Move(Vector3 destination) {
      yield return new WaitForSeconds(TimePerTile);

      transform.position = destination;
    }

    private void SetTargetPosition() {
      foreach (
        Arrow arrow in MapManager.Instance.arrowz
          .Where(arrow => arrow != null
                          && (Vector2)arrow.transform.position == (Vector2)transform.position
                          && arrow.spriteRenderer.enabled)
      ) {
        targetPosition = arrow.direction switch {
          CompassDirection.East => transform.position + Vector3.right,
          CompassDirection.North => transform.position + Vector3.up,
          CompassDirection.NorthEast => transform.position + Vector3Plus.upright,
          CompassDirection.NorthWest => transform.position + Vector3Plus.upleft,
          CompassDirection.South => transform.position + Vector3.down,
          CompassDirection.SouthEast => transform.position + Vector3Plus.downright,
          CompassDirection.SouthWest => transform.position + Vector3Plus.downleft,
          CompassDirection.West => transform.position + Vector3.left,
          _ => transform.position
        };

        return;
      }

      if (Input.GetMouseButtonDown(1) && isSelected) {
        isMoving = true;
        targetPosition = SelectorCircle.Instance.transform.position;
      
        targetPosition.z = -5;

        if (!hasMoved)
          hasMoved = true;
      }
    }

    private void SetPath() {
      Vector2Int startKey = new(
        (int)Mathf.Floor(transform.position.x),
        (int)Mathf.Floor(transform.position.y)
      );

      startNode = MapManager.Instance.map.ContainsKey(startKey)
        ? MapManager.Instance.map[startKey]
        : MapManager.Instance.map[startKey + Vector2Int.up];

      Vector2Int endKey = new(
        (int)Mathf.Floor(targetPosition.x),
        (int)Mathf.Floor(targetPosition.y)
      );

      endNode = MapManager.Instance.map.ContainsKey(endKey)
        ? MapManager.Instance.map[endKey]
        : MapManager.Instance.map[endKey + Vector2Int.up];

      path = PathFinder.FindPath(startNode, endNode);
    }
  
    protected void OnMouseDown() {
      isSelected = true;

      foreach (Grunt grunt in MapManager.Instance.gruntz.Where(grunt => grunt != this)) {
        grunt.isSelected = false;
      }
    }

    private void PlaySouthIdleAnimationByDefault() {
      if (hasMoved) {
        return;
      }
  
      float playTime = Time.time - idleTime;
      int frame = (int)(playTime * IdleFrameRate % animations.IdleSouth.Count);
  
      spriteRenderer.sprite = animations.IdleSouth[frame];
    }
  
    // private List<Sprite> GetWalkSprites() {
    //   List<Sprite> selectedSprites = null;
    //
    //   switch (diffVector.y) {
    //     case < 0: {
    //       selectedSprites = walkSpritesSouth;
    //       facingDirection = "SOUTH";
    //
    //       switch (diffVector.x) {
    //         case > 0: {
    //           selectedSprites = walkSpritesSouthEast;
    //           facingDirection = "SOUTHEAST";
    //           break;
    //         }
    //         case < 0: {
    //           selectedSprites = walkSpritesSouthWest;
    //           facingDirection = "SOUTHWEST";
    //           break;
    //         }
    //       }
    //       break;
    //     }
    //     case > 0: {
    //       selectedSprites = walkSpritesNorth;
    //       facingDirection = "NORTH";
    //
    //       switch (diffVector.x) {
    //         case > 0: {
    //           selectedSprites = walkSpritesNorthEast;
    //           facingDirection = "NORTHEAST";
    //           break;
    //         }
    //         case < 0: {
    //           selectedSprites = walkSpritesNorthWest;
    //           facingDirection = "NORTHWEST";
    //           break;
    //         }
    //       }
    //       break;
    //     }
    //     default: {
    //       switch (diffVector.x) {
    //         case > 0: {
    //           selectedSprites = walkSpritesEast;
    //           facingDirection = "EAST";
    //           break;
    //         }
    //         case < 0: {
    //           selectedSprites = walkSpritesWest;
    //           facingDirection = "WEST";
    //           break;
    //         }
    //       }
    //       break;
    //     }
    //   }
    //
    //   return selectedSprites;
    // }
    //
    // private List<Sprite> GetIdleSprites() {
    //   List<Sprite> selectedSprites = facingDirection switch {
    //     "NORTH" => idleSpritesNorth,
    //     "NORTHEAST" => idleSpritesNorthEast,
    //     "NORTHWEST" => idleSpritesNorthWest,
    //     "SOUTH" => idleSpritesSouth,
    //     "SOUTHEAST" => idleSpritesSouthEast,
    //     "SOUTHWEST" => idleSpritesSouthWest,
    //     "EAST" => idleSpritesEast,
    //     "WEST" => idleSpritesWest,
    //     _ => null
    //   };
    //
    //   return selectedSprites;
    // }
    //
    // private void PlayWalkAndIdleAnimations() {
    // if (isMoving) {
    //   List<Sprite> walkSprites = GetWalkSprites();
    //
    //   if (walkSprites != null) {
    //     float playTime = Time.time - idleTime;
    //     int frame = (int)(playTime * WalkFrameRate % walkSprites.Count);
    //
    //     spriteRenderer.sprite = walkSprites[frame];
    //   }
    //   else {
    //     isMoving = false;
    //     idleTime = Time.time;
    //   }
    // } else {
    //   List<Sprite> idleSprites = GetIdleSprites();
    //
    //   if (idleSprites != null) {
    //     float playTime = Time.time - idleTime;
    //     int frame = (int)(playTime * IdleFrameRate % idleSprites.Count);
    //
    //     spriteRenderer.sprite = idleSprites[frame];
    //   } else {
    //     idleTime = Time.time;
    //   }
    // }
    // }
  }
}
