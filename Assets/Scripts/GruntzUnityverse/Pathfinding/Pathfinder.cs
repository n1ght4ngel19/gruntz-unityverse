using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Pathfinding {
  public static class Pathfinder {
    public static List<Node> PathBetween(Node startNode, Node endNode, bool isForced, List<Node> grid, Grunt pathGrunt) {
      List<Node> openList = new List<Node>();
      List<Node> closedList = new List<Node>();

      // Adding the startNode to have something to begin with
      openList.Add(startNode);

      foreach (Node node in grid) {
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
          if (closedList.Contains(neighbour)
            || neighbour.IsUnavailable()
            || neighbour.IsOccupied()) {
            continue;
          }

          bool doSkip = false;

          foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz) {
            if (grunt == pathGrunt || !grunt.navigator.isMoving) {
              continue;
            }

            if (grunt.navigator.path?.Count >= 2 && grunt.navigator.path[1] == neighbour) {
              #if UNITY_EDITOR
              Debug.Log("Another Grunt is moving to this node, skipping.");
              #endif

              doSkip = true;
            }
          }

          // Skip Node if another Grunt is moving to it already
          if (doSkip) {
            continue;
          }

          // When neighbour is diagonal to the current node, and there is a hard-turn object on the side, skip this neighbour
          if (neighbour.IsDiagonalTo(currentNode)) {
            bool shouldContinue = neighbour.Neighbours.Any(
              neighboursNeighbour => neighboursNeighbour.IsOrthogonalTo(neighbour)
                && neighboursNeighbour.isHardTurn
                && currentNode.Neighbours.Contains(neighboursNeighbour)
            );

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

      // Return null in case there's no path to the endNode
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
      return Math.Max(Math.Abs(startNode.location.x - endNode.location.x),
        Math.Abs(startNode.location.y - startNode.location.y));
    }
  }
}
