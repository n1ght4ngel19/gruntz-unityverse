using System;
using System.Collections;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The class describing Gruntz' behaviour.
  /// </summary>
  public class Grunt : MonoBehaviour {
    [field: SerializeField] public Owner Owner { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }
    [field: SerializeField] public MapObject TargetObject { get; set; }
    [field: SerializeField] public Equipment Equipment { get; set; }
    public Navigator Navigator { get; set; }
    public Animator Animator { get; set; }
    public bool IsInterrupted { get; set; }


    private void Awake() {
      Animator = gameObject.GetComponent<Animator>();
      Navigator = gameObject.GetComponent<Navigator>();
      Equipment = gameObject.GetComponent<Equipment>();
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

      // Handling order to act
      if (haveActionCommand) {
        if (SelectorCircle.Instance.OwnLocation.Equals(Navigator.OwnLocation)) {
          // Todo: Bring up Equipment menu
        }

        // Would this work if Gruntz were able to have multiple Toolz at once?
        if (HasTool(ToolType.Gauntletz)) {
          Rock targetRock = LevelManager.Instance.Rockz.FirstOrDefault(
            rock => rock.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation)
          );

          if (targetRock is not null) {
            Navigator.SetTargetBeside(targetRock.OwnNode);
            TargetObject = targetRock;
          }
        }

        // Would this work if Gruntz were able to have multiple Toolz at once?
        if (HasTool(ToolType.Shovel)) {
          Hole targetHole = LevelManager.Instance.Holez.FirstOrDefault(
            rock => rock.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation)
          );

          if (targetHole is not null) {
            Navigator.SetTargetBeside(targetHole.OwnNode);
            TargetObject = targetHole;
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
        Animator.Play($"Idle_{Navigator.FacingDirection}");
      } else {
        Animator.Play($"Walk_{Navigator.FacingDirection}");
        Navigator.MoveTowardsTarget();
      }
    }

    /// <summary>
    /// Stops the Grunt and makes him play the pickup animation fitting the <paramref name="item"/> argument.
    /// </summary>
    /// <param name="item">A Tool, Toy, Powerup, or Collectible</param>
    /// <returns>An <see cref="IEnumerator"/> since this is a <see cref="Coroutine"/></returns>
    public IEnumerator PickupItem(MapObject item) {
      // Todo: Play corresponding Animation Clip -> More or less the same as the Die() method
      Animator.Play("Pickup_Item");
      IsInterrupted = true;

      // Todo: Wait for exact time needed to pick up an Item
      yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);

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
