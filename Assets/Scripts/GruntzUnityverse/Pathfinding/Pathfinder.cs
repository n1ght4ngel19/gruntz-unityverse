using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Pathfinding {
  public static class Pathfinder {
    public static List<Node> PathBetween(Node startNode, Node endNode, bool isForced, List<Node> nodes) {
      List<Node> openList = new List<Node>();
      List<Node> closedList = new List<Node>();

      // Adding the startNode to have something to begin with
      openList.Add(startNode);

      foreach (Node node in nodes) {
        node.gCost = int.MaxValue;
        node.fCost = node.gCost + node.hCost;
        node.previousNode = null;
      }

      startNode.gCost = 0;
      startNode.hCost = DistanceBetween(startNode, endNode);
      startNode.fCost = startNode.gCost + startNode.hCost;

      // Iterating until there are no Nodes left to check while searching for a path
      while (openList.Count > 0) {
        // Getting the Node with the lowest F cost (the practical distance between it and the endNode)
        Node currentNode = openList.OrderBy(node => node.fCost).First();

        if (currentNode == endNode) {
          return PathTo(endNode);
        }

        openList.Remove(currentNode);

        // Adding currentNode to the closedList so that it cannot be checked again
        closedList.Add(currentNode);

        foreach (Node neighbour in currentNode.Neighbours) {
          if (closedList.Contains(neighbour) || neighbour.IsUnavailable()) {
            continue;
          }

          if (neighbour.IsOccupied() && !isForced) {
            continue;
          }

          // When neighbour is diagonal to the current node, and there is a hard-turn object on the side, skip this neighbour
          if (neighbour.IsDiagonalTo(currentNode)) {
            bool shouldContinue = false;

            foreach (Node neighboursNeighbour in neighbour.Neighbours) {
              if (neighboursNeighbour.IsOrthogonalTo(neighbour)
                && neighboursNeighbour.isHardTurn
                && currentNode.Neighbours.Contains(neighboursNeighbour)) {
                shouldContinue = true;

                break;
              }
            }

            if (shouldContinue) {
              continue;
            }
          }

          int tentativeGCost = currentNode.gCost + DistanceBetween(currentNode, neighbour);

          if (tentativeGCost < neighbour.gCost) {
            neighbour.gCost = tentativeGCost;
            neighbour.hCost = DistanceBetween(neighbour, endNode);
            neighbour.fCost = neighbour.gCost + neighbour.hCost;
            neighbour.previousNode = currentNode;
          }

          if (openList.Contains(neighbour)) {
            continue;
          }

          openList.Add(neighbour);
        }
      }

      // Return an empty List in case there's no path to the endNode
      return null;
    }

    private static List<Node> PathTo(Node end) {
      List<Node> path = new List<Node> {
        end,
      };

      Node currentNode = end;

      while (currentNode.previousNode is not null) {
        path.Add(currentNode.previousNode);
        currentNode = currentNode.previousNode;
      }

      path.Reverse();

      return path;
    }

    private static int DistanceBetween(Node startNode, Node endNode) {
      return Math.Max(
        Math.Abs(startNode.location.x - endNode.location.x),
        Math.Abs(startNode.location.y - startNode.location.y)
      );
    }
  }
}
