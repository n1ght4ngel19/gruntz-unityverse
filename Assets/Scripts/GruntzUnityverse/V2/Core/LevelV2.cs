using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace GruntzUnityverse.V2.Core {
  public class LevelV2 : MonoBehaviour {
    /// <summary>
    /// The singleton accessor of the LevelV2.
    /// </summary>
    public static LevelV2 Instance { get; private set; }

    public Tilemap mainMap;
    public Tilemap background;

    public GameObject nodeGrid;
    public List<NodeV2> levelNodes;
    public NodeV2 nodePrefab;

    private void Awake() {
      if (Instance != null && Instance != this) {
        Destroy(gameObject);
      } else {
        Instance = this;
      }
    }

    public void Initialize() {
      // Clear existing nodes
      nodeGrid.GetComponentsInChildren<NodeV2>().ToList().ForEach(n => DestroyImmediate(n.gameObject));
      levelNodes.Clear();

      // Compress bounds to avoid iterating through empty tiles
      mainMap.CompressBounds();

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
