using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GruntzUnityverse.V2 {
  public class LevelV2 : MonoBehaviour {
    public Tilemap mainMap;
    public Tilemap background;

    public GameObject nodeGrid;
    public List<NodeV2> levelNodes;
    public NodeV2 nodePrefab;

    public void Initialize() {
      BoundsInt bounds = mainMap.cellBounds;

      // Iterate through all tiles in the map
      foreach (Vector3Int position in bounds.allPositionsWithin) {
        TileBase tile = mainMap.GetTile(position);

        if (tile == null) {
          continue;
        }

        // Create new node and set it up
        Instantiate(nodePrefab, mainMap.GetCellCenterLocal(position), Quaternion.identity, nodeGrid.transform)
          .Setup(position, tile.name, levelNodes);
      }

      // Assign neighbours to all nodes AFTER all nodes have been created
      levelNodes.ForEach(n => n.AssignNeighbours(levelNodes));
    }
  }
}
