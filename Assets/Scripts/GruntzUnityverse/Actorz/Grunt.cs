using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Hazardz;
using GruntzUnityverse.MapObjectz.Itemz;
using GruntzUnityverse.PathFinding;
using GruntzUnityverse.Utilitiez;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Grunt : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public int health = 20;
    [SerializeField] private HealthBar healthBar;
    public int stamina = 20;
    [SerializeField] private StaminaBar staminaBar;

    public HealthBar healthBarPrefab;
    public StaminaBar staminaBarPrefab;

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

    private List<Sprite> deathAnimations;

    /// <summary>
    ///   <para>The amount of time in seconds that a Grunt needs to travel from one tile to another.</para>
    ///   <para>0.2 seconds is set for the sake of simplicity, the real value is calculated below.</para>
    ///   <code>private const float TimePerTile = (float)TravelSpeed.Grunt / 1000;</code>
    /// </summary>
    private const float TimePerTile = 0.6f;

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

    private float elapsedTime;
    private int timesChanged;

    public Vector2Int ownGridLocation;

    public Vector2Int targetGridLocation;
    public Node starter;
    public Node ender;
    public List<Node> nodePath;

    private void Start() {
      SwitchGruntAnimationPack(tool);

      healthBar = Instantiate(healthBarPrefab, transform, false);
      staminaBar = Instantiate(staminaBarPrefab, transform, false);
      ownGridLocation = Vector2Int.FloorToInt(transform.position);
      targetGridLocation = ownGridLocation;
    }

    private void Update() {
      timesChanged++;

      health = gameObject.GetComponentInChildren<HealthBar>().value;
      stamina = gameObject.GetComponentInChildren<StaminaBar>().value;
      ownGridLocation = Vector2Int.FloorToInt(transform.position);

      LevelManager.Instance.mapNodes.First(node => node.GridLocation.Equals(ownGridLocation)).isBlocked = true;

      HandleSpikez();

      PlaySouthIdleAnimationByDefault();

      PlayWalkAndIdleAnimations();

      Move();
    }

    private IEnumerator Die() {
      float playTime = Time.time - idleTime;
      int frame = (int)(playTime * IdleFrameRate % animations.Death.Count);

      spriteRenderer.sprite = animations.Death[frame];

      yield return new WaitForSeconds(0.1f);

      // Destroy(gameObject);
    }

    private void HandleSpikez() {
      foreach (
        Spikez spikez in LevelManager.Instance.spikezList
          .Where(spikez1 => spikez1.GridLocation.Equals(ownGridLocation)
                            && timesChanged % Application.targetFrameRate == 0)
      ) {
        healthBar.value -= Spikez.Dps;
      }
    }

    public void SwitchGruntAnimationPack(ToolType toolType) {
      animations = toolType switch {
        ToolType.BareHandz => AnimationManager.BareHandzGruntAnimations,
        ToolType.Club => AnimationManager.ClubGruntAnimations,
        ToolType.Gauntletz => AnimationManager.GauntletzGruntAnimations,
        // ToolType.Warpstone1 => AnimationManager.WarpstoneGruntAnimations, // TODO: Add animations
        _ => throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null)
      };
    }

    private void Move() {
      SetTargetGridLocation();

      if (!targetGridLocation.Equals(new Vector2Int(0, 0))) {
        ender = LevelManager.Instance.mapNodes.First(node =>
          node.GridLocation.Equals(targetGridLocation));

        if (ender.isBlocked) {
          return;
        }

        // TODO: Move instead to the closest adjacent Node, if possible
        // if node is blocked AND there is a free adjacent position beside it
        // if (ender.isBlocked && thereIsAFreeAdjacentPosition)

        starter = LevelManager.Instance.mapNodes.First(node =>
          node.GridLocation.Equals(ownGridLocation));

        nodePath = PathFinder.FindPath(starter, ender);
      }

      if (nodePath.Count > 1) {
        // Setting the GridLocation the Grunt is standing on to blocked
        LevelManager.Instance.mapNodes.First(node => node.GridLocation.Equals(ownGridLocation)).isBlocked = false;

        Vector3 nextStop = new(
          nodePath[1].GridLocation.x + 0.5f,
          nodePath[1].GridLocation.y + 0.5f,
          -5
        );

        if (Vector2.Distance(nextStop, transform.position) > 0.025f) {
          Vector3 moveDir = (nextStop - transform.position).normalized;
          moveDir.x = Mathf.Round(moveDir.x);
          moveDir.y = Mathf.Round(moveDir.y);
          // moveDir.z = -5;

          // Increased speed for testing (Too fast is buggy!)
          transform.position += moveDir * (Time.deltaTime / 0.3f);

          // Real speed => transform.position += moveDir * (Time.deltaTime / TimePerTile); 
        } else {
          // Moving on to the next Node
          nodePath.RemoveAt(1);
        }
      }
    }

    private void SetTargetGridLocation() {
      // Handling Arrowz that force movement
      foreach (Arrow arrow in LevelManager.Instance.arrowz) {
        if (
          !arrow.spriteRenderer.enabled
          || Vector2.Distance(arrow.transform.position, transform.position) > 0.025f
        ) {
          continue;
        }

        targetGridLocation = arrow.direction switch {
          CompassDirection.North => ownGridLocation + Vector2Int.up,
          CompassDirection.South => ownGridLocation + Vector2Int.down,
          CompassDirection.East => ownGridLocation + Vector2Int.right,
          CompassDirection.West => ownGridLocation + Vector2Int.left,
          // TODO: Uncomment after reforming VectorExtensions
          // CompassDirection.NorthEast => ownGridLocation + Vector3Plus.upright,
          // CompassDirection.NorthWest => ownGridLocation + Vector3Plus.upleft,
          // CompassDirection.SouthEast => ownGridLocation + Vector3Plus.downright,
          // CompassDirection.SouthWest => ownGridLocation + Vector3Plus.downleft,
          _ => ownGridLocation
        };

        // Return, so that Arrow movement cancels manual move command
        return;
      }

      // TODO: Other factors that can interrupt movement come here ...

      // Set target location when nothing else is interrupting
      if (Input.GetMouseButtonDown(1) && isSelected) {
        targetGridLocation = Vector2Int.FloorToInt(SelectorCircle.Instance.transform.position);


        // TODO: Remove / Redo
        isMoving = true;

        if (!hasMoved) {
          hasMoved = true;
        }
      } else {
        targetGridLocation = new Vector2Int(0, 0);
      }
    }

    // Handling clicking selection
    protected void OnMouseDown() {
      isSelected = true;

      foreach (Grunt grunt in LevelManager.Instance.gruntz.Where(grunt => grunt != this)) {
        grunt.isSelected = false;
      }
    }

    // TODO: Redo
    private void PlaySouthIdleAnimationByDefault() {
      if (hasMoved) {
        return;
      }

      float playTime = Time.time - idleTime;
      int frame = (int)(playTime * IdleFrameRate % animations.IdleSouth.Count);

      spriteRenderer.sprite = animations.IdleSouth[frame];
    }

    // TODO: Redo
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

    // TODO: Redo
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

    // TODO: Redo
    private void PlayWalkAndIdleAnimations() {
      if (isMoving) {
        List<Sprite> walkSprites = GetWalkSprites();

        if (walkSprites != null) {
          float playTime = Time.time - idleTime;
          int frame = (int)(playTime * WalkFrameRate % walkSprites.Count);

          spriteRenderer.sprite = walkSprites[frame];
        } else {
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
