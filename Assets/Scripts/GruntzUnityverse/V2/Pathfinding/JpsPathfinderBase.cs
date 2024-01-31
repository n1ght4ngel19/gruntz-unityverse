// using System.Collections.Generic;
// using System.Linq;
//
// namespace GruntzUnityverse.V2.Pathfinding {
//   public class JpsPathfinderBase {
//     public static List<NodeV2> JumpPointSearch(NodeV2 start, NodeV2 end, List<NodeV2> all) {
//       // openSet contains nodes that have been discovered but not yet visited
//       List<NodeV2> openSet = new List<NodeV2>();
//       // closedSet contains nodes that have been visited
//       List<NodeV2> closedSet = new List<NodeV2>();
//       int nodesVisited = 0;
//
//       start.g = 0;
//
//       // Add start node to openSet
//       openSet.Add(start);
//
//       while (openSet.Count != 0) {
//         NodeV2 current = openSet.First();
//         closedSet.Add(current);
//
//         if (current == end) {
//           // Retrace path
//           return new List<NodeV2>();
//         }
//
//         // IdentifySuccessors(current, openSet, closedSet);
//       }
//
//       // Return empty list when no path is found
//       return new List<NodeV2>();
//     }
//
//     // public static void GetSuccessors(NodeV2 node, List<NodeV2> openSet, List<NodeV2> closedSet) {
//     //   List<NodeV2> successors = new List<NodeV2>();
//     //
//     //   foreach (NodeV2 neighbour in node.neighbours) {
//     //     NodeV2 jumpPoint = Jump(neighbour, node);
//     //
//     //     if (jumpPoint != null) {
//     //       if (closedSet.Contains(jumpPoint)) {
//     //         continue;
//     //       }
//     //
//     //       int distance = Heuristic(node, jumpPoint);
//     //       int nextG = node.g + distance;
//     //
//     //       if (nextG < jumpPoint.g) {
//     //         jumpPoint.g = nextG;
//     //         // jumpNode.h = jumpNode.h || heuristic(abs(jx - endX), abs(jy - endY));
//     //         jumpPoint.h = jumpPoint.h || Heuristic(jumpPoint, node);
//     //         jumpPoint.parent = node;
//     //
//     //         if (!openSet.Contains(jumpPoint)) {
//     //           openSet.Add(jumpPoint);
//     //         } else {
//     //           // openSet.updateItem(jumpNode);
//     //           NodeV2 openNode = openSet.First(n => n == jumpPoint);
//     //           openNode.g = jumpPoint.g;
//     //           openNode.h = jumpPoint.h;
//     //           openNode.parent = jumpPoint.parent;
//     //         }
//     //       }
//     //     }
//     //   }
//     // }
//     
//     
//   }
// }
