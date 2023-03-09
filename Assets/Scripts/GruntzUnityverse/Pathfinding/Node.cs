using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
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

    public Node previous;

    public bool isBlocked;

    [field: SerializeField] public Vector2Int GridLocation { get; set; }

    [field: SerializeField] public List<Node> Neighbours { get; set; }

    public Node(Vector2Int gridLocation) { GridLocation = gridLocation; }

    public void SetNeighboursOfSelf() {
      List<Node> neighbours = new List<Node>();

      // Up
      Vector2Int location = new Vector2Int(GridLocation.x, GridLocation.y + 1);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Down
      location = new Vector2Int(GridLocation.x, GridLocation.y - 1);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Right
      location = new Vector2Int(GridLocation.x + 1, GridLocation.y);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Left
      location = new Vector2Int(GridLocation.x - 1, GridLocation.y);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Up-Right
      location = new Vector2Int(GridLocation.x + 1, GridLocation.y + 1);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Up-Left
      location = new Vector2Int(GridLocation.x + 1, GridLocation.y - 1);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Down-Right
      location = new Vector2Int(GridLocation.x - 1, GridLocation.y + 1);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Down-Left
      location = new Vector2Int(GridLocation.x - 1, GridLocation.y - 1);

      if (LevelManager.Instance.nodeLocationsList.Contains(location)) {
        neighbours.Add(LevelManager.Instance.nodeList.First(node1 => node1.GridLocation.Equals(location)));
      }

      Neighbours = neighbours;
    }
  }
}
