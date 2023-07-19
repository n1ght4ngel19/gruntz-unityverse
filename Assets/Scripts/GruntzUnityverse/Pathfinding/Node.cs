using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Pathfinding {
  public class Node : MonoBehaviour {
    /// <summary>
    /// The path cost between the start Node and this Node
    /// </summary>
    [Header("Pathfinding Variables")] public int gCost;

    /// <summary>
    /// The estimated path cost between this Node and the end Node
    /// </summary>
    public int hCost;

    /// <summary>
    /// The sum of the F and G costz
    /// </summary>
    public int fCost;

    public Node previousNode;

    /// <summary>
    /// This flag shows whether the Node is on a tile which collides with moving objectz.
    /// Gruntz will NOT use these Nodez for pathfinding.
    /// </summary>
    [Header("Flags")] public bool isBlocked;

    /// <summary>
    /// This flag shows whether the Node is on a tile where Gruntz burn to ashez.
    /// </summary>
    public bool isBurn;

    /// <summary>
    /// This flag shows whether the Node is on a tile that's deadly in some way.
    /// Gruntz WILL use these Nodez for pathfinding.
    /// </summary>
    public bool isDeath;

    /// <summary>
    /// This flag shows whether the Node is on the edge of a Void, Burn, or Water patch.
    /// Gruntz will NOT use these Nodez for pathfinding.
    /// </summary>
    public bool isEdge;

    /// <summary>
    /// This flag shows whether the Node is on a tile which can only be walked around without cutting cornerz.
    /// </summary>
    public bool isHardTurn;

    /// <summary>
    /// This flag shows whether the Node is on a tile where Gruntz fall to their death below.
    /// Gruntz will NOT use these Nodez for pathfinding.
    /// </summary>
    public bool isVoid;

    /// <summary>
    /// This flag shows whether the Node is on a tile where Gruntz can drown.
    /// Gruntz will NOT use these Nodez for pathfinding.
    /// </summary>
    public bool isWater;

    public Vector2Int OwnLocation { get; set; }
    public List<Node> Neighbours { get; set; }

    public Node(Vector2Int ownLocation) {
      OwnLocation = ownLocation;
    }

    public bool IsDiagonalTo(Node neighbour) {
      return neighbour.OwnLocation.x != OwnLocation.x && neighbour.OwnLocation.y != OwnLocation.y;
    }

    public bool IsOrthogonalTo(Node neighbour) {
      return neighbour.OwnLocation.x == OwnLocation.x || neighbour.OwnLocation.y == OwnLocation.y;
    }

    public bool IsUnavailable() {
      return isBlocked || isEdge || isWater;
    }

    public bool IsOccupied() {
      return LevelManager.Instance.allGruntz.Any(grunt => grunt.navigator.ownNode.Equals(this));
    }

    public void SetNeighbours() {
      List<Node> neighbours = new List<Node>();

      AddNodeAt(OwnLocation.OwnNorth(), neighbours);
      AddNodeAt(OwnLocation.OwnSouth(), neighbours);
      AddNodeAt(OwnLocation.OwnEast(), neighbours);
      AddNodeAt(OwnLocation.OwnWest(), neighbours);
      AddNodeAt(OwnLocation.OwnNorthEast(), neighbours);
      AddNodeAt(OwnLocation.OwnNorthWest(), neighbours);
      AddNodeAt(OwnLocation.OwnSouthEast(), neighbours);
      AddNodeAt(OwnLocation.OwnSouthWest(), neighbours);

      Neighbours = neighbours;
    }

    private void AddNodeAt(Vector2Int location, List<Node> neighbours) {
      if (LevelManager.Instance.nodeLocations.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodes.First(node1 => node1.OwnLocation.Equals(location)));
      }
    }
  }
}
