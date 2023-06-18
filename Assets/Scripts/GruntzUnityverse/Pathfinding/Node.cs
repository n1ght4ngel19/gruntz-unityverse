using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Utility;
using UnityEngine;
using UnityEngine.Serialization;

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
    /// The sum of the F and G costs
    /// </summary>
    public int fCost;

    public Node previousNode;

    [Header("Flags")] public bool isBlocked;
    public bool isBurning;
    public bool isLake;
    public bool isDeath;
    public bool isHardTurn;
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
