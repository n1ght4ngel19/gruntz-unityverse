using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Misc;
using GruntzUnityverse.Objectz.Switchez;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GruntzUnityverse.Core {
public class Level : MonoBehaviour {
	/// <summary>
	/// The singleton accessor of the Level.
	/// </summary>
	public static Level instance { get; private set; }

	public Tilemap mainMap;
	public Tilemap background;

	public GameObject nodeGrid;
	public HashSet<Node> levelNodes;
	public Node nodePrefab;

	public string levelName;
	public LevelStatz levelStatz;

	private void OnDrawGizmosSelected() {
		transform.position = new Vector3(-0.5f, -0.5f, 0);
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	private void OnValidate() {
		transform.hideFlags = HideFlags.HideInInspector;
	}

	private void Awake() {
		instance = this;
		levelNodes = FindObjectsByType<Node>(FindObjectsSortMode.None).ToHashSet();
	}

	public void Initialize() {
		// Clear existing nodes
		nodeGrid.GetComponentsInChildren<Node>().ToList().ForEach(n => DestroyImmediate(n.gameObject));
		levelNodes = new HashSet<Node>();

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
				.SetupNode(position, tile.name);
		}

		HashSet<Node> allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None).ToHashSet();

		// Assign neighbours to all nodes after all nodes have been created
		foreach (Node node in allNodes) {
			node.AssignNeighbours(allNodes);
		}

		levelStatz.maxToolz = FindObjectsByType<LevelTool>(FindObjectsSortMode.None).Length;
		levelStatz.maxToyz = FindObjectsByType<LevelToy>(FindObjectsSortMode.None).Length;
		levelStatz.maxPowerupz = FindObjectsByType<LevelPowerup>(FindObjectsSortMode.None).Length;
		levelStatz.maxCoinz = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;
		levelStatz.maxSecretz = FindObjectsByType<SecretSwitch>(FindObjectsSortMode.None).Length;
		levelStatz.maxWarpletterz = FindObjectsByType<Warpletter>(FindObjectsSortMode.None).Length;

		Debug.Log(levelNodes.Count);
		Debug.Log(levelName is null);
	}
}
}
