using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The component describing the movement of a Grunt.
  /// </summary>
  public class Navigator : MonoBehaviour {
    public Vector2Int ownLocation;
    public Node ownNode;
    public Vector2Int previousLocation;
    public Vector2Int targetLocation;
    public Node targetNode;
    public Vector2Int savedTargetLocation;
    private bool _hasSavedTarget;
    public bool hasMoveCommand;


    #region Pathfinding

    public Node pathStart;
    public Node pathEnd;
    public List<Node> path;

    #endregion


    public bool isMoving;
    public bool isMoveForced;
    public bool movesDiagonally;
    public Vector3 moveVector;
    public Direction facingDirection;


    private void Start() {
      facingDirection = Direction.South;
      ownLocation = Vector2Int.RoundToInt(transform.position);
      ownNode = LevelManager.Instance.NodeAt(ownLocation);
      targetLocation = ownLocation;
      targetNode = LevelManager.Instance.NodeAt(targetLocation);
    }

    private void Update() {
      // Save new move target whether the Grunt already has one or not
      if (isMoving && hasMoveCommand) {
        savedTargetLocation = SelectorCircle.Instance.location;
        _hasSavedTarget = true;

        return;
      }

      // Set previously saved target as new target
      if (!isMoving && _hasSavedTarget) {
        targetLocation = savedTargetLocation;
        targetNode = LevelManager.Instance.NodeAt(savedTargetLocation);
        _hasSavedTarget = false;

        return;
      }

      ownNode = LevelManager.Instance.NodeAt(ownLocation);

      DecideDiagonal();
    }

    /// <summary>
    /// Moves the <see cref="Grunt"/> towards its current target.
    /// </summary>
    public void MoveTowardsTarget() {
      pathStart = LevelManager.Instance.NodeAt(ownLocation);
      pathEnd = LevelManager.Instance.NodeAt(targetLocation);

      // This way path is only calculated only when it's needed
      if (!isMoving) {
        path = Pathfinder.PathBetween(pathStart, pathEnd, isMoveForced, movesDiagonally);
      }

      if (path == null) {
        return;
      }

      if (path.Count <= 1) {
        return;
      }

      previousLocation = path[0].OwnLocation;

      Vector3 nextPosition = LocationAsPosition(path[1].OwnLocation);

      if (Vector2.Distance(nextPosition, transform.position) > 0.1f) {
        isMoving = true;
        moveVector = (nextPosition - gameObject.transform.position).normalized;

        // Todo: Swap 0.6f to Grunt speed
        transform.position += moveVector * (Time.deltaTime / 0.6f);

        ChangeFacingDirection(moveVector);

        if (isMoveForced) {
          Grunt deadGrunt = LevelManager.Instance.allGruntz.FirstOrDefault(grunt => grunt.AtLocation(targetLocation));

          if (deadGrunt != null) {
            StartCoroutine(deadGrunt.Death("Squash"));
          }

          isMoveForced = false;
        }
      } else {
        isMoving = false;

        ownLocation = path[1].OwnLocation;

        path.RemoveAt(1);
      }
    }

    public void SetClosestToTarget(Node node) {
      List<Node> freeNeighbours = node.Neighbours.FindAll(node1 => !node1.isBlocked);

      // No path possible
      if (freeNeighbours.Count == 0) {
        // Todo: Play line that says that the Grunt can't move
        return;
      }

      List<Node> shortestPath = Pathfinder.PathBetween(ownNode, freeNeighbours[0], isMoveForced, movesDiagonally);

      bool hasShortestPathPossible = false;

      // Iterate over neighbours to find shortest path
      foreach (Node neighbour in freeNeighbours) {
        if (shortestPath.Count == 1) {
          // There is no possible shorter way, set target to shortest path
          targetLocation = shortestPath[0].OwnLocation;

          hasShortestPathPossible = true;

          break;
        }

        List<Node> pathToNode = Pathfinder.PathBetween(ownNode, neighbour, isMoveForced, movesDiagonally);

        // Check if current path is shorter than current shortest path
        if (pathToNode.Count != 0 && pathToNode.Count < shortestPath.Count) {
          shortestPath = pathToNode;
        }
      }

      if (!hasShortestPathPossible) {
        targetLocation = shortestPath.Last().OwnLocation;
      }
    }

    private Vector3 LocationAsPosition(Vector2Int location) {
      return new Vector3(location.x, location.y, transform.position.z);
    }

    public bool AtLocation(Vector2Int location) {
      return ownLocation == location;
    }

    public void ChangeFacingDirection(Vector3 moveVector) {
      Vector2Int directionVector = Vector2Int.RoundToInt(moveVector);

      facingDirection = directionVector switch {
        var vector when vector.Equals(Vector2IntExtra.North()) => Direction.North,
        var vector when vector.Equals(Vector2IntExtra.NorthEast()) => Direction.Northeast,
        var vector when vector.Equals(Vector2IntExtra.East()) => Direction.East,
        var vector when vector.Equals(Vector2IntExtra.SouthEast()) => Direction.Southeast,
        var vector when vector.Equals(Vector2IntExtra.South()) => Direction.South,
        var vector when vector.Equals(Vector2IntExtra.SouthWest()) => Direction.Southwest,
        var vector when vector.Equals(Vector2IntExtra.West()) => Direction.West,
        var vector when vector.Equals(Vector2IntExtra.NorthWest()) => Direction.Northwest,
        _ => facingDirection,
      };
    }

    private void DecideDiagonal() {
      movesDiagonally = facingDirection == Direction.Northeast
        || facingDirection == Direction.Southeast
        || facingDirection == Direction.Southwest
        || facingDirection == Direction.Northwest;
    }
  }
}
