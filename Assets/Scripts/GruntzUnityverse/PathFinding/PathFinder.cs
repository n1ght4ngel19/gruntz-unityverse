using System;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Singletonz;
using UnityEngine;

namespace GruntzUnityverse.PathFinding {
  public static class PathFinder {
    public static List<Node> FindPath(Node startNode, Node endNode) {
      List<Node> openList = new();
      List<Node> closedList = new();

      // Adding the startNode to have something to begin with
      openList.Add(startNode);

      foreach (Node node in MapManager.Instance.mapNodes) {
        node.gCost = int.MaxValue;
        node.fCost = node.gCost + node.hCost;
        node.previous = null;
      }

      startNode.gCost = 0;
      startNode.hCost = GetDistanceBetween(startNode, endNode);
      startNode.fCost = startNode.gCost + startNode.hCost;

      // Iterating until there are no Nodes left to check while searching for a path
      while (openList.Count > 0) {
        // Getting the Node with the lowest F cost (the practical distance between it and the endNode)
        Node currentNode = openList.OrderBy(node => node.fCost).First();

        if (currentNode == endNode) {
          return GetPath(endNode);
        }

        openList.Remove(currentNode);
        // Adding currentNode to the closedList so that it cannot be checked again
        closedList.Add(currentNode);

        List<Node> neighbours = GetNeighbours(currentNode);

        foreach (Node neighbour in neighbours) {
          if (closedList.Contains(neighbour)) {
            continue;
          }

          if (neighbour.isBlocked) {
            continue;
          }

          int tentativeGCost = currentNode.gCost + GetDistanceBetween(currentNode, neighbour);

          if (tentativeGCost < neighbour.gCost) {
            neighbour.gCost = tentativeGCost;
            neighbour.hCost = GetDistanceBetween(neighbour, endNode);
            neighbour.fCost = neighbour.gCost + neighbour.hCost;
            neighbour.previous = currentNode;
          }

          if (openList.Contains(neighbour)) {
            continue;
          }

          openList.Add(neighbour);
        }
      }

      // Return an empty List in case there's no path to the endNode
      return new List<Node>();
    }

    private static List<Node> GetPath(Node end) {
      List<Node> path = new();
      path.Add(end);
      Node currentNode = end;

      while (currentNode.previous != null) {
        path.Add(currentNode.previous);
        currentNode = currentNode.previous;
      }

      path.Reverse();

      return path;
    }

    private static List<Node> GetNeighbours(Node node) {
      List<Node> neighbours = new();

      // Up
      Vector2Int location = new Vector2Int(node.GridLocation.x, node.GridLocation.y + 1);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Down
      location = new Vector2Int(node.GridLocation.x, node.GridLocation.y - 1);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Right
      location = new Vector2Int(node.GridLocation.x + 1, node.GridLocation.y);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Left
      location = new Vector2Int(node.GridLocation.x - 1, node.GridLocation.y);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Up-Right
      location = new Vector2Int(node.GridLocation.x + 1, node.GridLocation.y + 1);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Up-Left
      location = new Vector2Int(node.GridLocation.x + 1, node.GridLocation.y - 1);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Down-Right
      location = new Vector2Int(node.GridLocation.x - 1, node.GridLocation.y + 1);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      // Down-Left
      location = new Vector2Int(node.GridLocation.x - 1, node.GridLocation.y - 1);

      if (MapManager.Instance.mapNodeLocations.Contains(location)) {
        neighbours.Add(MapManager.Instance.mapNodes.First(node1 => node1.GridLocation.Equals(location)));
      }

      return neighbours;
    }

    private static int GetDistanceBetween(Node startNode, Node endNode) {
      return Math.Max(
        Math.Abs(startNode.GridLocation.x - endNode.GridLocation.x),
        Math.Abs(startNode.GridLocation.y - startNode.GridLocation.y));
    }
  }
}
