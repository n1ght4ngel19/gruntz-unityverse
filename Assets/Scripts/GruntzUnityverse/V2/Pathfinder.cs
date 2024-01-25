using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace GruntzUnityverse.V2 {
  /// <summary>
  /// Pathfinder class incorporating multiple pathfinding algorithms (possibly).
  /// </summary>
  public static class Pathfinder {
    /// <summary>
    /// A* search algorithm.
    /// </summary>
    /// <param name="start">The NodeV2 to start the search from.</param>
    /// <param name="end">The NodeV2 to end the search at.</param>
    /// <param name="all">All nodes in the given context.</param>
    /// <returns>A list of nodes representing the path from start to end.</returns>
    public static List<NodeV2> AstarSearch(NodeV2 start, NodeV2 end, HashSet<NodeV2> all, bool debug, Material debugColor) {
      PriorityQueue<NodeV2, int> openSet = new PriorityQueue<NodeV2, int>();
      HashSet<NodeV2> closedSet = new HashSet<NodeV2>();
      int counter = 0;

      openSet.Enqueue(start, start.F);

      foreach (NodeV2 node in all) {
        node.g = int.MaxValue;
        node.h = int.MaxValue;
        node.parent = null;
      }

      start.g = 0;
      start.h = CalculateHeuristic(start, end);

      while (openSet.Count > 0) {
        NodeV2 current = openSet.Dequeue();

        // Debug coloring
        if (debug) {
          all.First(node => node.location2D == current.location2D)
            .GetComponent<SpriteRenderer>()
            .material = debugColor;
        }

        counter++;

        // If goal is reached, retrace the path
        if (current == end) {
          Debug.Log($"Checked {counter} nodes");

          return RetracePath(start, end);
        }

        closedSet.Add(current);

        foreach (NodeV2 neighbour in current.neighbours) {
          if (closedSet.Contains(neighbour) || !neighbour.Walkable) {
            continue;
          }

          // Todo: Check if NodeV2 is another actor's next node, and skip it if so

          // Check if neighbour can't be reached diagonally directly, and skip it if so
          if (current.CannotReachDiagonally(neighbour)) {
            continue;
          }

          int tentativeG = current.g + CalculateHeuristic(current, neighbour);

          if (tentativeG < neighbour.g) {
            neighbour.g = tentativeG;
            neighbour.h = CalculateHeuristic(neighbour, end);
            neighbour.parent = current;
          }

          openSet.Enqueue(neighbour, neighbour.F);
        }
      }

      // Return empty list if no path was found
      return new List<NodeV2>();
    }

    private static int CalculateHeuristic(NodeV2 start, NodeV2 end) {
      return Manhattan(start, end);
    }

    private static int Euclidean(NodeV2 start, NodeV2 end) {
      int dxE = Math.Abs(start.location2D.x - end.location2D.x);
      int dyE = Math.Abs(start.location2D.y - end.location2D.y);

      // Euclidean distance
      int euclideanDistance = (int)Math.Sqrt(dxE * dxE + dyE * dyE);

      return euclideanDistance;
    }

    private static int Chebyshev(NodeV2 start, NodeV2 end) {
      return Math.Max(
        Math.Abs(start.location2D.x - end.location2D.x),
        Math.Abs(start.location2D.y - end.location2D.y)
      );
    }

    private static int Manhattan(NodeV2 start, NodeV2 end) {
      return Math.Abs(start.location2D.x - end.location2D.x) + Math.Abs(start.location2D.y - end.location2D.y);
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
