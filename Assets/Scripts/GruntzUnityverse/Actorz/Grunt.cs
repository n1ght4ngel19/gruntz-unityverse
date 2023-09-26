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
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.MapObjectz.Itemz.Toolz;
using GruntzUnityverse.MapObjectz.Itemz.Toyz;
using GruntzUnityverse.MapObjectz.MapItemz.Misc;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using Range = GruntzUnityverse.Enumz.Range;

namespace GruntzUnityverse.Actorz {
  public class Grunt : MonoBehaviour {
    private const int MinStatValue = 0;
    private const int MaxStatValue = 20;

    // ------------------------------------------------------------ //
    // Statz
    // ------------------------------------------------------------ //
    #region Statz
    [Header("Statz")] public int gruntId;
    public int playerGruntId;
    public Owner owner;
    public float moveSpeed;
    [Range(MinStatValue, MaxStatValue)] public int health;
    [Range(MinStatValue, MaxStatValue)] public int stamina;
    [Range(MinStatValue, MaxStatValue)] public int staminaRegenRate;
    public int powerupTime;
    public int toyTime;
    public int wingzTime;
    #endregion

    // ------------------------------------------------------------ //
    // Flagz
    // ------------------------------------------------------------ //
    #region Flagz
    [Header("Flagz")] public bool isSelected;
    public bool isInCircle;
    public bool isMoving;
    public bool haveMoveCommand;
    public bool haveActionCommand;
    public bool haveGivingCommand;
    public bool haveMovingToUsingCommand;
    public bool haveMovingToAttackingCommand;
    public bool haveMovingToGivingCommand;
    public bool isInterrupted;
    public bool hasPlayedMovementAcknowledgeSound = true;
    private bool _isDying;
    private bool _alreadyHostileIdling;
    #endregion

    // ------------------------------------------------------------ //
    // Action
    // ------------------------------------------------------------ //
    #region Action
    [Header("Action")] public MapObject targetObject;
    [CanBeNull] public Grunt targetGrunt;
    [CanBeNull] public MapObject targetMapObject;
    public GruntState state;
    #endregion

    // ------------------------------------------------------------ //
    // Componentz
    // ------------------------------------------------------------ //
    #region Componentz
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Navigator navigator;
    [HideInInspector] public Equipment equipment;
    [HideInInspector] public HealthBar healthBar;
    [HideInInspector] public StaminaBar staminaBar;
    [HideInInspector] public AnimancerComponent animancer;
    private Animator _animator;
    public SelectedCircle selectedCircle;
    #endregion

    // ------------------------------------------------------------ //
    // Other
    // ------------------------------------------------------------ //
    #region Other
    public GruntAnimationPack animationPack;
    public DeathName deathToDie;
    public AudioSource audioSource;
    #endregion

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
      state = GruntState.Idle;
      _animator ??= gameObject.AddComponent<Animator>();
      animancer ??= gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = _animator;
      selectedCircle = gameObject.GetComponentInChildren<SelectedCircle>();
      BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
      boxCollider.size = Vector2.one;

      audioSource = gameObject.AddComponent<AudioSource>();
      audioSource.playOnAwake = false;

      SetAnimPack(equipment.tool.toolName);
    }

