using System.Linq;
using GruntzUnityverse.V2.Pathfinding;
using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2.Core {
public class Selector : MonoBehaviour {
	public Vector2Int location2D;
	public NodeV2 node;
	public Camera mainCamera;

	private void Start() {
		mainCamera = Camera.main;
	}

	private void Update() {
		transform.position = Input.mousePosition.FromCameraView(mainCamera).RoundedToInt(15f);
		location2D = Vector2Int.RoundToInt(transform.position);
		node = LevelV2.Instance.levelNodes.FirstOrDefault(n => n.location2D == location2D);
	}
}
}
