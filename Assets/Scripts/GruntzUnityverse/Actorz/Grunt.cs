using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using GruntzUnityverse.AnimationPackz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Objectz.Itemz.Toolz;
using GruntzUnityverse.Objectz.MapItemz;
using GruntzUnityverse.Utility;
using JetBrains.Annotations;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    [field: SerializeField] public Owner Owner { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }
    [field: SerializeField] [CanBeNull] public MapObject TargetObject { get; set; }
    [field: SerializeField] public Equipment Equipment { get; set; }
    [field: SerializeField] public AnimancerComponent _Animancer { get; set; }
    public Animator Animator { get; set; }
    public Navigator Navigator { get; set; }
    public bool IsInterrupted { get; set; }
    public GruntAnimationPack AnimationPack { get; set; }

    private void Awake() {
      _Animancer = gameObject.GetComponent<AnimancerComponent>();
      Animator = gameObject.GetComponent<Animator>();
      Navigator = gameObject.GetComponent<Navigator>();
      Equipment = gameObject.GetComponent<Equipment>();
    }

    private void Start() {
      // Todo: Get from AnimationManager
      AnimationPack = AnimationManager.Instance.GauntletzGruntPack;
      // AnimationPack = new GruntAnimationPack(ToolType.Gauntletz);
    }

    private void Update() {
      bool haveMoveCommand = IsSelected && Input.GetMouseButtonDown(1);
      bool haveActionCommand = IsSelected && Input.GetMouseButtonDown(0);

      // Set target to previously saved target, if there is one
      if (!Navigator.IsMoving && Navigator.HaveSavedTarget) {
        Navigator.TargetLocation = Navigator.SavedTargetLocation;
        Navigator.HaveSavedTarget = false;

        return;
      }

      // Save new target for Grunt when it gets a command while moving to another tile
      if (haveMoveCommand && Navigator.IsMoving) {
        Navigator.SavedTargetLocation = SelectorCircle.Instance.OwnLocation;
        Navigator.HaveSavedTarget = true;

        return;
      }

      // Todo: Generalize
      // Handling the case when Grunt has a target already
      if (TargetObject is not null && !IsInterrupted) {
        if (Navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
          if (TargetObject is Rock) {
            StartCoroutine(Equipment.Tool.Use(this));
          }
        }
      }

      // Handling order to act
      if (haveActionCommand) {
        if (SelectorCircle.Instance.OwnLocation.Equals(Navigator.OwnLocation)) {
          // Todo: Bring up Equipment menu
        }

        // Todo: Generalize
        // Checking whether Grunt is interrupted so that its target cannot change mid-action
        if (HasTool(ToolType.Gauntletz) && !IsInterrupted) {
          Rock targetRock = LevelManager.Instance.Rockz.FirstOrDefault(
            rock => rock.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation)
          );

          if (targetRock is not null) {
            Navigator.SetTargetBeside(targetRock.OwnNode);
            TargetObject = targetRock;

            if (Navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
              StartCoroutine(Equipment.Tool.Use(this));
            }
          }
        }

        // Todo: Generalize
        // Checking whether Grunt is interrupted so that its target cannot change mid-action
        if (HasTool(ToolType.Shovel) && !IsInterrupted) {
          Hole targetHole = LevelManager.Instance.Holez.FirstOrDefault(
            rock => rock.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation)
          );

          if (targetHole is not null) {
            Navigator.SetTargetBeside(targetHole.OwnNode);
            TargetObject = targetHole;
          }

          if (Navigator.OwnNode.Neighbours.Contains(TargetObject.OwnNode)) {
            StartCoroutine(((Shovel)Equipment.Tool).DigHole(this));
          }
        }
      }

      // Handling order to move
      if (haveMoveCommand && !IsOnLocation(SelectorCircle.Instance.OwnLocation)) {
        // Check neighbours of Node for possible destinations if Node is blocked
        if (SelectorCircle.Instance.OwnNode.isBlocked
          || LevelManager.Instance.AllGruntz.Any(
            grunt => grunt.Navigator.OwnNode.Equals(SelectorCircle.Instance.OwnNode)
          )) {
          Navigator.SetTargetBeside(SelectorCircle.Instance.OwnNode);
        } else {
          // If Node is free
          Navigator.TargetLocation = SelectorCircle.Instance.OwnNode.OwnLocation;
        }
      }

      // Handling the case when Grunt is on a blocked Node
      if (LevelManager.Instance.nodeList.Any(node => node.isBlocked && IsOnLocation(node.OwnLocation))) {
        // Play drowning animation when on a Lake (Water or Death) tile
        if (LevelManager.Instance.LakeLayer.HasTile(
          new Vector3Int(Navigator.OwnLocation.x, Navigator.OwnLocation.y, 0)
        )) {
          StartCoroutine(Die(DeathType.Sink));
        } else {
          // Play squashed animation when on colliding Tile or Object
          StartCoroutine(Die(DeathType.GetSquashed));
        }
      }

      if (LevelManager.Instance.Holez.Any(hole => hole.OwnLocation.Equals(Navigator.OwnLocation) && hole.IsOpen)) {
        StartCoroutine(Die(DeathType.FallInHole));
      }

      if (!IsInterrupted) {
        HandleMovement();
      }
    }

    public bool IsOnLocation(Vector2Int location) {
      return Navigator.OwnLocation.Equals(location);
    }

    /// <summary>
    /// Decides whether the Grunt has a Tool equipped.
    /// </summary>
    /// <param name="tool">The Tool to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasTool(ToolType tool) {
      return Equipment.Tool is not null && Equipment.Tool.Type.Equals(tool);
    }

    /// <summary>
    /// Decides whether the Grunt has a Toy equipped.
    /// </summary>
    /// <param name="toy">The Toy to check</param>
    /// <returns>True or false according to whether the Grunt has the Item.</returns>
    public bool HasToy(ToyType toy) {
      return Equipment.Toy is not null && Equipment.Toy.Type.Equals(toy);
    }

    /// <summary>
    /// Decides between starting the next iteration of movement while playing the walking animation,
    /// and staying put playing while the idle animation.
    /// </summary>
    private void HandleMovement() {
      if (IsOnLocation(Navigator.TargetLocation)) {
        _Animancer.Play(AnimationPack.Idle[$"GauntletzGrunt_Idle_{Navigator.FacingDirection}_01"]);
      } else {
        _Animancer.Play(AnimationPack.Walk[$"GauntletzGrunt_Walk_{Navigator.FacingDirection}"]);
        Navigator.MoveTowardsTarget();
      }
    }

    public IEnumerator PickupItem(string category, string itemType) {
      // Animator.runtimeAnimatorController =
      //   Resources.Load<AnimatorOverrideController>($"Animationz/OverrideControllerz/Pickup{itemType}");

      switch (category) {
        case "Misc":
          _Animancer.Play(AnimationManager.Instance.PickupPack.Misc[itemType]);

          break;
        case "Tool":
          _Animancer.Play(AnimationManager.Instance.PickupPack.Tool[itemType]);

          break;
        case "Toy":
          _Animancer.Play(AnimationManager.Instance.PickupPack.Toy[itemType]);

          break;
      }

      // Animancer.Play(AnimationManager.Instance.PickupPack.Misc[$"Pickup_{itemType}"]);
      // Animator.Play("Pickup_Item");
      IsInterrupted = true;

      // yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

      // All pickup animations have 0.4s length
      yield return new WaitForSeconds(1f);

      // Overriding Grunt animationz with item-specific animationz (if needed)
      // if (itemType != nameof(Coin)) {
      //   Animator.runtimeAnimatorController = Resources.Load<AnimatorOverrideController>(
      //     $"Animationz/Gruntz/{itemType}Grunt/{itemType}Grunt_AnimatorOverrideController"
      //   );
      // } else {
      //   Animator.runtimeAnimatorController = Resources.Load<AnimatorOverrideController>(
      //     $"Animationz/Gruntz/BareHandzGrunt/BareHandzGrunt_AnimatorOverrideController"
      //   );
      // }

      IsInterrupted = false;
    }

    public IEnumerator Die(DeathType deathType) {
      Death currentDeath = deathType switch {
        DeathType.Sink => Death.Sink,
        DeathType.FallInHole => Death.FallInHole,
        DeathType.GetSquashed => Death.GetSquashed,
        _ => throw new ArgumentOutOfRangeException(nameof(deathType), deathType, null)
      };

      enabled = false;
      IsInterrupted = true;
      Animator.Play(currentDeath.animationName);

      yield return new WaitForSeconds(currentDeath.animationDuration);

      Navigator.OwnLocation = Vector2IntCustom.Max();
      Destroy(gameObject);
    }


    /// <summary>
    /// Selects the Grunt under the mouse and deselects all others.
    /// </summary>
    protected void OnMouseDown() {
      IsSelected = true;

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt != this)) {
        grunt.IsSelected = false;
      }
    }
  }
}
