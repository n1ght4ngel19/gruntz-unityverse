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
    /// <summary>
    /// The Tool the Grunt carries.
    /// </summary>
    public ToolType tool;
    /// <summary>
    /// The Toy the Grunt carries.
    /// </summary>
    public ToyType toy;
    /// <summary>
    /// The set of animations the Grunt currently uses when moving, attacking,
    /// being struck, idling, and interacting (determined by the Tool he carries).
    /// </summary>
    private GruntAnimationPack animations;

    /// <summary>
    ///   <para>The amount of time in seconds that a Grunt needs to travel from one tile to another.</para>
    ///   <para>0.2 seconds is set for the sake of simplicity, the real value is calculated below.</para>
    ///   <code>private const float TimePerTile = (float)TravelSpeed.Grunt / 1000;</code>
    /// </summary>
    private const float TimePerTile = 0.2f;
    /// <summary>
    /// Used for deciding <see cref="facingDirection"/>, which determines the animations played.
    /// </summary>
    private Vector3 diffVector; // TODO: Replace with better solution?
    /// <summary>
    /// Used for deciding which animations are played.
    /// </summary>
    public CompassDirection facingDirection = CompassDirection.South;

    public bool isSelected;
    private bool isMoving;
    private bool hasMoved;

    private const float WalkFrameRate = 12;
    private const float IdleFrameRate = 8;
    private float idleTime;

    // Variables for pathfinding 
    private Vector3 targetPosition;
    public NavTile startNode;
    public NavTile endNode;
    public List<NavTile> path;

    private void Start() {
      SwitchGruntAnimations(tool);

      targetPosition = transform.position;
    }

    private void Update() {
      PlaySouthIdleAnimationByDefault();
      PlayWalkAndIdleAnimations();
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

      diffVector = destination - transform.position;
      StartCoroutine(Move(destination));
    
      path.RemoveAt(0);
    }

    // Here we wait for the amount of time needed for the Grunt to move, after which he is moved to the next position.
    // This is meant to be the final method of calculating where any of the Gruntz are, but there is the task of
    // seemingly moving Gruntz towards their target position, with their real position unchanged.
    private IEnumerator Move(Vector3 destination) {
      yield return new WaitForSeconds(TimePerTile);

      transform.position = destination;
    }

    private void SetTargetPosition() {
      // Handling Arrowz that force movement
      foreach (
        Arrow arrow in MapManager.Instance.arrowz
          .Where(arrow => arrow != null // The Arrow exists
                          && (Vector2)arrow.transform.position == (Vector2)transform.position // There is a Grunt on the Arrow
                          && arrow.spriteRenderer.enabled) // The Arrow is visible
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

        // Return, so that Arrow movement cancels any previous move command
        return;
      }

      // Actually set the target position
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
  
    // Handling clicking selection
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
  
    private List<Sprite> GetWalkSprites() {
      List<Sprite> selectedSprites = null;
    
      switch (diffVector.y) {
        case < 0: {
          selectedSprites = animations.WalkSouth;
          facingDirection = CompassDirection.South;
    
          switch (diffVector.x) {
            case > 0: {
              selectedSprites = animations.WalkSouthEast;
              facingDirection = CompassDirection.SouthEast;
              break;
            }
            case < 0: {
              selectedSprites = animations.WalkSouthWest;
              facingDirection = CompassDirection.SouthWest;
              break;
            }
          }
          break;
        }
        case > 0: {
          selectedSprites = animations.WalkNorth;
          facingDirection = CompassDirection.North;
    
          switch (diffVector.x) {
            case > 0: {
              selectedSprites = animations.WalkNorthEast;
              facingDirection = CompassDirection.NorthEast;
              break;
            }
            case < 0: {
              selectedSprites = animations.WalkNorthWest;
              facingDirection = CompassDirection.NorthWest;
              break;
            }
          }
          break;
        }
        default: {
          switch (diffVector.x) {
            case > 0: {
              selectedSprites = animations.WalkEast;
              facingDirection = CompassDirection.East;
              break;
            }
            case < 0: {
              selectedSprites = animations.WalkWest;
              facingDirection = CompassDirection.West;
              break;
            }
          }
          break;
        }
      }
    
      return selectedSprites;
    }
    
    private List<Sprite> GetIdleSprites() {
      List<Sprite> selectedSprites = facingDirection switch {
        CompassDirection.North => animations.IdleNorth,
        CompassDirection.NorthEast => animations.IdleNorthEast,
        CompassDirection.NorthWest => animations.IdleNorthWest,
        CompassDirection.South => animations.IdleSouth,
        CompassDirection.SouthEast => animations.IdleSouthEast,
        CompassDirection.SouthWest => animations.IdleSouthWest,
        CompassDirection.East => animations.IdleEast,
        CompassDirection.West => animations.IdleWest,
        _ => null
      };
    
      return selectedSprites;
    }
    
    private void PlayWalkAndIdleAnimations() {
      if (isMoving) {
        List<Sprite> walkSprites = GetWalkSprites();
      
        if (walkSprites != null) {
          float playTime = Time.time - idleTime;
          int frame = (int)(playTime * WalkFrameRate % walkSprites.Count);
      
          spriteRenderer.sprite = walkSprites[frame];
        }
        else {
          isMoving = false;
          idleTime = Time.time;
        }
      } else {
        List<Sprite> idleSprites = GetIdleSprites();
      
        if (idleSprites != null) {
          float playTime = Time.time - idleTime;
          int frame = (int)(playTime * IdleFrameRate % idleSprites.Count);
      
          spriteRenderer.sprite = idleSprites[frame];
        } else {
          idleTime = Time.time;
        }
      }
    }
  }
}