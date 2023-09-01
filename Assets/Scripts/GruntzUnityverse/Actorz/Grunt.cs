using System;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using JetBrains.Annotations;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.MapObjectz.Itemz;
using GruntzUnityverse.MapObjectz.Itemz.Toolz;
using GruntzUnityverse.MapObjectz.Itemz.Toyz;
using GruntzUnityverse.MapObjectz.MapItemz.Misc;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Grunt : MonoBehaviour {
    private const int MinStatValue = 0;
    private const int MaxStatValue = 20;
    // -------------------------------------------------------------------------------- //

    #region Stats
    [Header("Statz")] public int gruntId;
    public Owner owner;
    public float moveSpeed;
    [Range(MinStatValue, MaxStatValue)] public int health;
    [Range(MinStatValue, MaxStatValue)] public int stamina;
    [Range(MinStatValue, MaxStatValue)] public int staminaRegenRate;
    public int powerupTime;
    public int toyTime;
    public int wingzTime;
    #endregion
    // -------------------------------------------------------------------------------- //

    #region Flags
    [Header("Flagz")] public bool isSelected;
    public bool isInCircle;
    public bool haveActionCommand;
    public bool haveGiveToyCommand;
    public bool canInteract;
    public bool isInterrupted;
    private bool _isDying;
    #endregion
    // -------------------------------------------------------------------------------- //

    #region Action
    [Header("Action")] public MapObject targetObject;
    [CanBeNull] public Grunt targetGrunt;
    [CanBeNull] public MapObject targetMapObject;
    public GruntState gruntState;
    #endregion
    // -------------------------------------------------------------------------------- //

    #region Components
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Equipment equipment;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public StaminaBar staminaBar;
    [HideInInspector] public AnimancerComponent animancer;
    private Animator _animator;
    #endregion
    // -------------------------------------------------------------------------------- //

    #region Other
    public GruntAnimationPack animationPack;
    #endregion
    // -------------------------------------------------------------------------------- //

    public DeathName deathToDie;

    private void Start() {
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      navigator = gameObject.AddComponent<Navigator>();
      equipment = gameObject.AddComponent<Equipment>();
      equipment.tool = gameObject.GetComponents<Tool>().FirstOrDefault();
      equipment.toy = gameObject.GetComponents<Toy>().FirstOrDefault();
      healthBar = gameObject.GetComponentInChildren<HealthBar>();
      health = health <= MinStatValue ? MaxStatValue : health;
      staminaBar = gameObject.GetComponentInChildren<StaminaBar>();
      stamina = stamina <= MinStatValue ? MaxStatValue : stamina;
      gruntState = GruntState.Idle;
      _animator = gameObject.AddComponent<Animator>();
      animancer = gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = _animator;
      BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
      boxCollider.size = Vector2.one;

      InvokeRepeating(nameof(RegenStamina), 0, 1);
    }
    // -------------------------------------------------------------------------------- //

    protected virtual void Update() {
      if (_isDying) {
        Debug.Log("Grunt is dying");
        return;
      }

      if (health <= 0) {
        StartCoroutine(Death(deathToDie));

        return;
      }

      if (targetGrunt == null) {
        CleanState();
      }

      // Setting flags necessary on all frames
      isInCircle = SelectorCircle.Instance.ownNode == navigator.ownNode;

      // ----------------------------------------
      // Attribute bars
      // ----------------------------------------
      healthBar.spriteRenderer.enabled = isSelected || health < MaxStatValue;
      healthBar.spriteRenderer.sprite = health <= 0
        ? healthBar.frames[0]
        : healthBar.frames[health];

      // ReSharper disable once ConditionIsAlwaysTrueOrFalse
      if (stamina > MaxStatValue) {
        stamina = MaxStatValue;
      }

      staminaBar.spriteRenderer.enabled = stamina < MaxStatValue;

      staminaBar.spriteRenderer.sprite = stamina >= staminaBar.frames.Count
        ? staminaBar.frames[^1]
        : staminaBar.frames[stamina];


      // ----------------------------------------
      // Movement
      // ----------------------------------------
      if (navigator.haveMoveCommand) {
        navigator.isMoving = true;
        navigator.MoveTowardsTargetNode();
      }

      // ----------------------------------------
      // Action
      // ----------------------------------------
      if (haveActionCommand && !isInterrupted) {
        // Giving or placing a Toy
        if (haveGiveToyCommand) {
          canInteract = targetGrunt is not null
            ? IsNeighbourOf(targetGrunt)
            : IsNeighbourOf(navigator.targetNode);

          gruntState = GruntState.GiveToy;
          // Attacking a Grunt or using a Tool
        } else if (equipment.tool.rangeType == RangeType.Melee) {
          canInteract = targetGrunt is not null
            ? IsNeighbourOf(targetGrunt)
            : IsNeighbourOf(targetMapObject);

          gruntState = targetGrunt is not null
            ? GruntState.Hostile
            : GruntState.Use;
        }

        navigator.haveMoveCommand = !canInteract;
      }

      // ----------------------------------------
      // Interaction (Attack / Item use / Toy use)
      // ----------------------------------------
      if (canInteract && !isInterrupted) {
        switch (gruntState) {
          case GruntState.None:
            break;

          case GruntState.Hostile:
            if (stamina == MaxStatValue) {
              StartCoroutine(HandleAttack());
              isInterrupted = true;
            } else {
              StartCoroutine(HostileIdling());
              isInterrupted = true;
            }

            break;

          case GruntState.Use:
            if (stamina == MaxStatValue) {
              StartCoroutine(HandleItemUse());
              isInterrupted = true;
            }

            break;
          case GruntState.GiveToy:
            // Give toy
            break;
        }
      }

      // ----------------------------------------
      // Death
      // ----------------------------------------
      // Todo

      // Todo: Fix this
      // #region Death handling
      //
      // // Handling the case when Grunt is on a blocked Node
      // Node node = navigator.ownNode;
      //
      // // Handling death
      // if (node.isBurn) {
      //   StartCoroutine(Death("Burn"));
      // }
      //
      // if (node.isWater) {
      //   StartCoroutine(Death("Sink"));
      // }
      //
      // if (node.isBlocked && !node.isBurn && !node.isWater) {
      //   StartCoroutine(Death("Squash"));
      // }
      //
      // // Todo: Move to Hole script!!!
      // if (LevelManager.Instance.Holez.Any(hole => hole.location.Equals(navigator.ownLocation) && hole.IsOpen)) {
      //   StartCoroutine(Death("Hole"));
      // }
      //
      // #endregion

      if (!isInterrupted) {
        PlayWalkOrIdleAnimation();
      }
    }

    private void RegenStamina() {
      if (stamina < MaxStatValue) {
        stamina += staminaRegenRate;
      }
    }

    public void TakeDamage(int damageAmount, int reduction) {
      health -= (damageAmount - reduction);
    }

    private IEnumerator HandleItemUse() {
      // Item
      switch (equipment.tool) {
        // Todo: All valid item use conditions
        case Gauntletz when targetMapObject is GiantRock:
          StartCoroutine(equipment.tool.UseItem());
          stamina = 0;

          yield return new WaitForSeconds(1.5f);
          break;
        case Gauntletz when targetMapObject is GiantRockEdge:
          StartCoroutine(equipment.tool.UseItem());
          stamina = 0;

          yield return new WaitForSeconds(1.5f);
          break;
        case Gauntletz when targetMapObject is IBreakable:
          StartCoroutine(equipment.tool.UseItem());
          stamina = 0;

          yield return new WaitForSeconds(1.5f);
          break;
        case Shovel when targetMapObject is Hole:
          StartCoroutine(equipment.tool.UseItem());
          stamina = 0;

          yield return new WaitForSeconds(1.5f);
          break;
        default:
          Debug.Log("Say: Can't do it ");

          CleanState();
          break;
      }
    }

    private IEnumerator HandleAttack() {
      gruntState = GruntState.Attack;

      StartCoroutine(equipment.tool.Attack(targetGrunt));
      stamina = 0;

      yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator HostileIdling() {
      string gruntType = $"{equipment.tool.toolName}Grunt";
      AnimationClip clipToPlay =
        animationPack.Attack[
          $"{gruntType}_Attack_{navigator.facingDirection}_Idle"];

      animancer.Play(clipToPlay);

      // yield return new WaitForSeconds(0.75f);
      yield return null;

      isInterrupted = false;
    }

    /// <summary>
    /// Checks if the Grunt is a valid target for another Grunt.
    /// </summary>
    /// <param name="otherGrunt">The other Grunt.</param>
    /// <returns>True if the Grunt is a valid target, false otherwise.</returns>
    public bool IsValidTargetFor(Grunt otherGrunt) {
      return otherGrunt != this && otherGrunt.owner != owner;
    }

    /// <summary>
    /// Resets all the Grunt's states and commands to default.
    /// </summary>
    public void CleanState() {
      haveActionCommand = false;
      haveGiveToyCommand = false;
      canInteract = false;
      isInterrupted = false;
      targetGrunt = null;
      targetMapObject = null;
      gruntState = GruntState.Idle;
    }

    /// <summary>
    /// Checks if the Grunt is at the given Node.
    /// </summary>
    /// <param name="node">The Node in question.</param>
    /// <returns>True if the Grunt is at the Node, false otherwise.</returns>
    public bool AtNode(Node node) {
      return navigator.ownNode == node;
    }

    /// <summary>
    /// Checks if the Grunt is beside the given object (Arrow, Rock, etc.).
    /// </summary>
    /// <param name="target">The object to check.</param>
    /// <returns>True if the Grunt is beside the object, false otherwise.</returns>
    public bool IsNeighbourOf(MapObject target) {
      return navigator.ownNode.Neighbours.Contains(target.ownNode);
    }

    /// <summary>
    /// Checks if the Grunt is beside another given Grunt.
    /// </summary>
    /// <param name="target">The Grunt to check.</param>
    /// <returns>True if the Grunt is beside the other Grunt, false otherwise.</returns>
    public bool IsNeighbourOf(Grunt target) {
      return navigator.ownNode.Neighbours.Contains(target.navigator.ownNode);
    }

    /// <summary>
    /// Checks if the Grunt is beside the given Node. 
    /// </summary>
    /// <param name="target">The Node to check.</param>
    /// <returns>True if the Grunt is beside the Node, false otherwise.</returns>
    public bool IsNeighbourOf(Node target) {
      return navigator.ownNode.Neighbours.Contains(target);
    }

    /// <summary>
    /// Checks if the Grunt has the given Tool equipped.
    /// </summary>
    /// <param name="tool">The Tool to check.</param>
    /// <returns>True if the Grunt has the Tool, false otherwise.</returns>
    public bool HasTool(ToolName tool) {
      return equipment.tool is not null && equipment.tool.toolName.Equals(tool);
    }

    /// <summary>
    /// Checks if the Grunt has the given Toy equipped.
    /// </summary>
    /// <param name="toy">The Toy to check.</param>
    /// <returns>True if the Grunt has the Toy, false otherwise.</returns>
    public bool HasToy(ToyName toy) {
      return equipment.toy is not null && equipment.toy.toyName.Equals(toy);
    }

    /// <summary>
    /// Plays the appropriate animation given the Grunt's current state.
    /// </summary>
    private void PlayWalkOrIdleAnimation() {
      animancer.Play(navigator.isMoving
        ? animationPack.Walk[$"{equipment.tool.toolName}Grunt_Walk_{navigator.facingDirection}"]
        : animationPack.Idle[$"{equipment.tool.toolName}Grunt_Idle_{navigator.facingDirection}_01"]);
    }

    public IEnumerator PickupItem(Item item) {
      switch (item.category) {
        case nameof(Tool):
          Destroy(GetComponents<Tool>().FirstOrDefault());

          equipment.tool = item.mapItemName switch {
            nameof(Gauntletz) => gameObject.AddComponent<Gauntletz>(),
            nameof(Shovel) => gameObject.AddComponent<Shovel>(),
            nameof(Warpstone) => gameObject.AddComponent<Warpstone>(),
            _ => throw new InvalidEnumArgumentException(),
          };

          StatzManager.acquiredToolz++;

          animancer.Play(AnimationManager.Instance.PickupPack.tool[item.mapItemName]);
          // Todo: Play pickup sound

          yield return new WaitForSeconds(0.8f);

          SetAnimPack(item.mapItemName);

          break;
        case nameof(Toy):
          Destroy(GetComponents<Toy>().FirstOrDefault());

          equipment.toy = item.mapItemName switch {
            nameof(Beachball) => gameObject.AddComponent<Beachball>(),
            _ => throw new InvalidEnumArgumentException(),
          };

          equipment.toy = gameObject.AddComponent<Beachball>();

          StatzManager.acquiredToyz++;

          animancer.Play(AnimationManager.Instance.PickupPack.toy[item.mapItemName]);

          break;
        case nameof(Powerup):
          StatzManager.acquiredPowerupz++;

          animancer.Play(AnimationManager.Instance.PickupPack.powerup[item.mapItemName]);

          break;

        case "Misc":
          switch (item.mapItemName) {
            case nameof(Coin):
              StatzManager.acquiredCoinz++;

              animancer.Play(AnimationManager.Instance.PickupPack.misc[item.mapItemName]);

              break;
          }

          break;
      }

      isInterrupted = true;

      // Wait the time it takes to pick up an item (subject to change)
      yield return new WaitForSeconds(0.8f);

      isInterrupted = false;
    }

    public void MoveOrDo() {
      if (IsNeighbourOf(targetObject)) {
        StartCoroutine(equipment.tool.Use(this));
        haveActionCommand = false;
        targetObject = null;
      } else {
        navigator.SetTargetBesideNode(targetObject.ownNode);
      }
    }

    public IEnumerator GetStruck() {
      AnimationClip struckClip =
        animationPack.Struck[$"{equipment.tool.toolName}Grunt_Struck_{navigator.facingDirection}"];

      animancer.Play(struckClip);

      yield return new WaitForSeconds(struckClip.length);
    }

    public IEnumerator Death(string deathName) {
      if (!_isDying) {
        _isDying = true;
        transform.position += Vector3.forward * 15;
      }

      healthBar.spriteRenderer.enabled = false;
      // Todo: Stair attribute bars, and move into separate method
      enabled = false;
      navigator.enabled = false;
      isInterrupted = true;

      AnimationClip deathClip = AnimationManager.Instance.DeathPack[deathName];

      animancer.Play(deathClip);

      // Wait the time it takes to play the animation (based on the animation)
      yield return new WaitForSeconds(deathClip.length);

      navigator.ownLocation = Vector2Direction.max;
      LevelManager.Instance.playerGruntz.Remove(this);
      LevelManager.Instance.allGruntz.Remove(this);
      Destroy(gameObject, deathClip.length);
    }

    /// <summary>
    /// Plays the appropriate death animation and removes the Grunt from the game.
    /// </summary>
    /// <param name="deathName">The death type to execute.</param>
    public IEnumerator Death(DeathName deathName) {
      healthBar.spriteRenderer.enabled = false;
      staminaBar.spriteRenderer.enabled = false;
      navigator.enabled = false;
      isInterrupted = true;
      _isDying = true;

      AnimationClip deathClip =
        animationPack.Death[$"{equipment.tool.GetType().Name}Grunt_Death"];
      float deathAnimLength = 2f;

      switch (deathName) {
        case DeathName.Burn:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Burn)];
          deathAnimLength = 1f;
          break;
        case DeathName.Electrocute:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Electrocute)];
          deathAnimLength = 1.5f;
          break;
        case DeathName.Explode:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Explode)];
          deathAnimLength = 0.5f;
          break;
        case DeathName.Fall:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Fall)];
          deathAnimLength = 5f;
          break;
        case DeathName.Flyup:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Flyup)];
          deathAnimLength = 1f;
          break;
        case DeathName.Freeze:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Freeze)];
          break;
        case DeathName.Hole:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Hole)];
          deathAnimLength = 1.5f;
          break;
        case DeathName.Karaoke:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Karaoke)];
          deathAnimLength = 15f;
          break;
        case DeathName.Melt:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Melt)];
          deathAnimLength = 1f;
          break;
        case DeathName.Sink:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Sink)];
          deathAnimLength = 1f;
          break;
        case DeathName.Squash:
          deathClip = AnimationManager.Instance.DeathPack[nameof(DeathName.Squash)];
          deathAnimLength = 0.5f;
          break;
      }

      animancer.Play(deathClip);

      yield return new WaitForSeconds(deathAnimLength);

      navigator.ownLocation = Vector2Direction.max;
      LevelManager.Instance.playerGruntz.Remove(this);
      LevelManager.Instance.allGruntz.Remove(this);
      Destroy(gameObject);
    }

    public void SetAnimPack(ToolName tool) {
      animationPack = tool switch {
        ToolName.Barehandz => AnimationManager.Instance.BarehandzGruntPack,
        ToolName.Gauntletz => AnimationManager.Instance.GauntletzGruntPack,
        ToolName.Shovel => AnimationManager.Instance.ShovelGruntPack,
        _ => AnimationManager.Instance.GauntletzGruntPack,
      };
    }

    public void SetAnimPack(string tool) {
      animationPack = tool switch {
        nameof(Barehandz) => AnimationManager.Instance.BarehandzGruntPack,
        nameof(Gauntletz) => AnimationManager.Instance.GauntletzGruntPack,
        nameof(Shovel) => AnimationManager.Instance.ShovelGruntPack,
        //nameof(Warpstone) => AnimationManager.Instance.WarpstonePack,
        _ => AnimationManager.Instance.BarehandzGruntPack,
      };
    }
  }
}
