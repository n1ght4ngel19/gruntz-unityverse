using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Attributez;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Hazardz;
using GruntzUnityverse.MapObjectz.Itemz;
using GruntzUnityverse.PathFinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Actorz
{
  public class Grunt : MonoBehaviour
  {
    public SpriteRenderer Renderer { get; set; }
    public Sprite DisplayFrame { get; set; }
    public List<Sprite> AnimationFrames { get; set; }

    public IAttribute Health { get; set; }
    public IAttribute Stamina { get; set; }
    public HealthBar OwnHealthBar { get; set; }
    public StaminaBar OwnStaminaBar { get; set; }

    public HealthBar healthBarPrefab;
    public StaminaBar staminaBarPrefab;

    // Todo: Extract into Equipment class
    public ToolType tool;
    public ToyType toy;

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

    public CompassDirection facingDirection = CompassDirection.South;

    public bool isSelected;
    private bool isMoving;
    private bool hasMoved;

    private const float WalkFrameRate = 12;
    private const float IdleFrameRate = 8;
    private float idleTime;

    private float elapsedTime;
    private int timesChanged;

    public NavComponent NavComponent { get; set; }

    private void Start()
    {
      Renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
      Health = gameObject.AddComponent<Health>();
      Stamina = gameObject.AddComponent<Stamina>();

      OwnHealthBar = Instantiate(healthBarPrefab, transform, false);
      OwnStaminaBar = Instantiate(staminaBarPrefab, transform, false);

      NavComponent = gameObject.AddComponent<NavComponent>();

      SelectGruntAnimationPack(tool);
      ;
    }

    private void Update()
    {
      timesChanged++;

      OwnHealthBar.Renderer.sprite =
        OwnHealthBar.AnimationFrames[Health.ActualValue];

      OwnStaminaBar.Renderer.sprite =
        OwnStaminaBar.AnimationFrames[Stamina.ActualValue];

      // LevelManager.Instance.mapNodes.First(node => node.GridLocation.Equals(NavComponent.OwnGridLocation)).isBlocked = true;

      HandleSpikez();

      PlaySouthIdleAnimationByDefault();
      PlayWalkAndIdleAnimations();

      // Todo: Replace with HandleMovement()
      Move();
    }

    private IEnumerator Die()
    {
      float playTime = Time.time - idleTime;
      int frame = (int)(playTime * IdleFrameRate % animations.Death.Count);

      Renderer.sprite = animations.Death[frame];

      yield return new WaitForSeconds(0.1f);

      // Destroy(gameObject);
    }

    private void HandleSpikez()
    {
      foreach (
        Spikez spikez in LevelManager.Instance.spikezList
          .Where
          (
            spikez1 => spikez1.GridLocation.Equals(NavComponent.OwnGridLocation)
              && timesChanged % Application.targetFrameRate == 0
          )
      )
      {
        if (Health.ActualValue >= Spikez.Dps)
        {
          Health.ActualValue -= Spikez.Dps;
        }
      }
    }

    public void SelectGruntAnimationPack(ToolType toolType)
    {
      animations = toolType switch {
        ToolType.BareHandz => AnimationManager.BareHandzGruntAnimations,
        ToolType.Club => AnimationManager.ClubGruntAnimations,
        ToolType.Gauntletz => AnimationManager.GauntletzGruntAnimations,
        // ToolType.Warpstone1 => AnimationManager.WarpstoneGruntAnimations, // TODO: Add animations
        _ => throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null)
      };
    }

    /// <summary>
    /// Todo: Replace with HandleMovement()
    /// </summary>
    private void Move()
    {
      // TODO: Extract this into HandleMovement()
      SetTargetGridLocation();
      NavComponent.HandleMovement();
    }

    /// <summary>
    /// Todo: Extract into NavComponent and split into pieces, such as logic determining the separate factors that can interrupt default movement (with a flag that is set when default movement is interrupted)
    /// </summary>
    private void SetTargetGridLocation()
    {
      // Handling Arrowz that force movement
      foreach (Arrow arrow in LevelManager.Instance.arrowz)
      {
        if (
          !arrow.spriteRenderer.enabled
          || Vector2.Distance(arrow.transform.position, transform.position) > 0.025f
        )
        {
          continue;
        }

        NavComponent.TargetGridLocation = arrow.direction switch {
          CompassDirection.North => NavComponent.OwnGridLocation + Vector2Int.up,
          CompassDirection.South => NavComponent.OwnGridLocation + Vector2Int.down,
          CompassDirection.East => NavComponent.OwnGridLocation + Vector2Int.right,
          CompassDirection.West => NavComponent.OwnGridLocation + Vector2Int.left,
          // TODO: Uncomment after reforming VectorExtensions
          // CompassDirection.NorthEast => NavComponent.OwnGridLocation + Vector3Plus.upright,
          // CompassDirection.NorthWest => NavComponent.OwnGridLocation + Vector3Plus.upleft,
          // CompassDirection.SouthEast => NavComponent.OwnGridLocation + Vector3Plus.downright,
          // CompassDirection.SouthWest => NavComponent.OwnGridLocation + Vector3Plus.downleft,
          _ => NavComponent.OwnGridLocation
        };

        // Return, so that Arrow movement cancels manual move command
        return;
      }

      // Todo: Handling other factors that can interrupt movement

      // Set target location when nothing is interrupting
      if (Input.GetMouseButtonDown(1) && isSelected)
      {
        NavComponent.TargetGridLocation = Vector2Int.FloorToInt(SelectorCircle.Instance.transform.position);

        // Todo: Move to a proper place
        isMoving = true;

        if (!hasMoved)
        {
          hasMoved = true;
        }
      }
      else
      {
        NavComponent.TargetGridLocation = Vector2Int.zero;
      }
    }

    // Handling clicking selection
    protected void OnMouseDown()
    {
      isSelected = true;

      foreach (Grunt grunt in LevelManager.Instance.gruntz.Where(grunt => grunt != this))
      {
        grunt.isSelected = false;
      }
    }

    /// <summary>
    /// Todo: Replace this with a parameterized PlayAnimation() method, which would choose animations based on facingDirection
    /// </summary>
    private void PlaySouthIdleAnimationByDefault()
    {
      if (hasMoved)
      {
        return;
      }

      float playTime = Time.time - idleTime;
      int frame = (int)(playTime * IdleFrameRate % animations.IdleSouth.Count);

      Renderer.sprite = animations.IdleSouth[frame];
    }

    /// <summary>
    /// Todo: Replace nested switch statements with a single one that entails the actual cases that can occur,this will lead to better readability, and is less error-prone
    /// </summary>
    private List<Sprite> GetWalkSprites()
    {
      List<Sprite> selectedSprites = null;

      switch (diffVector.y)
      {
        case < 0: {
          selectedSprites = animations.WalkSouth;
          facingDirection = CompassDirection.South;

          switch (diffVector.x)
          {
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

          switch (diffVector.x)
          {
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
          switch (diffVector.x)
          {
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

    /// <summary>
    /// Todo: Replace this with a parameterized GetSprites() function, which would choose Sprites based on facingDirection
    /// </summary>
    private List<Sprite> GetIdleSprites()
    {
      List<Sprite> selectedSprites = facingDirection switch {
        CompassDirection.North => animations.IdleNorth,
        CompassDirection.NorthEast => animations.IdleNorthEast,
        CompassDirection.NorthWest => animations.IdleNorthWest,
        CompassDirection.South => animations.IdleSouth,
        CompassDirection.SouthEast => animations.IdleSouthEast,
        CompassDirection.SouthWest => animations.IdleSouthWest,
        CompassDirection.East => animations.IdleEast,
        CompassDirection.West => animations.IdleWest,
        var _ => null,
      };

      return selectedSprites;
    }

    /// <summary>
    /// Todo: Replace this with a parameterized PlayAnimation() method, which would choose animations based on facingDirection
    /// </summary>
    private void PlayWalkAndIdleAnimations()
    {
      if (isMoving)
      {
        List<Sprite> walkSprites = GetWalkSprites();

        if (walkSprites is not null)
        {
          float playTime = Time.time - idleTime;
          int frame = (int)(playTime * WalkFrameRate % walkSprites.Count);

          Renderer.sprite = walkSprites[frame];
        }
        else
        {
          isMoving = false;
          idleTime = Time.time;
        }
      }
      else
      {
        List<Sprite> idleSprites = GetIdleSprites();

        if (idleSprites != null)
        {
          float playTime = Time.time - idleTime;
          int frame = (int)(playTime * IdleFrameRate % idleSprites.Count);

          Renderer.sprite = idleSprites[frame];
        }
        else
        {
          idleTime = Time.time;
        }
      }
    }
  }
}
