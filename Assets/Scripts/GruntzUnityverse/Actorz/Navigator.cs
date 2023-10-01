using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The component describing the movement of a Grunt.
  /// </summary>
  public class Navigator : MonoBehaviour {
    #region Nodes & Locations
    public Vector2Int ownLocation;
    public Node ownNode;
    public Vector2Int targetLocation;
    public Node targetNode;
    public Grunt ownGrunt;
    #endregion

    private bool _doFindPath;

    #region Pathfinding
    public Node pathStart;
    public Node pathEnd;
    public List<Node> path;
    #endregion

    #region Flags
    public bool isMoving;
    public bool isMoveForced;
    public bool movesDiagonally;
    #endregion

    public Vector3 moveVector;
    public Direction facingDirection;
    private const float StepThreshold = 0.1f;

    private void Start() {
      facingDirection = Direction.South;
      ownLocation = Vector2Int.RoundToInt(transform.position);
      ownNode = GameManager.Instance.currentLevelManager.NodeAt(ownLocation);
      targetLocation = ownLocation;
      targetNode = ownNode;
      ownGrunt = gameObject.GetComponent<Grunt>();
      _doFindPath = true;
    }

    private void Update() {
      ownNode = GameManager.Instance.currentLevelManager.NodeAt(ownLocation);

      SetDiagonalFlag();
    }

    public void MoveTowardsTargetNode() {
      if (isMoveForced) {
        HandleForcedMovement(isMoveForced);
      } else if ((targetNode.IsOccupied() || targetNode.IsUnavailable()) && targetNode != ownNode) {
        SetTargetBesideNode(targetNode);
      }

      if (_doFindPath && ownNode != targetNode) {
        path = Pathfinder.PathBetween(ownNode, targetNode, isMoveForced, GameManager.Instance.currentLevelManager.nodes, ownGrunt);
      }

      // There is no path to target or Grunt has reached target
      if ((path is null) || (path.Count <= 1)) {
        switch (ownGrunt.state) {
          case GruntState.MovingToUsing:
            ownGrunt.state = GruntState.Using;
            break;

          case GruntState.MovingToAttacking:
            ownGrunt.state = GruntState.Attacking;
            break;

          case GruntState.MovingToGiving:
            ownGrunt.state = GruntState.Giving;
            break;

          default:
            ownGrunt.CleanState();
            break;
        }

        if (path is not null) {
          ownGrunt.PlayCommandVoice("Bad");
        }

        return;
      }

      ownGrunt.PlayCommandVoice("Good");
      Vector3 nextPosition = LocationAsPosition(path[1].location);
      isMoving = true;

      // Continuing only if Grunt is not close enough to target
      if (Vector2.Distance(nextPosition, transform.position) > StepThreshold) {
        MoveSomeTowards(nextPosition);
        FaceTowards(moveVector);
      } else {
        FinishStep();
      }
    }

    private void HandleForcedMovement(bool isForced) {
      if (!isForced) {
        return;
      }

      Grunt deathMarkedGrunt = GameManager.Instance.currentLevelManager.allGruntz
        .FirstOrDefault(grunt => grunt.navigator.ownNode == targetNode && grunt != ownGrunt);

      // Killing the target if the Grunt was forced to move (e.g. by an Arrow or by teleporting)
      if (deathMarkedGrunt is not null) {
        deathMarkedGrunt.spriteRenderer.sortingOrder = 0;
        StartCoroutine(deathMarkedGrunt.Die(DeathName.Squash));
        deathMarkedGrunt = null;
      }

      isMoveForced = false;
    }

    private void MoveSomeTowards(Vector3 nextPosition) {
      _doFindPath = false;
      moveVector = (nextPosition - gameObject.transform.position).normalized;
      gameObject.transform.position += moveVector * (Time.deltaTime / ownGrunt.moveSpeed);
    }

    private void FinishStep() {
      ownLocation = path[1].location;
      _doFindPath = true;
      isMoving = false;

      path.RemoveAt(1);
    }

    public void SetTargetBesideNode(Node node) {
      List<Node> freeNeighbours = node.Neighbours.FindAll(node1
        => !node1.IsUnavailable()
        && !node1.IsOccupied());

      // No path possible
      if (freeNeighbours.Count == 0) {
        #if UNITY_EDITOR
        Debug.Log("No free neighbours, clearing path.");
        #endif

        path.Clear();

        isMoving = false;
        ownGrunt.haveMoveCommand = false;
        isMoveForced = false;

        // Todo: Play line that says that the Grunt can't move
        return;
      }

      List<Node> shortestPath = new List<Node>();
      int shortestLength = int.MaxValue;

      // Iterate over free neighbours to find shortest path
      foreach (Node neighbour in freeNeighbours) {
        List<Node> pathToNeighbour =
          Pathfinder.PathBetween(ownNode, neighbour, isMoveForced, GameManager.Instance.currentLevelManager.nodes, ownGrunt);

        int pathLength = pathToNeighbour is null
          ? int.MaxValue
          : pathToNeighbour.Count;

        if (pathToNeighbour is null) {
          continue;
        }

        // Check if path to neighbour is shorter than shortest path
        if ((pathLength == int.MaxValue) || (pathLength >= shortestLength)) {
          continue;
        }

        shortestLength = pathLength;
        shortestPath = pathToNeighbour;
      }

      if (shortestLength != int.MaxValue) {
        targetNode = shortestPath.Last();
      }
    }

    private Vector3 LocationAsPosition(Vector2Int location) {
      return new Vector3(location.x, location.y, transform.position.z);
    }

    public void FaceTowards(Vector3 facingVector) {
      Vector2Int directionVector = Vector2Int.RoundToInt(facingVector);

      facingDirection = directionVector switch {
        var vector when vector.Equals(Vector2Direction.north) => Direction.North,
        var vector when vector.Equals(Vector2Direction.northeast) => Direction.Northeast,
        var vector when vector.Equals(Vector2Direction.east) => Direction.East,
        var vector when vector.Equals(Vector2Direction.southeast) => Direction.Southeast,
        var vector when vector.Equals(Vector2Direction.south) => Direction.South,
        var vector when vector.Equals(Vector2Direction.southwest) => Direction.Southwest,
        var vector when vector.Equals(Vector2Direction.west) => Direction.West,
        var vector when vector.Equals(Vector2Direction.northwest) => Direction.Northwest,
        _ => facingDirection,
      };
    }

    private void SetDiagonalFlag() {
      movesDiagonally = facingDirection == Direction.Northeast
        || facingDirection == Direction.Southeast
        || facingDirection == Direction.Southwest
        || facingDirection == Direction.Northwest;
    }
  }
}
