﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.V2.Pathfinding {
  /// <summary>
  /// Custom representation of a node used for pathfinding.
  /// </summary>
  public class NodeV2 : MonoBehaviour {
    /// <summary>
    /// The location of this NodeV2 in 2D space.
    /// </summary>
    public Vector2Int location2D;

    /// <summary>
    /// The SpriteRenderer of this NodeV2.
    /// </summary>
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// The collider used for checking interactions with this NodeV2.
    /// </summary>
    protected CircleCollider2D circleCollider2D;


    #region Pathfinding
    // --------------------------------------------------
    // Pathfinding
    // --------------------------------------------------
    /// <summary>
    /// The parent NodeV2 of this NodeV2, used for retracing paths.
    /// </summary>
    [Header("Pathfinding")]
    public NodeV2 parent;

    /// <summary>
    /// Cost from start to this node.
    /// </summary>
    public int g;

    /// <summary>
    /// Heuristic (cost from this NodeV2 to end).
    /// </summary>
    public int h;

    /// <summary>
    /// Total cost (g + h).
    /// </summary>
    public int F => g + h;

    /// <summary>
    /// The neighbours of this NodeV2.
    /// </summary>  
    public List<NodeV2> neighbours;

    /// <summary>
    /// The diagonal neighbours of this NodeV2.
    /// </summary>
    public List<NodeV2> diagonalNeighbours;

    /// <summary>
    /// The orthogonal neighbours of this NodeV2.
    /// </summary>
    public List<NodeV2> orthogonalNeighbours;

    /// <summary>
    /// Serializable representation of the neighbours of this NodeV2.
    /// </summary>
    public NeighbourSet neighbourSet;
    #endregion

    #region Flags
    // --------------------------------------------------
    // Flags
    // --------------------------------------------------
    /// <summary>
    /// If true, this NodeV2 is occupied by an actor.
    /// </summary>
    [Header("Flags")]
    public bool isOccupied;

    /// <summary>
    /// If true, this NodeV2 is blocked by a wall or other obstacle.
    /// </summary>
    public bool isBlocked;

    public bool isWater;
    public bool isFire;
    public bool isVoid;

    /// <summary>
    /// If true, this NodeV2 is walkable. This combines the 'occupied' and 'blocked' flags.
    /// </summary>
    public bool IsWalkable {
      get => !isOccupied && !isBlocked && !isWater && !isFire && !isVoid;
    }

    /// <summary>
    /// If true, this NodeV2 is walkable with a Toob equipped.
    /// </summary>
    public bool IsWalkableWithToob {
      get => !isOccupied && !isBlocked && !isFire && !isVoid;
    }

    /// <summary>
    /// If true, this NodeV2 is walkable with Wingz equipped.
    /// </summary>
    public bool IsWalkableWithWingz {
      get => !isOccupied;
    }

    /// <summary>
    /// If true, this NodeV2 is a hard corner, meaning that it prevents
    /// diagonal movement to its neighbours.
    /// </summary>
    public bool hardCorner;
    #endregion

    public TileType tileType;

    /// <summary>
    /// Sets up this NodeV2 with the given parameters.
    /// </summary>
    /// <param name="position">The position of this NodeV2 in the grid. </param>
    /// <param name="tileName">The name of the tile this NodeV2 is based on.</param>
    /// <param name="nodes">The list of all nodes in the grid.</param>
    public void Setup(Vector3Int position, string tileName, List<NodeV2> nodes) {
      transform.position = position - Vector3Int.one / 2;
      location2D = new Vector2Int(position.x, position.y);
      AssignTileType(tileName);
      nodes.Add(this);
    }

    /// <summary>
    /// Assigns the tile type of this NodeV2 based on the given tile name.
    /// </summary>
    /// <param name="tileName"></param>
    private void AssignTileType(string tileName) {
      if (tileName.Contains("Ground")) {
        tileType = TileType.Ground;
      } else if (tileName.Contains("Collision")) {
        tileType = TileType.Collision;
        isBlocked = true;
      } else if (tileName.Contains("Water")) {
        tileType = TileType.Water;
        isWater = true;
      } else if (tileName.Contains("Fire")) {
        tileType = TileType.Fire;
        isFire = true;
      } else if (tileName.Contains("Void")) {
        tileType = TileType.Void;
        isVoid = true;
      } else {
        tileType = TileType.Unknown;
      }
    }

    /// <summary>
    /// Returns whether this NodeV2 can be reached diagonally from the given NodeV2.
    /// </summary>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    public bool CannotReachDiagonally(NodeV2 toCheck) {
      return IsDiagonalTo(toCheck)
        && neighbours.Any(n => n.hardCorner && toCheck.neighbours.Contains(n));
    }

    /// <summary>
    /// Assigns the neighbours of this NodeV2 based from the given list of nodes.
    /// </summary>
    /// <param name="nodes"></param>
    public void AssignNeighbours(List<NodeV2> nodes) {
      neighbourSet = new NeighbourSet {
        up = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.up),
        upRight = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.up + Vector2Int.right),
        right = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.right),
        downRight = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.down + Vector2Int.right),
        down = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.down),
        downLeft = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.down + Vector2Int.left),
        left = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.left),
        upLeft = nodes.FirstOrDefault(n => n.location2D == location2D + Vector2Int.up + Vector2Int.left),
      };

      neighbours = neighbourSet.AsList();

      diagonalNeighbours = new List<NodeV2> {
        neighbourSet.upRight,
        neighbourSet.downRight,
        neighbourSet.downLeft,
        neighbourSet.upLeft,
      };

      orthogonalNeighbours = new List<NodeV2> {
        neighbourSet.up,
        neighbourSet.right,
        neighbourSet.down,
        neighbourSet.left,
      };
    }

    /// <summary>
    /// Returns whether this NodeV2 is diagonal to the other given NodeV2.
    /// </summary>
    /// <param name="toCheck">The NodeV2 to check.</param>
    /// <returns>A boolean indicating whether this NodeV2 is diagonal to the other given NodeV2.</returns>
    private bool IsDiagonalTo(NodeV2 toCheck) {
      return Math.Abs(toCheck.location2D.x - location2D.x) != 0
        && Math.Abs(toCheck.location2D.y - location2D.y) != 0;
    }

    public void SetColor(Color color) {
      GetComponent<SpriteRenderer>().material.color = color;
    }
  }
}