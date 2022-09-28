using System;
using System.Collections.Generic;
using System.Linq;

using Singletonz;

using UnityEngine;

public static class PathFinder {
  public static List<NavTile> FindPath(NavTile startNode, NavTile endNode) {
    List<NavTile> openList = new();
    List<NavTile> closedList = new();

    openList.Add(startNode);

    while (openList.Count > 0) {
      NavTile currentNode = openList.OrderBy(x => x.F).First();

      openList.Remove(currentNode);
      closedList.Add(currentNode);

      if (currentNode == endNode) {
        return GetFinishedList(startNode, endNode);
      }

      IEnumerable<NavTile> neighbourTiles = GetNeighbourTiles(currentNode);

      foreach (NavTile neighbour in neighbourTiles.Where(neighbour => !neighbour.isBlocked && !closedList.Contains(neighbour))) {
        neighbour.G = GetChebyshevDistance(startNode, neighbour);
        neighbour.H = GetChebyshevDistance(endNode, neighbour);
        neighbour.previous = currentNode;

        if (!openList.Contains(neighbour))
          openList.Add(neighbour);
      }
    }

    return new List<NavTile>();
  }

  private static int GetChebyshevDistance(NavTile start, NavTile neighbour) {
    return Math.Max(
      Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x),
      Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y)
    );
  }

  private static List<NavTile> GetFinishedList(NavTile startNode, NavTile endNode) {
    List<NavTile> finishedList = new();

    NavTile currentNode = endNode;

    while (currentNode != startNode) {
      finishedList.Add(currentNode);
      currentNode = currentNode.previous;
    }

    finishedList.Reverse();

    return finishedList;
  }

  private static IEnumerable<NavTile> GetNeighbourTiles(NavTile navTile) {
    Dictionary<Vector2Int, NavTile> map = MapManager.Instance.map;
    List<NavTile> neighbourTiles = new();

    // Up
    Vector2Int locationToCheck = new(
      navTile.gridLocation.x,
      navTile.gridLocation.y + 1
    );

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    // Down
    locationToCheck.x = navTile.gridLocation.x;
    locationToCheck.y = navTile.gridLocation.y - 1;

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    // Up-Left
    locationToCheck.x = navTile.gridLocation.x - 1;
    locationToCheck.y = navTile.gridLocation.y + 1;

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    // Up-Right
    locationToCheck.x = navTile.gridLocation.x + 1;
    locationToCheck.x = navTile.gridLocation.y + 1;

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    // Down-Left
    locationToCheck.x = navTile.gridLocation.x - 1;
    locationToCheck.y = navTile.gridLocation.y - 1;

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    // Down-Right
    locationToCheck.x = navTile.gridLocation.x + 1;
    locationToCheck.y = navTile.gridLocation.y - 1;

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    // Left
    locationToCheck.x = navTile.gridLocation.x - 1;
    locationToCheck.y = navTile.gridLocation.y;

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    // Right
    locationToCheck.x = navTile.gridLocation.x + 1;
    locationToCheck.y = navTile.gridLocation.y;

    if (map.ContainsKey(locationToCheck))
      neighbourTiles.Add(map[locationToCheck]);

    return neighbourTiles;
  }
}