    protected virtual void Update() {
      // ----------------------------------------
      // Death
      // ----------------------------------------
      #region Death
      if (_isDying) {
        return;
      }
      if (health <= 0) {
        StartCoroutine(Die(deathToDie));

        return;
      }
      if (navigator.ownNode.isDeath || navigator.ownNode.isWater) {
        StartCoroutine(Die(DeathName.Sink));

        int randIdx = Random.Range(1, 16);
        string voicePath = randIdx >= 10
          ? $"Assets/Audio/Voicez/Deathz/Voice_Death_Sink_{randIdx}.wav"
          : $"Assets/Audio/Voicez/Deathz/Voice_Death_Sink_0{randIdx}.wav";
        PlayVoice(voicePath);

        return;
      }
      if (navigator.ownNode.isBlocked) {
        spriteRenderer.sortingLayerName = "AlwaysBottom";

        int randIdx = Random.Range(1, 7);
        PlayVoice($"Assets/Audio/Voicez/Deathz/Voice_Death_Explode_0{randIdx}.wav");

        StartCoroutine(Die(DeathName.Explode));

        return;
      }
      if (GameManager.Instance.currentLevelManager.rollingBallz.Any(ball => ball.ownNode == navigator.ownNode)) {
        spriteRenderer.sortingLayerName = "AlwaysBottom";

        int randIdx = Random.Range(1, 7);
        PlayVoice($"Assets/Audio/Voicez/Deathz/Voice_Death_Explode_0{randIdx}.wav");

        StartCoroutine(Die(DeathName.Squash));

        return;
      }
      if (navigator.ownNode.isHole) {
        StartCoroutine(Die(DeathName.Hole));

        int randIdx = Random.Range(1, 5);
        PlayVoice($"Assets/Audio/Voicez/Deathz/Voice_Death_Hole_0{randIdx}.wav");

        return;
      }
      #endregion

      // ----------------------------------------
      // Stamina regen
      // ----------------------------------------
      switch (stamina) {
        case 0:
          InvokeRepeating(nameof(RegenStamina), 0, 1);
          break;

        case MaxStatValue:
          CancelInvoke(nameof(RegenStamina));
          break;
      }

      // Setting flags necessary on all frames
      isInCircle = GameManager.Instance.selectorCircle.ownNode == navigator.ownNode;

      // ----------------------------------------
      // Attribute barz
      // ----------------------------------------
      #region Attribute barz
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
      #endregion

      // Use isInterrupted to disallow any commands while the Grunt is doing something
      if (isInterrupted) {
        return;
      }

      // ----------------------------------------
      // Action command // Todo: Remove!!!
      // ----------------------------------------
      if (haveActionCommand) {
        state = targetGrunt is not null
          ? GruntState.Attacking
          : targetMapObject is not null
            ? GruntState.Using
            : GruntState.Idle;

        haveActionCommand = false;
      }

      // ----------------------------------------
      // Command
      // ----------------------------------------
      #region Command
      if (haveMoveCommand) {
        state = GruntState.Moving;
        haveMoveCommand = false;
      }

      if (haveGivingCommand) {
        state = GruntState.Giving;

        haveGivingCommand = false;
      }

      if (haveMovingToUsingCommand) {
        state = GruntState.MovingToUsing;

        haveMovingToUsingCommand = false;
      }

      if (haveMovingToAttackingCommand) {
        state = GruntState.MovingToAttacking;

        haveMovingToAttackingCommand = false;
      }

      if (haveMovingToGivingCommand) {
        state = GruntState.MovingToGiving;

        haveMovingToGivingCommand = false;
      }
      #endregion

      // ----------------------------------------
      // Action
      // ----------------------------------------
      switch (state) {
        case GruntState.None:
          break;

        case GruntState.Idle:
          break;

        case GruntState.Moving:
          navigator.MoveTowardsTargetNode();

          break;

        case GruntState.Using:
          if (stamina < MaxStatValue) {
            state = GruntState.UsingIdle;
            break;
          }

          switch (equipment.tool.range) {
            case Range.Melee:
              if (IsNeighbourOf(targetMapObject) && stamina == MaxStatValue) {
                stamina = 0;
                isInterrupted = true;
                StartCoroutine(equipment.tool.UseTool());

                if (targetMapObject == null) {
                  CleanState();
                }
              }

              if (!IsNeighbourOf(targetMapObject)) {
                CleanState(); // Todo: Is this needed?
              }

              break;

            case Range.Ranged:
              break;

            case Range.None:
              throw new InvalidEnumArgumentException($"Range cannot be None for Tool {equipment.tool.name} on Grunt {name}!");
            default:
              throw new ArgumentOutOfRangeException();
          }

          break;

        case GruntState.UsingIdle:
          if (targetMapObject == null) {
            CleanState();

            break;
          }

          if (stamina == MaxStatValue) {
            state = GruntState.Using;
          }

          break;

        case GruntState.Attacking:
          if (stamina < MaxStatValue) {
            state = GruntState.AttackingIdle;
            break;
          }

          switch (equipment.tool.range) {
            case Range.Melee:
              if (IsNeighbourOf(targetGrunt) && stamina == MaxStatValue) {
                stamina = 0;
                isInterrupted = true;
                StartCoroutine(equipment.tool.Attack(targetGrunt));

                if (targetGrunt == null) {
                  CleanState();
                }

                break;
              }

              if (!IsNeighbourOf(targetGrunt)) {
                CleanState(); // Todo: Make the Grunt follow its target instead of idling
              }

              break;

            case Range.Ranged:
              // if (HasWithinRange(targetGrunt) && stamina == MaxStatValue) {
              //   isInterrupted = true;
              //   StartCoroutine(equipment.tool.Attack(targetGrunt));
              // }

              break;

            case Range.None:
              throw new InvalidEnumArgumentException($"Range cannot be None for Tool {equipment.tool.name} on Grunt {name}!");

            default:
              throw new ArgumentOutOfRangeException();
          }

          break;

        case GruntState.AttackingIdle:
          if (targetGrunt == null) {
            CleanState();

            break;
          }

          Vector2Int diffVector = targetGrunt.navigator.ownLocation - navigator.ownLocation;
          navigator.FaceTowards(new Vector3(diffVector.x, diffVector.y, 0));

          if (stamina == MaxStatValue) {
            state = GruntState.Attacking;
          }

          break;

        case GruntState.Giving:
          break;

        case GruntState.MovingToUsing:
          if (!IsNeighbourOf(targetMapObject)) {
            navigator.MoveTowardsTargetNode();
          } else {
            state = GruntState.Using;
          }
          break;

        case GruntState.MovingToAttacking:
          if (IsNeighbourOf(targetGrunt)) {
            state = GruntState.Attacking;
          } else {
            navigator.MoveTowardsTargetNode();
          }

          break;

        case GruntState.MovingToGiving:
          if (!IsNeighbourOf(targetGrunt)) {
            navigator.MoveTowardsTargetNode();
          } else {
            state = GruntState.Giving;
          }
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }

      PlayNonCombatAnimation();
    }

