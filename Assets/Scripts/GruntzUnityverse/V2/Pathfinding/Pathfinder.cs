using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2.Pathfinding {
  /// <summary>
  /// Pathfinder class incorporating multiple pathfinding algorithms (possibly).
  /// </summary>
  public static class Pathfinder {
    /// <summary>
    /// A* search algorithm.
    /// </summary>
    /// <param name="start">The NodeV2 to start the search from.</param>
    /// <param name="end">The NodeV2 to end the search at.</param>
    /// <param name="nodes">All nodes in the given context.</param>
    /// <returns>A list of nodes representing the path from start to end.</returns>
    public static List<NodeV2> AstarSearch(NodeV2 start, NodeV2 end, HashSet<NodeV2> nodes) {
      if (end == start) {
        return new List<NodeV2>();
      }

      // When the selected end node is not walkable, find a new end node if possible
      if (!end.IsWalkable) {
        // When the target is right beside the start, return an empty list
        if (start.neighbours.Contains(end)) {
          return new List<NodeV2>();
        }

        List<NodeV2> freeNeighbours = end.neighbours.Where(n => n.IsWalkable).ToList();

        if (freeNeighbours.Count == 0) {
          return new List<NodeV2>();
        }

        end = freeNeighbours.OrderBy(n => CalculateHeuristic(start, n)).First();
      }

      HashSet<NodeV2> grid = new HashSet<NodeV2>(nodes);

      // Use a PriorityQueue to keep track of the nodes to check, since we need to sort Nodes in it anyway by their F cost,
      // and a PriorityQueue does this automatically
      PriorityQueue<NodeV2, int> openSet = new PriorityQueue<NodeV2, int>();
      // Use a HashSet to keep track of the nodes that have already been checked,
      // since we don't need to sort them, and but need to check for duplicates
      HashSet<NodeV2> closedSet = new HashSet<NodeV2>();
      int counter = 0;

      openSet.Enqueue(start, start.F);

      foreach (NodeV2 node in grid) {
        node.g = int.MaxValue;
        node.parent = null;
      }

      start.g = 0;
      start.h = CalculateHeuristic(start, end);

      while (openSet.Count > 0) {
        NodeV2 current = openSet.Dequeue();

        counter++;

        // If goal is reached, retrace the path
        if (current == end) {
          return RetracePath(start, end);
        }

        closedSet.Add(current);

        foreach (NodeV2 neighbour in current.neighbours.Where(n => n != current.parent)) {
          if (closedSet.Contains(neighbour) || !neighbour.IsWalkable) {
            continue;
          }

          // Todo: Check if NodeV2 is another actor's next node, and skip it if so

          // Check if neighbour can be reached diagonally directly, and skip it if not
          if (!current.CanReachDiagonally(neighbour)) {
            continue;
          }

          int tentativeG = current.g + CalculateHeuristic(current, neighbour);

          if (tentativeG < neighbour.g) {
            neighbour.g = tentativeG;
            neighbour.h = CalculateHeuristic(neighbour, end);
            neighbour.parent = current;
          }

          // See if PriorityQueue contains neighbour, and only enqueue it if it doesn't
          if (!openSet.UnorderedItems.ToList().Select(tuple => tuple.Item1).Contains(neighbour)) {
            openSet.Enqueue(neighbour, neighbour.F);
          }
        }
      }

      Debug.Log($"No path found, checked {counter} nodes");

      // If no path was found, return an empty list 
      return new List<NodeV2>();
    }

    private enum Heuristic {
      Manhattan,
      Chebyshev,
      Euclidean,
      Octile,
    }

    private static int CalculateHeuristic(NodeV2 start, NodeV2 end, Heuristic heuristic = Heuristic.Manhattan) {
      return heuristic switch {
        Heuristic.Euclidean => Euclidean(start, end),
        Heuristic.Chebyshev => Chebyshev(start, end),
        Heuristic.Octile => Octile(start, end),
        _ => Manhattan(start, end),
      };
    }

    private static int Manhattan(NodeV2 start, NodeV2 end) {
      return Math.Abs(start.location2D.x - end.location2D.x) + Math.Abs(start.location2D.y - end.location2D.y);
    }

    private static int Chebyshev(NodeV2 start, NodeV2 end) {
      return Math.Max(
        Math.Abs(start.location2D.x - end.location2D.x),
        Math.Abs(start.location2D.y - end.location2D.y)
      );
    }

    private static int Euclidean(NodeV2 start, NodeV2 end) {
      int dxE = Math.Abs(start.location2D.x - end.location2D.x);
      int dyE = Math.Abs(start.location2D.y - end.location2D.y);

      // Euclidean distance
      int euclideanDistance = (int)Math.Sqrt(dxE * dxE + dyE * dyE);

      return euclideanDistance;
    }

    private static int Octile(NodeV2 start, NodeV2 end) {
      int dX = Math.Abs(start.location2D.x - end.location2D.x);
      int dY = Math.Abs(start.location2D.y - end.location2D.y);

      return dX + dY + (int)((Math.Sqrt(2) - 2) * Math.Min(dX, dY));
    }

    /// <summary>
    /// Retraces the path from start to end, reversing the initial path produced by the A* algorithm.
    /// </summary>
    /// <param name="start">The start of the path.</param>
    /// <param name="end">The end of the path.</param>
    /// <returns>A list of nodes representing the path from start to end.</returns>
    private static List<NodeV2> RetracePath(NodeV2 start, NodeV2 end) {
      List<NodeV2> path = new List<NodeV2>();
      NodeV2 current = end;

      while (current != start) {
        path.Add(current);
        current = current.parent;
      }

      path.Reverse();

      return path;
    }
  }
}
