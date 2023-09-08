using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Pathfinding {
  public class Node : MonoBehaviour {
    #region Pathfinding Variables
    /// <summary>
    /// The path cost between the start Node and this Node
    /// </summary>
    [Header("Pathfinding Variables")]
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
    #endregion

    #region Flags
    /// <summary>
    /// This flag shows whether the Node is on a tile which collides with moving objects.
    /// Gruntz will NOT use these Nodes for pathfinding.
    /// </summary>
    [Header("Flags")]
    public bool isBlocked;

    /// <summary>
    /// This flag shows whether the Node is on a tile where Grunts burn to ashes.
    /// </summary>
    public bool isBurn;

    /// <summary>
    /// This flag shows whether the Node is on a tile that's deadly in some way.
    /// Gruntz WILL use these Nodes for pathfinding.
    /// </summary>
    public bool isDeath;

    /// <summary>
    /// This flag shows whether the Node is on the edge of a Void, Burn, or Water patch.
    /// Gruntz will NOT use these Nodes for pathfinding.
    /// </summary>
    public bool isEdge;

    /// <summary>
    /// This flag shows whether the Node is on a tile which can only be walked around without cutting corners.
    /// </summary>
    public bool isHardTurn;

    /// <summary>
    /// This flag shows whether the Node is on a tile where Gruntz fall to their death below.
    /// Gruntz will NOT use these Nodes for pathfinding.
    /// </summary>
    public bool isVoid;

    /// <summary>
    /// This flag shows whether the Node is on a tile where Gruntz can drown.
    /// Gruntz will NOT use these Nodes for pathfinding.
    /// </summary>
    public bool isWater;
    #endregion

    /// <summary>
    /// The location of the Node in the grid.
    /// </summary>
    public Vector2Int location;

    /// <summary>
    /// The list of Nodes adjacent to the current Node.
    /// </summary>
    public List<Node> Neighbours { get; set; }

    /// <summary>
    /// Checks whether the given Node is diagonal to the current Node.
    /// </summary>
    /// <param name="neighbour">The other Node to compare to.</param>
    /// <returns>True if the other Node is diagonal, false othwerwise.</returns>
    public bool IsDiagonalTo(Node neighbour) {
      return neighbour.location.x != location.x && neighbour.location.y != location.y;
    }

    public bool IsOrthogonalTo(Node neighbour) {
      return neighbour.location.x == location.x || neighbour.location.y == location.y;
    }

    /// <summary>
    /// Checks whether the Node is unavailable for pathfinding.
    /// </summary>
    /// <returns>True if the </returns>
    public bool IsUnavailable() {
      return isBlocked || isEdge || isWater;
    }

    /// <summary>
    /// Checks whether the Node is occupied by a Grunt.
    /// </summary>
    /// <returns>True if the Node is occupied, false otherwise.</returns>
    public bool IsOccupied() {
      return GameManager.Instance.currentLevelManager.allGruntz.Any(otherGrunt
        => otherGrunt.navigator.ownNode.Equals(this));
    }

    /// <summary>
    /// Collects all the Nodes adjacent to the current Node.
    /// </summary>
    public void SetNeighbours() {
      List<Node> neighbours = new List<Node>();

      AddNodeAt(location + Vector2Direction.north, neighbours);
      AddNodeAt(location + Vector2Direction.south, neighbours);
      AddNodeAt(location + Vector2Direction.east, neighbours);
      AddNodeAt(location + Vector2Direction.west, neighbours);
      AddNodeAt(location + Vector2Direction.northeast, neighbours);
      AddNodeAt(location + Vector2Direction.northwest, neighbours);
      AddNodeAt(location + Vector2Direction.southeast, neighbours);
      AddNodeAt(location + Vector2Direction.southwest, neighbours);

      Neighbours = neighbours;
    }

    /// <summary>
    /// Adds the Node at the given location to the list of the current Node's neighbours
    /// if there exists one at that location.
    /// </summary>
    /// <param name="loc">The location to check for a neighbour.</param>
    /// <param name="neighbours">The list of Nodes to add the new Node to.</param>
    private static void AddNodeAt(Vector2Int loc, List<Node> neighbours) {
      if (GameManager.Instance.currentLevelManager.nodeLocations.Contains(loc)) {
        neighbours.Add(GameManager.Instance.currentLevelManager.nodes.First(node1 => node1.location.Equals(loc)));
      }
    }
  }
}
