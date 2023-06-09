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
    public int gCost;

    /// <summary>
    /// The estimated path cost between this Node and the end Node
    /// </summary>
    public int hCost;

    /// <summary>
    /// The sum of the F and G costs
    /// </summary>
    public int fCost;

    public Node previousNode;
    public bool isBlocked;
    public bool isHardTurn;

    public Vector2Int OwnLocation { get; set; }
    public List<Node> Neighbours { get; set; }

    public Node(Vector2Int ownLocation) {
      OwnLocation = ownLocation;
    }

    public bool IsDiagonalTo(Node neighbour) {
      return neighbour.OwnLocation.x != OwnLocation.x && neighbour.OwnLocation.y != OwnLocation.y;
    }

    public void GetNeighboursAroundSelf() {
      List<Node> neighbours = new List<Node>();

      // North
      AddNodeToNeighboursAt(OwnLocation.OwnNorth(), neighbours);

      // South
      AddNodeToNeighboursAt(OwnLocation.OwnSouth(), neighbours);

      // East
      AddNodeToNeighboursAt(OwnLocation.OwnEast(), neighbours);

      // West
      AddNodeToNeighboursAt(OwnLocation.OwnWest(), neighbours);

      // NorthEast
      AddNodeToNeighboursAt(OwnLocation.OwnNorthEast(), neighbours);

      // NorthWest
      AddNodeToNeighboursAt(OwnLocation.OwnNorthWest(), neighbours);

      // SouthEast
      AddNodeToNeighboursAt(OwnLocation.OwnSouthEast(), neighbours);

      // SouthWest
      AddNodeToNeighboursAt(OwnLocation.OwnSouthWest(), neighbours);

      Neighbours = neighbours;
    }

    private void AddNodeToNeighboursAt(Vector2Int location, List<Node> neighbours) {
      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.OwnLocation.Equals(location)));
      }
    }
  }
}
