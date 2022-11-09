using UnityEngine;

namespace GruntzUnityverse.PathFinding {
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

    /// <summary>
    /// Indicates whether this Node can currently be used in pathfinding
    /// </summary>
    public bool isBlocked;

    /// <summary>
    /// The Node that precedes this Node, used when retracing the path in case of successful pathfinding
    /// </summary>
    public Node previous;

    /// <summary>
    /// The location of this Node inside the bounds of the map
    /// </summary>
    public Vector2Int GridLocation {get; set;}

    public Node(Vector2Int gridLocation) {
      GridLocation = gridLocation;
    }
  }
}
