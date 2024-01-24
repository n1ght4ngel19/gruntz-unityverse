using System;
using System.Collections.Generic;
using System.Linq;

namespace GruntzUnityverse.V2 {
  public class JumpPointSearch {
    public static List<NodeV2> Search(NodeV2 start, NodeV2 goal, HashSet<NodeV2> nodes) {
      Queue<NodeV2> openSet = new Queue<NodeV2>();
      HashSet<NodeV2> closedSet = new HashSet<NodeV2>();

      // Setup starting node
      start.g = 0;
      start.h = Heuristic(start, goal);
      openSet.Enqueue(start);

      // Searching until there are no more nodes to search
      while (openSet.Count > 0) {
        NodeV2 current = openSet.Dequeue();

        // If we've reached the goal, retrace the path
        if (current == goal) {
          return RetracePath(start, goal);
        }

        // The current node cannot be checked again
        closedSet.Add(current);

        // Check all neighbours of the current node
        foreach (NodeV2 neighbour in current.neighbours) {
          // Skip neighbour if it's already checked
          if (closedSet.Contains(neighbour)) {
            continue;
          }

          // Prune neighbour, if possible
          int dx = neighbour.Location2D.x - current.Location2D.x;
          int dy = neighbour.Location2D.y - current.Location2D.y;

          if (dx != 0 && dy != 0) {
            PruneForcedNeighbours(current, neighbour, nodes, closedSet);
          }

          // -----
          // Calculate new g cost for neighbour
          int newMovementCostToNeighbour = current.g + Heuristic(current, neighbour);

          if (newMovementCostToNeighbour >= neighbour.g && openSet.Contains(neighbour)) {
            continue;
          }

          neighbour.g = newMovementCostToNeighbour;
          neighbour.h = Heuristic(neighbour, goal);
          neighbour.parent = current;

          if (!openSet.Contains(neighbour)) {
            openSet.Enqueue(neighbour);
          }
        }
      }

      // No path found
      return null;
    }

    private static void PruneForcedNeighbours(NodeV2 current, NodeV2 neighbour, HashSet<NodeV2> nodes, HashSet<NodeV2> closedSet) {
      int dx = neighbour.Location2D.x - current.Location2D.x;
      int dy = neighbour.Location2D.y - current.Location2D.y;

      // Cardinal move
      if (dx == 0 || dy == 0) {
        return;
      }

      // Diagonal move
      // if (
      //   GetNode(current.location2D.x, neighbour.location2D.y, nodes).walkable
      //   && !GetNode(neighbour.location2D.x, current.location2D.y + dy, nodes).walkable
      //   || !GetNode(current.location2D.x, neighbour.location2D.y, nodes).walkable
      //   && GetNode(neighbour.location2D.x, current.location2D.y, nodes).walkable
      // ) {
      //   closedSet.Add(neighbour);
      // }
    }

    private static NodeV2 GetNode(int x, int y, HashSet<NodeV2> nodes) {
      return nodes.FirstOrDefault(node => node.Location2D.x == x && node.Location2D.y == y);
    }

    private static List<NodeV2> RetracePath(NodeV2 start, NodeV2 goal) {
      List<NodeV2> path = new List<NodeV2>();
      NodeV2 current = goal;

      while (current != start) {
        path.Add(current);
        current = current.parent;
      }

      path.Reverse();

      return path;
    }

    public static int Heuristic(NodeV2 node1, NodeV2 node2) {
      return Math.Abs(node1.Location2D.x - node2.Location2D.x)
        + Math.Abs(node1.Location2D.y - node2.Location2D.y);
    }
  }
}
