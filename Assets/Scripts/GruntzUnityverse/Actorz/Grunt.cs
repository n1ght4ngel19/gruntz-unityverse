using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  public class Grunt : MonoBehaviour {
    #region Fieldz

    [field: SerializeField] public Equipment Equipment { get; set; }
    [field: SerializeField] public NavComponent NavComponent { get; set; }
    [field: SerializeField] public Animator Animator { get; set; }
    [field: SerializeField] public bool IsSelected { get; set; }
    [field: SerializeField] public bool IsMovementInterrupted { get; set; }
    [field: SerializeField] public bool AllowMoveCommand { get; set; }
    [field: SerializeField] public bool AllowActionCommand { get; set; }
    [field: SerializeField] public Owner Owner { get; set; }
    [field: SerializeField] public MapObject TargetObject { get; set; }

    #endregion


    protected void Start() { Animator = gameObject.GetComponent<Animator>(); }

    private void Update() {
      // Set target to previously saved target, if there is one
      if (!NavComponent.IsMoving && NavComponent.HaveSavedTarget) {
        NavComponent.TargetLocation = NavComponent.SavedTargetLocation;
        NavComponent.HaveSavedTarget = false;

        return;
      }

      Node ownNode = LevelManager.Instance.nodeList.First(node => node.GridLocation.Equals(NavComponent.OwnLocation));

      // Save new target for Grunt when it gets a command while moving to another tile
      if (IsSelected && Input.GetMouseButtonDown(1) && NavComponent.IsMoving) {
        NavComponent.SavedTargetLocation = SelectorCircle.Instance.OwnLocation;
        NavComponent.HaveSavedTarget = true;

        return;
      }

      // Handling order to act
      if (IsSelected && Input.GetMouseButtonDown(0)) {
        if (IsOnLocation(SelectorCircle.Instance.OwnLocation)) {
          // Todo: Bring up Equipment menu
        }

        if (HasTool("Gauntletz")) {
          if (LevelManager.Instance.Rockz.Any(rock => rock.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation))) {
            /*
             * Check all neighbours of the node the Rock is on, and choose the
             * one that has the shortest path to it, or the first if multiple
             * are the exact distance, or none at all, if all paths return 0 length
             */
            foreach (Rock rock in LevelManager.Instance.Rockz.Where(
              rock => rock.OwnLocation.Equals(SelectorCircle.Instance.OwnLocation)
            )) {
              // Get the Rock's Node
              Node rockNode = LevelManager.Instance.nodeList.First(node => node.GridLocation.Equals(rock.OwnLocation));

              // Get non-blocking neighbours of rockNode
              List<Node> nonBlockingNeighbours = rockNode.Neighbours.FindAll(node => !node.isBlocked);

              // Get first possible shortest path
              List<Node> shortestPath = Pathfinder.PathBetween(
                ownNode, nonBlockingNeighbours[0], NavComponent.IsMovementForced
              );

              bool breakFromLoop = false;

              foreach (Node node in nonBlockingNeighbours) {
                if (shortestPath.Count == 1) {
                  // There is no possible shorter way, set target to shortest path
                  NavComponent.TargetLocation = shortestPath[0]
                    .GridLocation;

                  TargetObject = rock;

                  breakFromLoop = true;

                  break;
                }

                List<Node> pathToNode = Pathfinder.PathBetween(ownNode, node, NavComponent.IsMovementForced);

                // Check if current path is shorter than the one before
                if (pathToNode.Count != 0 && pathToNode.Count < shortestPath.Count) {
                  shortestPath = pathToNode;
                }
              }

              if (shortestPath.Count != 0 && !breakFromLoop) {
                // Set target to closest non-blocking location neighbouring clicked location
                NavComponent.TargetLocation = shortestPath.Last()
                  .GridLocation;

                TargetObject = rock;
              }

              // breakFromLoop breaks just like this one
              break;
            }
          }
        }
      }

      // Handling order to move
      if (IsSelected && Input.GetMouseButtonDown(1) && !IsOnLocation(SelectorCircle.Instance.OwnLocation)) {
        // Todo: Simplify => Remove loop and calculate own Node at initialization for MapObjects
        foreach (Node node in LevelManager.Instance.nodeList) {
          if (!node.GridLocation.Equals(SelectorCircle.Instance.OwnLocation)) {
            continue;
          }

          // Check neighbours of Node for possible destinations if Node is blocked
          if (node.isBlocked) {
            List<Node> freeNeighbours = node.Neighbours.FindAll(node1 => !node1.isBlocked);

            // No path possible
            if (freeNeighbours.Count == 0) {
              // Todo: Play line that says that the Grunt can't move
              break;
            }

            List<Node> shortestPath = Pathfinder.PathBetween(
              ownNode, freeNeighbours[0], NavComponent.IsMovementForced
            );

            bool hasShortestPossiblePath = false;

            // Iterate over neighbours to find shortest path
            foreach (Node neighbour in freeNeighbours) {
              if (shortestPath.Count == 1) {
                // There is no possible shorter way, set target to shortest path
                NavComponent.TargetLocation = shortestPath[0]
                  .GridLocation;

                hasShortestPossiblePath = true;

                break;
              }

              List<Node> pathToNode = Pathfinder.PathBetween(ownNode, neighbour, NavComponent.IsMovementForced);

              // Check if current path is shorter than current shortest path
              if (pathToNode.Count != 0 && pathToNode.Count < shortestPath.Count) {
                shortestPath = pathToNode;
              }
            }

            if (!hasShortestPossiblePath) {
              NavComponent.TargetLocation = shortestPath.Last()
                .GridLocation;
            }
          } else {
            // If Node is free
            NavComponent.TargetLocation = node.GridLocation;
          }

          break;
        }
      }

      if (LevelManager.Instance.nodeList.Any(node => node.isBlocked && IsOnLocation(node.GridLocation))) {
        // Play drowning animation when on a Lake (Water or Death) tile
        if (LevelManager.Instance.LakeLayer.HasTile(
          new Vector3Int(NavComponent.OwnLocation.x, NavComponent.OwnLocation.y, 0)
        )) {
          StartCoroutine(Sink());
        } else {
          // Play squashed animation when on colliding Tile or Object
          StartCoroutine(GetSquashed());
        }
      }

      if (!IsMovementInterrupted) {
        HandleMovement();
      }
    }

    public bool IsOnLocation(Vector2Int location) { return NavComponent.OwnLocation.Equals(location); }


    public IEnumerator BreakRock() {
      Animator.Play($"UseItem_{NavComponent.FacingDirection}");
      IsMovementInterrupted = true;

      // Todo: Wait for the exact time needed for breaking Rockz
      yield return new WaitForSeconds(1);

      IsMovementInterrupted = false;

      StartCoroutine(((Rock)TargetObject).Break());
      TargetObject = null;
    }


    #region Equipment

    public bool HasTool(string tool) {
      if (Equipment.Tool is null) {
        return false;
      }

      return Equipment.Tool.Type.ToString()
        .Equals(tool);
    }

    public bool HasToy(string toy) {
      return Equipment.Toy.Type.ToString()
        .Equals(toy);
    }

    public bool HasPowerup(string powerup) {
      return Equipment.Powerup.Type.ToString()
        .Equals(powerup);
    }

    public bool HasItem(string item) { return HasTool(item) || HasToy(item) || HasPowerup(item); }

    #endregion


    private void HandleMovement() {
      if (IsOnLocation(NavComponent.TargetLocation)) {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();

        // Debug.Log(
        //   Animator.runtimeAnimatorController.animationClips[0]
        //     .name
        // );

        Animator.Play($"Idle_{NavComponent.FacingDirection}");
      } else {
        Animator.Play($"Walk_{NavComponent.FacingDirection}");
        NavComponent.MoveTowardsTarget();
      }
    }


    #region Actionz

    public IEnumerator PickupItem(MapObject item) {
      // Todo: Play corresponding Animation Clip
      Animator.Play("Pickup_Item");
      IsMovementInterrupted = true;

      // Todo: Wait for exact time needed to pick up an Item
      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      IsMovementInterrupted = false;
    }

    #endregion


    #region Deathz

    public IEnumerator GetSquashed() {
      enabled = false;
      Animator.Play("Death_Squash");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      NavComponent.OwnLocation = Vector2IntCustom.Max;
      Destroy(gameObject);
    }

    public IEnumerator Sink() {
      enabled = false;

      // Get appropriate Animation Clip from AnimationManager and set it to 
      Animator.Play("Death_Sink");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      NavComponent.OwnLocation = Vector2IntCustom.Max;
      Destroy(gameObject);
    }

    public IEnumerator FallInHole() {
      enabled = false;
      Animator.Play("Death_Hole");
      IsMovementInterrupted = true;

      yield return new WaitForSeconds(
        Animator.GetCurrentAnimatorStateInfo(0)
          .length
      );

      NavComponent.OwnLocation = Vector2IntCustom.Max;
      Destroy(gameObject);
    }

    #endregion


    protected void OnMouseDown() {
      IsSelected = true;

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(grunt => grunt != this)) {
        grunt.IsSelected = false;
      }
    }
  }
}