    private void RegenStamina() {
      stamina += staminaRegenRate;
    }

    public void TakeDamage(int damageAmount, int reduction) {
      health -= (damageAmount - reduction);
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
    /// Resets the Grunt.
    /// </summary>
    public void CleanState() {
      navigator.isMoving = false;
      navigator.isMoveForced = false;
      haveActionCommand = false;
      haveGivingCommand = false;
      haveMovingToUsingCommand = false;
      isInterrupted = false;
      targetGrunt = null;
      targetMapObject = null;
      state = GruntState.Idle;
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
    /// Plays the appropriate non-combat animation given the Grunt's current state.
    /// </summary>
    private void PlayNonCombatAnimation() {
      AnimationClip clipToPlay;

      switch (state) {
        case GruntState.Idle:
          clipToPlay = animationPack.Idle[$"{equipment.tool.gruntType}_Idle_{navigator.facingDirection}_01"];
          animancer.Play(clipToPlay);
          break;

        case GruntState.UsingIdle:
          clipToPlay = animationPack.Idle[$"{equipment.tool.gruntType}_Idle_{navigator.facingDirection}_01"];
          animancer.Play(clipToPlay);
          break;

        case GruntState.Moving:
          clipToPlay = animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];
          animancer.Play(clipToPlay);
          break;

        case GruntState.MovingToUsing:
          clipToPlay = animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];
          animancer.Play(clipToPlay);
          break;

        case GruntState.MovingToAttacking:
          clipToPlay = animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];
          animancer.Play(clipToPlay);
          break;

        case GruntState.MovingToGiving:
          clipToPlay = animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];
          animancer.Play(clipToPlay);
          break;

