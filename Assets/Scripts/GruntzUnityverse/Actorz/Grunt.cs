﻿using System;
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
using GruntzUnityverse.Itemz.MiscItemz;
using GruntzUnityverse.Itemz.Powerupz;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Itemz.Toyz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.Saving;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;
using Range = GruntzUnityverse.Enumz.Range;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    public bool hasSaveData;

    /// <summary>
    /// The data to set the Grunt's state provided by a saved game state.
    /// </summary>
    public GruntData saveData;

    private const int MinStatValue = 0;
    private const int MaxStatValue = 20;

    // ------------------------------------------------------------ //
    // Statz
    // ------------------------------------------------------------ //

    #region Statz
    /// <summary>
    /// The ID differentiating all Gruntz present on a Level.
    /// </summary>
    [Header("Statz")]
    public int gruntId;

    /// <summary>
    /// The ID differentiating the Gruntz on a certain team.
    /// </summary>
    public int playerGruntId;

    /// <summary>
    /// The team controlling the Grunt.
    /// </summary>
    public Team team;

    public float moveSpeed;

    [Range(MinStatValue, MaxStatValue)]
    public int health;

    [Range(MinStatValue, MaxStatValue)]
    public int stamina;

    [Range(MinStatValue, MaxStatValue)]
    public int staminaRegenRate;

    public int powerupTime;
    public int toyTime;
    public int wingzTime;
    #endregion

    // ------------------------------------------------------------ //
    // Flagz
    // ------------------------------------------------------------ //

    #region Flagz
    [Header("Flagz")]
    public bool isSelected;

    public bool isInCircle;
    public bool isMoving;
    public bool haveMoveCommand;
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
    [Header("Action")]
    public MapObject targetObject;

    [CanBeNull]
    public Grunt targetGrunt;

    [CanBeNull]
    public MapObject targetMapObject;

    public GruntState state;
    #endregion

    // ------------------------------------------------------------ //
    // Componentz
    // ------------------------------------------------------------ //

    #region Componentz
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public Navigator navigator;
    public Equipment equipment;

    [HideInInspector]
    public HealthBar healthBar;

    [HideInInspector]
    public StaminaBar staminaBar;

    [HideInInspector]
    public ToyTimeBar toyTimeBar;

    [HideInInspector]
    public WingzTimeBar wingzTimeBar;

    [HideInInspector]
    public AnimancerComponent animancer;

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
      navigator = gameObject.GetComponent<Navigator>();
      equipment = gameObject.GetComponent<Equipment>();
      equipment.tool = gameObject.GetComponents<Tool>().FirstOrDefault();
      equipment.toy = gameObject.GetComponents<Toy>().FirstOrDefault();

      healthBar = gameObject.GetComponentInChildren<HealthBar>();
      health = MaxStatValue;
      healthBar.spriteRenderer.enabled = false;

      staminaBar = gameObject.GetComponentInChildren<StaminaBar>();
      stamina = MaxStatValue;
      staminaBar.spriteRenderer.enabled = false;

      toyTimeBar = gameObject.GetComponentInChildren<ToyTimeBar>();
      toyTime = MinStatValue;
      toyTimeBar.spriteRenderer.enabled = false;

      wingzTimeBar = gameObject.GetComponentInChildren<WingzTimeBar>();
      wingzTime = MinStatValue;
      wingzTimeBar.spriteRenderer.enabled = false;

      state = GruntState.Idle;
      _animator = gameObject.GetComponent<Animator>();
      animancer = gameObject.GetComponent<AnimancerComponent>();
      animancer.Animator = _animator;

      selectedCircle = gameObject.GetComponentInChildren<SelectedCircle>();
      BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
      boxCollider.size = Vector2.one;

      audioSource = gameObject.GetComponent<AudioSource>();
      audioSource.playOnAwake = false;

      SetAnimPack(equipment.tool.toolName);
    }

    // public void SetupGrunt() {
    //   gruntId = GameManager.Instance.currentLevelManager.gruntIdCounter++;
    //   GameManager.Instance.currentLevelManager.allGruntz.Add(this);
    //
    //   if (team == Team.Player) {
    //     playerGruntId = GameManager.Instance.currentLevelManager.playerGruntIdCounter++;
    //     GameManager.Instance.currentLevelManager.playerGruntz.Add(this);
    //   } else {
    //     GameManager.Instance.currentLevelManager.dizGruntled.Add(this);
    //   }
    //
    //   spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    //   navigator = gameObject.GetComponent<Navigator>();
    //   equipment = gameObject.GetComponent<Equipment>();
    //   equipment.tool = gameObject.GetComponents<Tool>().FirstOrDefault();
    //   equipment.toy = gameObject.GetComponents<Toy>().FirstOrDefault();
    //
    //   healthBar = gameObject.GetComponentInChildren<HealthBar>();
    //   health = health <= MinStatValue ? MaxStatValue : health;
    //   staminaBar = gameObject.GetComponentInChildren<StaminaBar>();
    //   stamina = stamina <= MinStatValue ? MaxStatValue : stamina;
    //
    //   state = GruntState.Idle;
    //   _animator = gameObject.GetComponent<Animator>();
    //   animancer = gameObject.GetComponent<AnimancerComponent>();
    //   animancer.Animator = _animator;
    //
    //   selectedCircle = gameObject.GetComponentInChildren<SelectedCircle>();
    //   BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
    //   boxCollider.size = Vector2.one;
    //
    //   audioSource = gameObject.GetComponent<AudioSource>();
    //   audioSource.playOnAwake = false;
    //
    //   SetAnimPack(equipment.tool.toolName);
    // }

    protected virtual void Update() {
      if (hasSaveData) {
        hasSaveData = false;

        HandleSaveData(saveData);
      }

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

      if (GameManager.Instance.currentLevelManager.rollingBallz.Any(
          ball => ball.ownNode == navigator.ownNode
        )) {
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

      staminaBar.spriteRenderer.sprite =
        stamina >= MaxStatValue && !toyTimeBar.spriteRenderer.enabled
          ? staminaBar.frames[^1]
          : staminaBar.frames[stamina];

      toyTimeBar.spriteRenderer.enabled = toyTime > MinStatValue;

      toyTimeBar.spriteRenderer.sprite = toyTime <= MinStatValue
        ? toyTimeBar.frames[0]
        : toyTimeBar.frames[toyTime];
      #endregion

      // Use isInterrupted to disallow any commands while the Grunt is doing something
      if (isInterrupted) {
        return;
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
              throw new InvalidEnumArgumentException(
                $"Range cannot be None for Tool {equipment.tool.name} on Grunt {name}!"
              );
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
              throw new InvalidEnumArgumentException(
                $"Range cannot be None for Tool {equipment.tool.name} on Grunt {name}!"
              );

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
          // Play giving voice here
          GiveToy();
          CleanState();

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

        case GruntState.Playing:
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }

      PlayNonCombatAnimation();
    }

    private void RegenStamina() {
      stamina += staminaRegenRate;
    }

    private void RegenToyTime() {
      toyTime--;
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
      return otherGrunt != this && otherGrunt.team != team;
    }

    /// <summary>
    /// Resets the Grunt.
    /// </summary>
    public void CleanState() {
      // navigator.isMoving = false;
      navigator.isMoveForced = false;
      haveGivingCommand = false;
      haveMovingToUsingCommand = false;
      isInterrupted = false;
      targetGrunt = null;
      targetMapObject = null;
      state = GruntState.Idle;
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
      SetAnimPack(equipment.tool.toolName);

      AnimationClip clipToPlay;

      switch (state) {
        case GruntState.Idle:
          clipToPlay =
            animationPack.Idle[$"{equipment.tool.gruntType}_Idle_{navigator.facingDirection}_01"];

          animancer.Play(clipToPlay);

          break;

        case GruntState.UsingIdle:
          clipToPlay =
            animationPack.Idle[$"{equipment.tool.gruntType}_Idle_{navigator.facingDirection}_01"];

          animancer.Play(clipToPlay);

          break;

        case GruntState.Moving:
          clipToPlay =
            animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];

          animancer.Play(clipToPlay);

          break;

        case GruntState.MovingToUsing:
          clipToPlay =
            animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];

          animancer.Play(clipToPlay);

          break;

        case GruntState.MovingToAttacking:
          clipToPlay =
            animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];

          animancer.Play(clipToPlay);

          break;

        case GruntState.MovingToGiving:
          clipToPlay =
            animationPack.Walk[$"{equipment.tool.gruntType}_Walk_{navigator.facingDirection}"];

          animancer.Play(clipToPlay);

          break;

        case GruntState.AttackingIdle:
          clipToPlay =
            animationPack.Attack[
              $"{equipment.tool.gruntType}_Attack_{navigator.facingDirection}_Idle"];

          animancer.Play(clipToPlay);

          break;
      }
    }

    public void ChangeToolTo(string tool) {
      Destroy(GetComponents<Tool>().FirstOrDefault());

      equipment.tool = tool switch {
        nameof(Barehandz) => gameObject.AddComponent<Barehandz>(),
        nameof(Gauntletz) => gameObject.AddComponent<Gauntletz>(),
        nameof(Shovel) => gameObject.AddComponent<Shovel>(),
        nameof(Warpstone) => gameObject.AddComponent<Warpstone>(),
        nameof(GooberStraw) => gameObject.AddComponent<GooberStraw>(),
        nameof(Club) => gameObject.AddComponent<Club>(),
        _ => throw new InvalidEnumArgumentException(),
      };

      SetAnimPack(tool);
    }

    public IEnumerator PickupItem(Item item) {
      switch (item.category) {
        case nameof(Tool):
          ChangeToolTo(item.mapItemName);

          StatzManager.acquiredToolz++;

          animancer.Play(
            GameManager.Instance.currentAnimationManager.pickupPack.tool[item.mapItemName]
          );

          PlayRandomVoice(nameof(Tool), item);

          break;
        case nameof(Toy):
          Destroy(GetComponents<Toy>().FirstOrDefault());

          equipment.toy = item.mapItemName switch {
            nameof(Beachball) => gameObject.AddComponent<Beachball>(),
            _ => throw new InvalidEnumArgumentException(),
          };

          StatzManager.acquiredToyz++;

          animancer.Play(
            GameManager.Instance.currentAnimationManager.pickupPack.toy[item.mapItemName]
          );

          PlayRandomVoice(nameof(Toy), item);

          break;
        case nameof(Powerup):
          StatzManager.acquiredPowerupz++;

          animancer.Play(
            GameManager.Instance.currentAnimationManager.pickupPack.powerup[item.mapItemName]
          );

          PlayRandomVoice(nameof(Powerup), item);

          if (item is ZapCola cola) {
            health += cola.healAmount;
          }

          break;

        case "Misc":
          switch (item.mapItemName) {
            case nameof(Coin):
              StatzManager.acquiredCoinz++;

              animancer.Play(
                GameManager.Instance.currentAnimationManager.pickupPack.misc[item.mapItemName]
              );

              PlayRandomVoice("Misc", item);

              break;
            case nameof(Warpletter):
              StatzManager.acquiredWarpletterz++;

              WarpletterType type = ((Warpletter)item).warpletterType;

              animancer.Play(
                GameManager.Instance.currentAnimationManager.pickupPack.misc[
                  $"{item.mapItemName}{type}"]
              );

              PlayRandomVoice("Misc", item);

              break;
            case nameof(Helpbox):
              animancer.Play(
                GameManager.Instance.currentAnimationManager.pickupPack.misc[$"{item.mapItemName}"]
              );

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
      if (state == GruntState.Playing) {
        StopCoroutine(nameof(PlayWithToy));
        CleanState();
        toyTime = MinStatValue;
      }

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
      toyTimeBar.spriteRenderer.enabled = false;
      wingzTimeBar.spriteRenderer.enabled = false;
      navigator.enabled = false;
      isInterrupted = true;
      _isDying = true;

      AnimationClip deathClip = animationPack.Death[$"{equipment.tool.GetType().Name}Grunt_Death"];

      string voicePath = "Assets/Audio/Voicez/Deathz/Voice_Death_";
      float deathAnimLength = 2f;

      switch (deathName) {
        case DeathName.Burn:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Burn)];

          deathAnimLength = 1f;

          int burnRandIdx = Random.Range(1, 8);
          voicePath += $"{nameof(DeathName.Burn)}_0{burnRandIdx}.wav";

          break;
        case DeathName.Electrocute:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Electrocute)];

          deathAnimLength = 1.5f;

          int elecRandIdx = Random.Range(1, 10);
          voicePath += $"{nameof(DeathName.Electrocute)}_0{elecRandIdx}.wav";

          break;
        case DeathName.Explode:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Explode)];

          deathAnimLength = 0.5f;

          int expRandIdx = Random.Range(1, 7);
          voicePath += $"{nameof(DeathName.Explode)}_0{expRandIdx}.wav";

          break;
        case DeathName.Fall:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Fall)];

          deathAnimLength = 5f;

          int fallRandIdx = Random.Range(1, 11);
          string fallIdxStr = fallRandIdx >= 10 ? fallRandIdx.ToString() : $"0{fallRandIdx}";
          voicePath += $"{nameof(DeathName.Fall)}_{fallIdxStr}.wav";

          break;
        case DeathName.Flyup:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Flyup)];

          deathAnimLength = 0.5f;

          int flyUpRandIdx = Random.Range(1, 7);
          voicePath += $"{nameof(DeathName.Flyup)}_0{flyUpRandIdx}.wav";

          break;
        case DeathName.Freeze:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Freeze)];

          int freezeRandIdx = Random.Range(1, 10);
          voicePath += $"{nameof(DeathName.Freeze)}_0{freezeRandIdx}.wav";

          break;
        case DeathName.Hole:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Hole)];

          deathAnimLength = 1.5f;

          int holeRandIdx = Random.Range(1, 5);
          voicePath += $"{nameof(DeathName.Hole)}_0{holeRandIdx}.wav";

          break;
        case DeathName.Karaoke:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Karaoke)];

          deathAnimLength = 15f;

          int karaokeRandIdx = Random.Range(1, 10);
          voicePath += $"{nameof(DeathName.Karaoke)}_0{karaokeRandIdx}.wav";

          break;
        case DeathName.Melt:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Melt)];

          deathAnimLength = 1f;

          int meltRandIdx = Random.Range(1, 10);
          voicePath += $"{nameof(DeathName.Melt)}_0{meltRandIdx}.wav";

          break;
        case DeathName.Sink:
          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Sink)];

          deathAnimLength = 1f;

          int sinkRandIdx = Random.Range(1, 11);
          string sinkIdxStr = sinkRandIdx >= 10 ? sinkRandIdx.ToString() : $"0{sinkRandIdx}";
          voicePath += $"{nameof(DeathName.Sink)}_{sinkIdxStr}.wav";

          break;
        case DeathName.Squash:
          transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

          deathClip =
            GameManager.Instance.currentAnimationManager.deathPack[nameof(DeathName.Squash)];

          deathAnimLength = 0.5f;

          int squashRandIdx = Random.Range(1, 7);
          voicePath += $"{nameof(DeathName.Explode)}_0{squashRandIdx}.wav";

          break;
      }

      navigator.ownLocation = Vector2Direction.max;
      GameManager.Instance.currentLevelManager.player1Gruntz.Remove(this);
      GameManager.Instance.currentLevelManager.allGruntz.Remove(this);


      Addressables.InstantiateAsync("GruntPuddle.prefab").Completed += handle => {
        GruntPuddle puddle = handle.Result.GetComponent<GruntPuddle>();
        puddle.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
        puddle.transform.position = transform.position;
        string materialKey = spriteRenderer.material.name.Split("(")[0].Trim();
        // Debug.Log(materialKey);
        puddle.SetMaterial(materialKey);
      };


      Addressables.LoadAssetAsync<AudioClip>(voicePath).Completed += handle => {
        audioSource.PlayOneShot(handle.Result);
      };

      animancer.Play(deathClip);

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
        ToolName.Bricklayer => GameManager.Instance.currentAnimationManager.bricklayerGruntPack,
        ToolName.Club => GameManager.Instance.currentAnimationManager.clubGruntPack,
        ToolName.Gauntletz => GameManager.Instance.currentAnimationManager.gauntletzGruntPack,
        ToolName.GooberStraw => GameManager.Instance.currentAnimationManager.gooberStrawGruntPack,
        ToolName.GravityBootz => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
        ToolName.Gunhat => throw new InvalidEnumArgumentException("Tool not yet implemented!"),
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
        nameof(Bricklayer) => GameManager.Instance.currentAnimationManager.bricklayerGruntPack,
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

          Addressables.LoadAssetAsync<AudioClip>(
              $"Assets/Audio/Voicez/Pickupz/Pickup_GenericPickup_{voiceIndex}.wav"
            )
            .Completed += handle => {
            audioSource.Stop();
            audioSource.PlayOneShot(handle.Result);
          };

          break;
        default:
          Addressables.LoadAssetAsync<AudioClip>(
              $"Assets/Audio/Voicez/Pickupz/Pickup_{voiceType}_{pickupItem.mapItemName}_0{voiceIndex}.wav"
            )
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

      string audioPath =
        $"Assets/Audio/Voicez/Commandz/{goodOrBad}/Voice_Command_{goodOrBad}_{textVoiceIndex}.wav";

      Addressables.LoadAssetAsync<AudioClip>(audioPath).Completed += handle => {
        audioSource.PlayOneShot(handle.Result);
      };
    }

    public void PlayVoice(string clipKey) {
      Addressables.LoadAssetAsync<AudioClip>(clipKey).Completed += handle => {
        audioSource.PlayOneShot(handle.Result);
      };
    }

    public IEnumerator PlayWithToy(ToyName toy) {
      state = GruntState.Playing;

      staminaBar.spriteRenderer.enabled = false;
      wingzTimeBar.spriteRenderer.enabled = false;

      toyTime = MaxStatValue;
      toyTimeBar.spriteRenderer.enabled = true;

      float duration = 2f;

      switch (toy) {
        case ToyName.None:
          break;
        case ToyName.BabyWalker:
          break;
        case ToyName.Beachball:
          animancer.Play(GameManager.Instance.currentAnimationManager.toyPlayPack.beachball1);

          duration = 26f;

          break;
        case ToyName.GoKart:
          break;
        case ToyName.JackInTheBox:
          break;
        case ToyName.Jumprope:
          break;
        case ToyName.MonsterWheelz:
          break;
        case ToyName.PogoStick:
          break;
        case ToyName.Scroll:
          break;
        case ToyName.SqueakToy:
          break;
        case ToyName.YoYo:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(toy), toy, null);
      }

      InvokeRepeating(nameof(RegenToyTime), 0, duration / MaxStatValue);

      yield return new WaitForSeconds(duration);

      CancelInvoke(nameof(RegenToyTime));

      toyTime = MinStatValue;
      toyTimeBar.spriteRenderer.enabled = false;

      CleanState();
    }

    public void GiveToy() {
      StartCoroutine(targetGrunt.PlayWithToy(equipment.toy.toyName));
      Destroy(GetComponent<Toy>());
    }

    public void HandleSaveData(GruntData data) {
      gruntId = data.gruntId;
      gameObject.name = data.gruntName;
      transform.position = data.position;
      team = data.team;

      // g.state = data.state;
      state = data.state switch {
        "Idle" => GruntState.Moving,
        "Moving" => GruntState.Moving,
        "Using" => GruntState.Moving,
        "Attacking" => GruntState.Moving,
        "Giving" => GruntState.Moving,
        _ => GruntState.Moving,
      };

      health = data.health;
      stamina = data.stamina;
      toyTime = data.toyTime;
      wingzTime = data.wingzTime;

      isInterrupted = data.isInterrupted;

      navigator.ownLocation = data.navigatorOwnLocation;

      navigator.ownNode =
        GameManager.Instance.currentLevelManager.NodeAt(data.navigatorOwnLocation);

      navigator.targetNode =
        GameManager.Instance.currentLevelManager.NodeAt(data.navigatorTargetNodeLocation);

      haveMoveCommand = data.haveMoveCommand;

      navigator.isMoving = data.navigatorIsMoving;
      navigator.isMoveForced = data.navigatorIsMoveForced;
      navigator.movesDiagonally = data.navigatorMovesDiagonally;

      navigator.moveVector = data.navigatorMoveVector;
      navigator.facingDirection = data.navigatorFacingDirection;

      if (data.targetGruntId != -1) {
        targetGrunt =
          GameManager.Instance.currentLevelManager.ai1Gruntz.First(
            grunt => grunt.gruntId == data.targetGruntId
          );
      }

      if (data.targetMapObjectId != -1) {
        targetMapObject = GameManager.Instance.currentLevelManager.mapObjectz.First(
          mapObject => mapObject.objectId == data.targetMapObjectId
        );
      }

      // Adding the Grunt to the LevelManager's appropriate lists
      GameManager.Instance.currentLevelManager.allGruntz.Add(this);

      if (team == Team.Player1) {
        GameManager.Instance.currentLevelManager.player1Gruntz.Add(this);
      } else {
        GameManager.Instance.currentLevelManager.ai1Gruntz.Add(this);
      }

      // Setting the Grunt's color
      Addressables.LoadAssetAsync<Material>($"{data.materialKey}.mat").Completed += handle2 => {
        spriteRenderer.material = handle2.Result;
      };
    }
  }
}
