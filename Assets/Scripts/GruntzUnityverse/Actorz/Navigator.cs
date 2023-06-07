using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
  /// <summary>
  /// The component describing the movement of a Grunt.
  /// </summary>
  public class Navigator : MonoBehaviour {
    [field: SerializeField] public Vector2Int OwnLocation { get; set; }
    [field: SerializeField] public Node OwnNode { get; set; }
    [field: SerializeField] public Vector2Int PreviousLocation { get; set; }
    [field: SerializeField] public Vector2Int TargetLocation { get; set; }
    [field: SerializeField] public Vector2Int SavedTargetLocation { get; set; }
    [field: SerializeField] public bool HaveSavedTarget { get; set; }


    #region Pathfinding

    [field: SerializeField] public Node PathStart { get; set; }
    [field: SerializeField] public Node PathEnd { get; set; }
    [field: SerializeField] public List<Node> Path { get; set; }

    #endregion


    [field: SerializeField] public bool IsMoving { get; set; }
    [field: SerializeField] public bool IsMovementForced { get; set; }
    [field: SerializeField] public Vector3 MoveVector { get; set; }
    [field: SerializeField] public Direction FacingDirection { get; set; }


    private void Start() {
      FacingDirection = Direction.South;
      OwnLocation = Vector2Int.FloorToInt(transform.position);
      OwnNode = LevelManager.Instance.NodeAt(OwnLocation);
      TargetLocation = OwnLocation;
    }

    private void Update() {
      // Todo: Maybe not calculate it every frame?
      OwnNode = LevelManager.Instance.NodeAt(OwnLocation);
    }

    /// <summary>
    /// Moves the <see cref="Grunt"/> towards its current target.
    /// </summary>
    public void MoveTowardsTarget() {
      PathStart = LevelManager.Instance.NodeAt(OwnLocation);
      PathEnd = LevelManager.Instance.NodeAt(TargetLocation);

      // This way path is only calculated only when it's needed
      if (!IsMoving) {
        Path = Pathfinder.PathBetween(PathStart, PathEnd, IsMovementForced);
      }

      if (Path == null) {
        return;
      }

      if (Path.Count <= 1) {
        return;
      }

      PreviousLocation = Path[0].OwnLocation;

      Vector3 nextPosition = LocationAsPosition(Path[1].OwnLocation);

      if (Vector2.Distance(nextPosition, transform.position) > 0.1f) {
        IsMoving = true;
        MoveVector = (nextPosition - gameObject.transform.position).normalized;

        transform.position += MoveVector * (Time.deltaTime / 0.6f);

        ChangeFacingDirection(MoveVector);

        if (IsMovementForced) {
          Grunt deadGrunt = LevelManager.Instance.AllGruntz.FirstOrDefault(grunt => grunt.IsOnLocation(TargetLocation));

          if (deadGrunt is not null) {
            StartCoroutine(deadGrunt.Death("Squash"));
          }

          IsMovementForced = false;
        }
      } else {
        IsMoving = false;

        OwnLocation = Path[1].OwnLocation;

        Path.RemoveAt(1);
      }
    }

    public void SetTargetBeside(Node node) {
      List<Node> freeNeighbours = node.Neighbours.FindAll(node1 => !node1.isBlocked);

      // No path possible
      if (freeNeighbours.Count == 0) {
        // Todo: Play line that says that the Grunt can't move
        return;
      }

      List<Node> shortestPath = Pathfinder.PathBetween(OwnNode, freeNeighbours[0], IsMovementForced);

      bool hasShortestPathPossible = false;

      // Iterate over neighbours to find shortest path
      foreach (Node neighbour in freeNeighbours) {
        if (shortestPath.Count == 1) {
          // There is no possible shorter way, set target to shortest path
          TargetLocation = shortestPath[0].OwnLocation;

          hasShortestPathPossible = true;

          break;
        }

        List<Node> pathToNode = Pathfinder.PathBetween(OwnNode, neighbour, IsMovementForced);

        // Check if current path is shorter than current shortest path
        if (pathToNode.Count != 0 && pathToNode.Count < shortestPath.Count) {
          shortestPath = pathToNode;
        }
      }

      if (!hasShortestPathPossible) {
        TargetLocation = shortestPath.Last().OwnLocation;
      }
    }

    private Vector3 LocationAsPosition(Vector2Int location) {
      return new Vector3(location.x + 0.5f, location.y + 0.5f, transform.position.z);
    }

    public void ChangeFacingDirection(Vector3 moveVector) {
      Vector2Int directionVector = Vector2Int.RoundToInt(moveVector);

      FacingDirection = directionVector switch {
        var vector when vector.Equals(Vector2IntCustom.North()) => Direction.North,
        var vector when vector.Equals(Vector2IntCustom.NorthEast()) => Direction.Northeast,
        var vector when vector.Equals(Vector2IntCustom.East()) => Direction.East,
        var vector when vector.Equals(Vector2IntCustom.SouthEast()) => Direction.Southeast,
        var vector when vector.Equals(Vector2IntCustom.South()) => Direction.South,
        var vector when vector.Equals(Vector2IntCustom.SouthWest()) => Direction.Southwest,
        var vector when vector.Equals(Vector2IntCustom.West()) => Direction.West,
        var vector when vector.Equals(Vector2IntCustom.NorthWest()) => Direction.Northwest,
        _ => FacingDirection,
      };
    }
  }
}