        case GruntState.AttackingIdle:
          clipToPlay = animationPack.Attack[$"{equipment.tool.gruntType}_Attack_{navigator.facingDirection}_Idle"];
          animancer.Play(clipToPlay);
          break;
      }
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
          PlayRandomVoice(nameof(Tool), item);
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
          PlayRandomVoice(nameof(Toy), item);

          break;
        case nameof(Powerup):
          StatzManager.acquiredPowerupz++;

          animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.powerup[item.mapItemName]);
          PlayRandomVoice(nameof(Powerup), item);

          break;

        case "Misc":
          switch (item.mapItemName) {
            case nameof(Coin):
              StatzManager.acquiredCoinz++;

              animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.misc[item.mapItemName]);
              PlayRandomVoice("Misc", item);

              break;
            case nameof(Warpletter):
              StatzManager.acquiredWarpletterz++;

              WarpletterType type = ((Warpletter)item).warpletterType;

              animancer.Play(GameManager.Instance.currentAnimationManager.pickupPack.misc[$"{item.mapItemName}{type}"]);
              PlayRandomVoice("Misc", item);

              break;
          }

          break;
      }

      // Prevent the Grunt from moving while picking up the item
      isInterrupted = true;

      // Wait more than the length of the pickup animations,
      // this way the Grunt holds the item for some time
      yield return new WaitForSeconds(1.5f);

      isInterrupted = false;
    }

    public IEnumerator GetHit() {
      int clipIdx = Random.Range(1, 3);
      string clipKey = $"{equipment.tool.gruntType}_Struck_{navigator.facingDirection}_0{clipIdx}";
      AnimationClip struckClip = animationPack.Struck[clipKey];

      animancer.Play(struckClip);  

      isInterrupted = true;
      yield return new WaitForSeconds(0.5f);
      isInterrupted = false;
    }

    public IEnumerator GetHitBy(Grunt attacker) {
      navigator.facingDirection = DirectionUtility.OppositeOf(attacker.navigator.facingDirection);
      int clipIdx = Random.Range(1, 3);
      string clipKey = $"{equipment.tool.gruntType}_Struck_{navigator.facingDirection}_0{clipIdx}";
      AnimationClip struckClip = animationPack.Struck[clipKey];

      animancer.Play(struckClip);

      isInterrupted = true;
      yield return new WaitForSeconds(0.5f);
      isInterrupted = false;
    }

    /// <summary>
    /// Plays the appropriate death animation and removes the Grunt from the game.
    /// </summary>
    /// <param name="deathName">The death type to execute.</param>
    public IEnumerator Die(DeathName deathName) {
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
          deathAnimLength = 0.5f;
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

      // Todo: Play death sound HERE
      // Addressables.LoadAssetAsync<AudioClip>("").Completed += handle => {
      //   audioSource.PlayOneShot(handle.Result);
      // };

      yield return new WaitForSeconds(deathAnimLength);

      spriteRenderer.enabled = false;
      selectedCircle.spriteRenderer.enabled = false;
      Destroy(gameObject, 1f);
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

    public void PlayRandomVoice(string voiceType, Item pickupItem) {
      int voiceIndex = Random.Range(1, 3);

      switch (voiceType) {
        case "Misc":
          voiceIndex = Random.Range(1, 17);

          Addressables.LoadAssetAsync<AudioClip>($"Assets/Audio/Voicez/Pickupz/Pickup_GenericPickup_{voiceIndex}.wav")
            .Completed += handle => {
            audioSource.Stop();
            audioSource.PlayOneShot(handle.Result);
          };

          break;
        default:
          Addressables.LoadAssetAsync<AudioClip>(
              $"Assets/Audio/Voicez/Pickupz/Pickup_{voiceType}_{pickupItem.mapItemName}_0{voiceIndex}.wav")
            .Completed += handle => {
            audioSource.Stop();
            audioSource.PlayOneShot(handle.Result);
          };

          break;
      }
    }

    public void PlayCommandVoice(string goodOrBad) {
      if (hasPlayedMovementAcknowledgeSound || audioSource.isPlaying) {
        return;
      }

      hasPlayedMovementAcknowledgeSound = true;

      int voiceIndex = Random.Range(1, 11);
      string textVoiceIndex = voiceIndex < 10 ? $"0{voiceIndex}" : $"{voiceIndex}";
      string audioPath = $"Assets/Audio/Voicez/Commandz/{goodOrBad}/Voice_Command_{goodOrBad}_{textVoiceIndex}.wav";

      Addressables.LoadAssetAsync<AudioClip>(audioPath).Completed += handle => {
        audioSource.PlayOneShot(handle.Result);
      };
    }

    public void PlayVoice(string clipKey) {
      Addressables.LoadAssetAsync<AudioClip>(clipKey).Completed += handle => {
        audioSource.PlayOneShot(handle.Result);
      };
    }
  }
}
