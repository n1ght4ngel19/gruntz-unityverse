using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GruntzUnityverse.V2.Pathfinding {
  public static class JpsPathfinder {
    public static List<NodeV2> FindPath(NodeV2 startNode, NodeV2 endNode, List<NodeV2> grid, Material green) {
      List<NodeV2> openSet = new List<NodeV2>();
      List<NodeV2> closedSet = new List<NodeV2>();

      startNode.g = 0;
      startNode.h = CalculateHeuristic(startNode, endNode);

      openSet.Add(startNode);

      while (openSet.Count != 0) {
        NodeV2 current = openSet.OrderBy(n => n.F).First();
        closedSet.Add(current);

        if (current == endNode) {
          return RetracePath(startNode, endNode);
        }

        IdentifySuccessors(current, endNode, grid, openSet, closedSet, green);
      }

      // No path found
      return new List<NodeV2>();
    }

    public static void IdentifySuccessors(
      NodeV2 current,
      NodeV2 endNode,
      List<NodeV2> grid,
      List<NodeV2> openSet,
      List<NodeV2> closedSet,
      Material mat
    ) {
      List<NodeV2> successors = new List<NodeV2>();
      List<NodeV2> neighbours = FindNeighbours(current, grid);

      foreach (NodeV2 node in neighbours) {
        NodeV2 jumpPoint = Jump(node, current, grid, endNode, mat);

        // if (jumpPoint != null && !closedSet.Contains(jumpPoint)) {
        //   if (!openSet.Contains(jumpPoint)) {
        //     openSet.Add(jumpPoint);
        //   }
        // }
        if (jumpPoint != null) {
          if (closedSet.Contains(jumpPoint)) {
            continue;
          }

          int distance = CalculateHeuristic(current, jumpPoint);
          int tentativeG = current.g + distance;

          if (!openSet.Contains(jumpPoint) || tentativeG < jumpPoint.g) {
            jumpPoint.g = tentativeG;
            // jumpPoint.h = jumpPoint.h || CalculateHeuristic(jumpPoint, endNode);
            jumpPoint.h = CalculateHeuristic(jumpPoint, endNode);
            jumpPoint.parent = current;

            if (!openSet.Contains(jumpPoint)) {
              openSet.Add(jumpPoint);
            } else {
              openSet = openSet.OrderBy(n => n.F).ToList();
            }
          }
        }
      }
    }

    public static List<NodeV2> RetracePath(NodeV2 startNode, NodeV2 endNode) {
      List<NodeV2> path = new List<NodeV2>();
      NodeV2 currentNode = endNode;

      while (currentNode != startNode) {
        path.Add(currentNode);
        currentNode = currentNode.parent;
      }

      path.Reverse();

      return path;
    }

    public static int CalculateHeuristic(NodeV2 a, NodeV2 b) {
      int dX = Mathf.Abs(a.location2D.x - b.location2D.x);
      int dY = Mathf.Abs(a.location2D.y - b.location2D.y);

      return 14 * Mathf.Min(dX, dY) + 10 * Mathf.Abs(dX - dY);
    }


    //
    // Search recursively in the direction (parent -> child), stopping only when a
    // jump point is found.
    // @protected
    // @return {Array<Array<number>>} The x, y coordinate of the jump point
    // found, or null if not found
    // 


    public static NodeV2 Jump(NodeV2 node, NodeV2 current, List<NodeV2> grid, NodeV2 end, Material mat) {
      GetNodeAt(node.location2D.x, node.location2D.y, grid).GetComponent<SpriteRenderer>().material = mat;
      int dX = node.location2D.x - current.location2D.x;
      int dY = node.location2D.y - current.location2D.y;

      if (!IsWalkableAt(node.location2D.x, node.location2D.y, grid)) {
        return null;
      }

      if (GetNodeAt(node.location2D.x, node.location2D.y, grid) == end) {
        return GetNodeAt(node.location2D.x, node.location2D.y, grid);
      }

      // Check forced neighbours along the diagonal
      if (dX != 0 && dY != 0) {
        NodeV2 n1 = GetNodeAt(node.location2D.x + dX, node.location2D.y, grid);
        NodeV2 n2 = GetNodeAt(node.location2D.x, node.location2D.y + dY, grid);

        if (Jump(n1, node, grid, end, mat) != null || Jump(n2, node, grid, end, mat) != null) {
          return node;
        }
      } else { // Moving horizontally/vertically
        if (dX != 0) {
          NodeV2 n1 = GetNodeAt(node.location2D.x - dX, node.location2D.y - 1, grid);
          NodeV2 n2 = GetNodeAt(node.location2D.x - dX, node.location2D.y + 1, grid);

          if (node.neighbourSet.down.isBlocked && !n1.isBlocked
            || node.neighbourSet.up.isBlocked && !n2.isBlocked) {
            return node;
          } else if (dY != 0) {
            NodeV2 n1b = GetNodeAt(node.location2D.x - 1, node.location2D.y - dY, grid);
            NodeV2 n2b = GetNodeAt(node.location2D.x + 1, node.location2D.y - dY, grid);

            if (node.neighbourSet.left && !n1b.isBlocked
              || node.neighbourSet.right && !n2b.isBlocked) {
              return node;
            }
          }
        }
      }

      NodeV2 n1c = GetNodeAt(node.location2D.x + dX, node.location2D.y, grid);
      NodeV2 n2c = GetNodeAt(node.location2D.x, node.location2D.y + dY, grid);

      if (n1c.isBlocked && n2c.isBlocked) {
        return Jump(n1c, n2c, grid, end, mat);
      }

      return null;
    }

    public static List<NodeV2> FindNeighbours(NodeV2 node, List<NodeV2> grid) {
      List<NodeV2> jpsNeighbours = new List<NodeV2>();
      int x, y, pX, pY, nX, nY, dX, dY;

      x = node.location2D.x;
      y = node.location2D.y;

      if (node.parent != null) {
        pX = node.parent.location2D.x;
        pY = node.parent.location2D.y;
        // Normalized direction of travel
        dX = (node.location2D.x - pX) / Mathf.Max(Mathf.Abs(node.location2D.x - pX), 1);
        dY = (node.location2D.y - pY) / Mathf.Max(Mathf.Abs(node.location2D.y - pY), 1);

        // Search diagonally
        if (dX != 0 && dY != 0) {
          if (IsWalkableAt(x, y + dY, grid)) {
            jpsNeighbours.Add(GetNodeAt(x, y + dY, grid));
          }

          if (IsWalkableAt(x + dX, y, grid)) {
            jpsNeighbours.Add(GetNodeAt(x + dX, y, grid));
          }

          if (IsWalkableAt(x, y + dY, grid) && IsWalkableAt(x + dX, y, grid)) {
            jpsNeighbours.Add(GetNodeAt(x + dX, y + dY, grid));
          }
        }

        // Search orthogonally
        else {
          bool isNextWalkable;

          if (dX != 0) {
            isNextWalkable = IsWalkableAt(x + dX, y, grid);
            bool isTopWalkable = IsWalkableAt(x, y + 1, grid);
            bool isBottomWalkable = IsWalkableAt(x, y - 1, grid);

            if (isNextWalkable) {
              jpsNeighbours.Add(GetNodeAt(x + dX, y, grid));

              if (isTopWalkable) {
                jpsNeighbours.Add(GetNodeAt(x + dX, y + 1, grid));
              }

              if (isBottomWalkable) {
                jpsNeighbours.Add(GetNodeAt(x + dX, y - 1, grid));
              }
            }

            if (isTopWalkable) {
              jpsNeighbours.Add(GetNodeAt(x, y + 1, grid));
            }

            if (isBottomWalkable) {
              jpsNeighbours.Add(GetNodeAt(x, y - 1, grid));
            }
          } else if (dY != 0) {
            isNextWalkable = IsWalkableAt(x, y + dY, grid);
            bool isRightWalkable = IsWalkableAt(x + 1, y, grid);
            bool isLeftWalkable = IsWalkableAt(x - 1, y, grid);

            if (isNextWalkable) {
              jpsNeighbours.Add(GetNodeAt(x, y + dY, grid));

              if (isRightWalkable) {
                jpsNeighbours.Add(GetNodeAt(x + 1, y + dY, grid));
              }

              if (isLeftWalkable) {
                jpsNeighbours.Add(GetNodeAt(x - 1, y + dY, grid));
              }
            }

            if (isRightWalkable) {
              jpsNeighbours.Add(GetNodeAt(x + 1, y, grid));
            }

            if (isLeftWalkable) {
              jpsNeighbours.Add(GetNodeAt(x - 1, y, grid));
            }
          }
        }
      } else {
        // Return all neighbours
        jpsNeighbours = node.neighbours;
      }

      return jpsNeighbours;
    }

    public static bool IsWalkableAt(int x, int y, List<NodeV2> grid) {
      NodeV2 possibleNode = grid.FirstOrDefault(n => n.location2D.x == x && n.location2D.y == y);

      if (possibleNode != null) {
        return possibleNode.isBlocked == false;
      } else {
        return false;
      }
    }

    public static NodeV2 GetNodeAt(int x, int y, List<NodeV2> grid) {
      return grid.FirstOrDefault(n => n.location2D.x == x && n.location2D.y == y);
    }
  }

}
