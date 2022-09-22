using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class PathFinder {
    public List<NavTile> FindPath(NavTile start, NavTile end) {
        List<NavTile> openList = new List<NavTile>();
        List<NavTile> closedList = new List<NavTile>();
        
        openList.Add(start);

        while (openList.Count > 0) {
            NavTile current = openList.OrderBy(x => x.F).First();
            
            openList.Remove(current);
            closedList.Add(current);

            if (current == end) {
                return GetFinishedList(start, end);
            }
            
            var neighbourTiles = GetNeighbourTiles(current);

            foreach (var neighbour in neighbourTiles) {
                if (neighbour.isBlocked || closedList.Contains(neighbour)) {
                    continue;                    
                }

                neighbour.G = GetChebyshevDistance(start, neighbour);
                neighbour.H = GetChebyshevDistance(end, neighbour);
                neighbour.previous = current;
                
                if (!openList.Contains(neighbour)) {
                    openList.Add(neighbour);
                }
            }
        }

        return new List<NavTile>();
    }

    private int GetChebyshevDistance(NavTile start, NavTile neighbour) {
        return Math.Max(
            Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x),
            Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y)
        );
    }

    private List<NavTile> GetFinishedList(NavTile start, NavTile end) {
        List<NavTile> finishedList = new List<NavTile>();
        
        NavTile current = end;

        while (current != start) {
            finishedList.Add(current);
            current = current.previous;
        }

        finishedList.Reverse();
        
        return finishedList;
    }

    private List<NavTile> GetNeighbourTiles(NavTile navTile) {
        var map = MapManager.Instance.map;
        List<NavTile> neighbourTiles = new List<NavTile>();
        
        // Up
        Vector2Int locationToCheck = new Vector2Int(
            navTile.gridLocation.x,
            navTile.gridLocation.y + 1
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        // Down
        locationToCheck = new Vector2Int(
            navTile.gridLocation.x,
            navTile.gridLocation.y - 1
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        // Up-Left
        locationToCheck = new Vector2Int(
            navTile.gridLocation.x - 1,
            navTile.gridLocation.y + 1
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        // Up-Right
        locationToCheck = new Vector2Int(
            navTile.gridLocation.x + 1,
            navTile.gridLocation.y + 1
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        // Down-Left
        locationToCheck = new Vector2Int(
            navTile.gridLocation.x - 1,
            navTile.gridLocation.y - 1
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        // Down-Right
        locationToCheck = new Vector2Int(
            navTile.gridLocation.x + 1,
            navTile.gridLocation.y - 1
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        // Left
        locationToCheck = new Vector2Int(
            navTile.gridLocation.x - 1,
            navTile.gridLocation.y
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        // Right
        locationToCheck = new Vector2Int(
            navTile.gridLocation.x + 1,
            navTile.gridLocation.y
        );

        if (map.ContainsKey(locationToCheck))
            neighbourTiles.Add(map[locationToCheck]);

        return neighbourTiles;
    }
}
