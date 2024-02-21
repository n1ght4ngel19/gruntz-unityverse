using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GruntzUnityverse.V2.Core {
public class Level : MonoBehaviour {
	/// <summary>
	/// The singleton accessor of the LevelV2.
	/// </summary>
	public static Level Instance { get; private set; }

	public Tilemap mainMap;
	public Tilemap background;

	public GameObject nodeGrid;
	public List<Node> levelNodes;
	public Node nodePrefab;

	public string levelName;
	public LevelStatz levelStatz;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
	}

	public void Initialize() {
		// Clear existing nodes
		nodeGrid.GetComponentsInChildren<Node>().ToList().ForEach(n => DestroyImmediate(n.gameObject));
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
				.SetupNode(position, tile.name, levelNodes);
		}

		// Assign neighbours to all nodes AFTER all nodes have been created
		levelNodes.ForEach(n => n.AssignNeighbours(levelNodes));

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Blocker")) {
			Vector2Int position = Vector2Int.RoundToInt(go.transform.position);
			levelNodes.FirstOrDefault(n => n.location2D.Equals(position))!.isBlocked = true;
		}
	}
}
}
