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
using UnityEngine.AddressableAssets;

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
    public AudioSource audioSource;

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
      _animator = gameObject.GetComponent<Animator>();
      animancer = gameObject.GetComponent<AnimancerComponent>();
      animancer.Animator = _animator;
      BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
      boxCollider.size = Vector2.one;

      audioSource = gameObject.AddComponent<AudioSource>();

      SetAnimPack(equipment.tool.toolName);
      
      InvokeRepeating(nameof(RegenStamina), 0, 1);
    }
    // -------------------------------------------------------------------------------- //

    protected virtual void Update() {
      if (_isDying) {
        return;
      }

      if (health <= 0) {
        StartCoroutine(Death(deathToDie));

        return;
      }

      // Setting flags necessary on all frames
      isInCircle = GameManager.Instance.selectorCircle.ownNode == navigator.ownNode;

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
        } else if (equipment.tool.toolRange == RangeType.Melee) {
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
            if (targetGrunt is null) {
              CleanState();
            } else if (stamina == MaxStatValue) {
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
      // if (GameManager.Instance.currentLevelManager.Holez.Any(hole => hole.location.Equals(navigator.ownLocation) && hole.IsOpen)) {
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
          StartCoroutine(equipment.tool.UseTool());
          stamina = 0;

          // Todo: Move GauntletzGrunt item anim length into constant
          yield return new WaitForSeconds(2f);
          break;
        case Gauntletz when targetMapObject is GiantRockEdge:
          StartCoroutine(equipment.tool.UseTool());
          stamina = 0;

          // Todo: Move GauntletzGrunt item anim length into constant
          yield return new WaitForSeconds(2f);
          break;
        case Gauntletz when targetMapObject is IBreakable:
          StartCoroutine(equipment.tool.UseTool());
          stamina = 0;

          // Todo: Move GauntletzGrunt item anim length into constant
          yield return new WaitForSeconds(2f);
          break;
        case Shovel when targetMapObject is Hole:
          StartCoroutine(equipment.tool.UseTool());
          stamina = 0;

          yield return new WaitForSeconds(1.5f);
          break;
        default:
          #if UNITY_EDITOR
          Debug.Log("Say: Can't do it ");
          #endif

          CleanState();
          break;
      }
    }

    private IEnumerator HandleAttack() {
      gruntState = GruntState.Attack;

      StartCoroutine(equipment.tool.Attack(targetGrunt));
      stamina = 0;

      yield return new WaitForSeconds(equipment.tool.attackContactDelay);
    }

    private IEnumerator HostileIdling() {
      string gruntType = $"{equipment.tool.toolName}Grunt";
      AnimationClip clipToPlay =
        animationPack.Attack[
          $"{gruntType}_Attack_{navigator.facingDirection}_Idle"];

      animancer.Play(clipToPlay);

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

          animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.tool[item.mapItemName]);

          // Todo: Randomized voice
          Addressables.LoadAssetAsync<AudioClip>($"Pickup_Tool_{item.mapItemName}_01.wav").Completed += (handle) => {
            GameManager.Instance.audioSource.PlayOneShot(handle.Result);
          };

          // Todo: Play pickup sound

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

          animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.toy[item.mapItemName]);

          break;
        case nameof(Powerup):
          StatzManager.acquiredPowerupz++;

          animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.powerup[item.mapItemName]);

          break;

        case "Misc":
          switch (item.mapItemName) {
            case nameof(Coin):
              StatzManager.acquiredCoinz++;

              animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.misc[item.mapItemName]);

              break;
            case nameof(Warpletter):
              StatzManager.acquiredWarpletterz++;
              WarpletterType type = ((Warpletter)item).warpletterType;

              Debug.Log($"{item.mapItemName}{type}");
              animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.misc[$"{item.mapItemName}{type}"]);

              break;
          }

          break;
      }

      isInterrupted = true;

      // Wait more than the length of the pickup animations,
      // this way the Grunt holds the item for some time
      yield return new WaitForSeconds(1.5f);

      isInterrupted = false;
    }

    public IEnumerator GetStruck() {
      AnimationClip struckClip =
        animationPack.Struck[$"{equipment.tool.toolName}Grunt_Struck_{navigator.facingDirection}"];

      animancer.Play(struckClip);

      yield return new WaitForSeconds(0.5f);
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

      AnimationClip deathClip = GameManager.Instance.currentAnimationManager.deathPack[deathName];

      animancer.Play(deathClip);

      // Wait the time it takes to play the animation (based on the animation)
      yield return new WaitForSeconds(deathClip.length);

      navigator.ownLocation = Vector2Direction.max;

      if (owner == Owner.Player) {
        GameManager.Instance.currentLevelManager.playerGruntz.Remove(this);
      } else {
        GameManager.Instance.currentLevelManager.enemyGruntz.Remove(this);
      }

      GameManager.Instance.currentLevelManager.allGruntz.Remove(this);
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
      float deathAnimLength = 1f;

      switch (deathName) {
        case DeathName.Burn:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Burn)];
          deathAnimLength = 1f;
          break;
        case DeathName.Electrocute:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Electrocute)];
          deathAnimLength = 1.5f;
          break;
        case DeathName.Explode:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Explode)];
          deathAnimLength = 0.5f;
          break;
        case DeathName.Fall:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Fall)];
          deathAnimLength = 5f;
          break;
        case DeathName.Flyup:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Flyup)];
          deathAnimLength = 1f;
          break;
        case DeathName.Freeze:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Freeze)];
          break;
        case DeathName.Hole:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Hole)];
          deathAnimLength = 1.5f;
          break;
        case DeathName.Karaoke:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Karaoke)];
          deathAnimLength = 15f;
          break;
        case DeathName.Melt:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Melt)];
          deathAnimLength = 1f;
          break;
        case DeathName.Sink:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Sink)];
          deathAnimLength = 1f;
          break;
        case DeathName.Squash:
          deathClip = GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Squash)];
          deathAnimLength = 0.5f;
          break;
      }

      navigator.ownLocation = Vector2Direction.max;
      GameManager.Instance.currentLevelManager.playerGruntz.Remove(this);
      GameManager.Instance.currentLevelManager.allGruntz.Remove(this);

      animancer.Play(deathClip);

      yield return new WaitForSeconds(deathAnimLength);

      Destroy(gameObject);
    }

    public IEnumerator Exit(int idx, float delay) {
      animancer.Play(GameManager.Instance.currentAnimationManager.exitPack[$"Grunt_Exit_0{idx}"]);

      // Wait for Grunt exit animation to finish
      yield return new WaitForSeconds(delay);

      animancer.Play(GameManager.Instance.currentAnimationManager.exitPack["Grunt_Exit_End"]);

      // Wait for Grunt exit end animation to finish
      yield return new WaitForSeconds(2.5f);
    }

    public void SetAnimPack(ToolName tool) {
      animationPack = tool switch {
        ToolName.Barehandz => GameManager.Instance.currentAnimationManager.barehandzGruntPack,
        ToolName.Bomb => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Boomerang => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.BoxingGlovez => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Bricklayer => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Club => GameManager.Instance.currentAnimationManager.clubGruntPack,
        ToolName.Gauntletz => GameManager.Instance.currentAnimationManager.gauntletzGruntPack,
        ToolName.GooberStraw => GameManager.Instance.currentAnimationManager.gooberStrawGruntPack,
        ToolName.GravityBootz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.GunHat => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.NerfGun => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Rockz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Shield => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Shovel => GameManager.Instance.currentAnimationManager.shovelGruntPack,
        ToolName.Spring => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.SpyGear => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Sword => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.TimeBombz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Toob => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Wand => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Warpstone => GameManager.Instance.currentAnimationManager.warpstoneGruntPack,
        ToolName.WelderKit => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Wingz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        _ => throw new InvalidEnumArgumentException("Invalid tool name!"),
      };
    }

    public void SetAnimPack(string tool) {
      animationPack = tool switch {
        nameof(Barehandz) => GameManager.Instance.currentAnimationManager.barehandzGruntPack,
        // ToolName.Bomb => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Boomerang => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.BoxingGlovez => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Bricklayer => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        nameof(Club) => GameManager.Instance.currentAnimationManager.clubGruntPack,
        nameof(Gauntletz) => GameManager.Instance.currentAnimationManager.gauntletzGruntPack,
        nameof(GooberStraw) => GameManager.Instance.currentAnimationManager.gooberStrawGruntPack,
        // ToolName.GravityBootz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.GunHat => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.NerfGun => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Rockz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Shield => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        nameof(Shovel) => GameManager.Instance.currentAnimationManager.shovelGruntPack,
        // ToolName.Spring => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.SpyGear => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Sword => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.TimeBombz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Toob => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Wand => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        nameof(Warpstone) => GameManager.Instance.currentAnimationManager.warpstoneGruntPack,
        // ToolName.WelderKit => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        // ToolName.Wingz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        _ => throw new InvalidEnumArgumentException("Invalid tool name!"),
      };
    }
  }
}
